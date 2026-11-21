using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    public float velocidad = 10f; // Velocidad de movimiento de la nave
    public float limiteX; // Límite lateral para que no se salga de la pantalla
    public GameObject prefabDisparo; // Referencia al objeto del disparo que crearemos después
    public Transform puntoDisparo;// Punto desde donde sale el disparo (frente de la nave)
    public float tiempoCooldown = 1f; // Segundos de espera entre disparo
    public float duracionPowerUp; // Segundos de uso del Power up activo
    private float siguienteDisparoDisponible = 0f;// Controla cuándo se podrá volver a atacar
    private bool powerUpActivo = false;//gestionar el power up activo
    private bool invensible = false;// gestionar el power up de rebote
    void Update()
    {
        // 1. Obtener la entrada del teclado (Flechas o A/D)
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // 2. Mover la nave en el eje X
        transform.Translate(Vector3.right * movimientoHorizontal * velocidad * Time.deltaTime);

        // 3. Restringir la posición para que no cruce los límites de la pantalla
        float posicionXLimitada = Mathf.Clamp(transform.position.x, -limiteX-1, limiteX+1);
        transform.position = new Vector3(posicionXLimitada, transform.position.y, transform.position.z);

        // 4. Detectar si el jugador presiona la Barra Espaciadora para disparar
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= siguienteDisparoDisponible)
        {
            Disparar();
        }
        //bloque de codigo util para desactivar los efectos de los powerUps
        if (powerUpActivo && Time.time >= duracionPowerUp)
        {
            SpriteRenderer miSprite = GetComponent<SpriteRenderer>();
            Color colorActual = miSprite.color;
            colorActual.a = 1f;// vuelve su opacidad normal
            miSprite.color = colorActual;
            cambiarCadencia(0.7f);//vuelve su cadencia default
            powerUpActivo = false;
            invensible = false;
        }
    }

    void Disparar()
    {
        // Crea una copia del disparo en la posición del "puntoDisparo"
        Instantiate(prefabDisparo, puntoDisparo.position, Quaternion.identity);
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoJugador,1f,1f); // Sonido de disparo
        siguienteDisparoDisponible = Time.time + tiempoCooldown;
    }
    
    //Detectar si el disparo enemigo nos golpea
    private void OnTriggerEnter2D(Collider2D otroObjeto) 
        { 
            // Detectamos si nos golpea un disparo enemigo 
            if (otroObjeto.CompareTag("DisparoEnemigo")&& !invensible) 
            { 
                // 1. Reproducir sonido de explosión 
                if (AudioManager.Instancia != null) 
                { 
                    
                    AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoHit,1f,1f); 
                } 
        
                // 2. Destruir el proyectil que nos golpeó 
                Destroy(otroObjeto.gameObject);  

                // 3. Avisar al GameManager que nos reste una vida y puntos
                GameManager.Instancia.RestarVida(); 
                GameManager.Instancia.restarPuntos(40); 
        
                // 4. Si el jugador se quedó sin vidas, destruimos la nave física 
                if (GameManager.Instancia.vidas <= 0) 
                { 
                    Destroy(gameObject); 
                } 
                else 
                { 
                // Opcional: Centrar la nave de nuevo al recibir daño como penalización/reaparición 
                transform.position = new Vector3(0f, transform.position.y, transform.position.z); 
                } 
            }
            //bloque para gestionar cuando tenemos nuestro power up de rebote activo
            if(otroObjeto.CompareTag("DisparoEnemigo")&& invensible)
            {
                Disparar();
                AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoHitDesviado,1f,1f);
                Destroy(otroObjeto.gameObject);   
            }
        }
    // funcion para cambiar la cadencia
    public void cambiarCadencia(float cadencia)
    {
        tiempoCooldown = cadencia;
    }
    //activar power ups dependiendo de string y duracion por parametro
    public void activarPowerUp(string poder,float duracion)
    {
        powerUpActivo =true;
        duracionPowerUp = duracion;
        SpriteRenderer miSprite = GetComponent<SpriteRenderer>();
        if(poder == "cadencia")
        {
            cambiarCadencia(0.3f);
        }
        if(poder == "rebote")
        {
            Color colorActual = miSprite.color;
            colorActual.a = 0.25f;
            miSprite.color = colorActual;
            invensible = true;
        }
    }
    

}
