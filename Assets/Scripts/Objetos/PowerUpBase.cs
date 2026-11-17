using UnityEngine; 
public abstract class PowerUpBase : MonoBehaviour
{
    //clase abstracta para ahorrar lineas de codigos y hacer uso de la herencia mediante este power up base
    protected float duracion;// duracion del power up estando activo
    protected int posicion;// ubicacion
    protected float despawn;//tiempo en desaparecer 
    protected bool activo = false;// bool util para manejar bucles

    protected virtual void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.powerUpcadencia,1f,1f);
    }
    protected virtual void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si el jugador colisiona con el power up
        if (otroObjeto.CompareTag("Jugador"))
        {
            ControladorJugador jg = otroObjeto.gameObject.GetComponent<ControladorJugador>();//obtenemos la instancia del jugador
            activarPowerUp(jg);//activamos el poder en el jugador
            GameManager.Instancia.agregarTiempo(duracion);// agregamos el tiempo del power up
            GameManager.Instancia.agregarPosicionOcupada(posicion);// agregamos la posicion que esta ocupada
            reproducirAudio(); // Sonido de Power Up  
            Destroy(gameObject);
        }
    }
    //funcion que asigna la posicion en la que esta ubicado este power up y le da el valor de despawn 
    public virtual void asignarPosicion(int lugar)
    {
        activo = true;
        posicion = lugar;
        despawn = Time.time + 8f;
    }
    //funcion para activar el power up en el jugador o entidad
    protected virtual void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 5f;
        jg.activarPowerUp("cadencia",duracion);
    }
    
    void Update()
    {
        gestionarOpacidad();// funcion para bajar opacidad de los efectos que no son agarrados
        if(Time.time >= despawn && activo)// si paso el tiempo despawn, se destruye 
        {
            Destroy(gameObject);
            GameManager.Instancia.desocuparPosicion(posicion);
            activo = false;
        }
    }
    //funcion para gestionar la opacidad mediante el tiempo transcurrido de la variable despawn
    public virtual void gestionarOpacidad()
    {
        SpriteRenderer miSprite = GetComponent<SpriteRenderer>();
        if(Time.time >= despawn-1f)
        {
            Color colorActual = miSprite.color;
            colorActual.a = 0.125f;
            miSprite.color = colorActual;
        }
        else if(Time.time >= despawn-2f)
        {
            Color colorActual = miSprite.color;
            colorActual.a = 0.25f;
            miSprite.color = colorActual;
        }
        else if(Time.time >= despawn-4f)
        {
            Color colorActual = miSprite.color;
            colorActual.a = 0.50f;
            miSprite.color = colorActual;
        }
        else if(Time.time >= despawn-6f)
        {
            Color colorActual = miSprite.color;
            colorActual.a = 0.75f;
            miSprite.color = colorActual;
        }
    }

} 
