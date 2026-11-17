using UnityEngine; 
public abstract class PowerUpBase : MonoBehaviour
{
    protected float duracion;
    protected int posicion;
    protected float despawn;
    protected bool activo = false;

    protected virtual void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.powerUpcadencia);
    }
    protected virtual void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si el jugador colisiona con el power up
        if (otroObjeto.CompareTag("Jugador"))
        {
            ControladorJugador jg = otroObjeto.gameObject.GetComponent<ControladorJugador>();
            activarPowerUp(jg);
            GameManager.Instancia.agregarTiempo(duracion);
            GameManager.Instancia.agregarPosicionOcupada(posicion);
            reproducirAudio(); // Sonido de Power Up  
            Destroy(gameObject);
        }
    }
    public virtual void asignarPosicion(int lugar)
    {
        activo = true;
        posicion = lugar;
        despawn = Time.time + 8f;
    }

    protected virtual void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        jg.activarPowerUp("cadencia",duracion);
    }
    
    void Update()
    {
        if(Time.time >= despawn && activo)
        {
            Destroy(gameObject);
            GameManager.Instancia.desocuparPosicion(posicion);
            activo = false;
        }
    }

} 
