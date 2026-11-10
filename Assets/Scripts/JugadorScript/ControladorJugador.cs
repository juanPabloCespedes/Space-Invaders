using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    // Velocidad de movimiento de la nave
    public float velocidad = 10f; 
    // Límite lateral para que no se salga de la pantalla
    public float limiteX = 8f; 
    // Referencia al objeto del disparo que crearemos después
    public GameObject prefabDisparo; 
    // Punto desde donde sale el disparo (frente de la nave)
    public Transform puntoDisparo;
    public float tiempoCooldown = 1f; // Segundos de espera entre disparo
    private float siguienteDisparoDisponible = 0f;          // Controla cuándo se podrá volver a atacar

    void Update()
    {
        // 1. Obtener la entrada del teclado (Flechas o A/D)
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // 2. Mover la nave en el eje X
        transform.Translate(Vector3.right * movimientoHorizontal * velocidad * Time.deltaTime);

        // 3. Restringir la posición para que no cruce los límites de la pantalla
        float posicionXLimitada = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(posicionXLimitada, transform.position.y, transform.position.z);

        // 4. Detectar si el jugador presiona la Barra Espaciadora para disparar
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= siguienteDisparoDisponible)
        {
            Disparar();
        }
    }

    void Disparar()
    {
        // Crea una copia del disparo en la posición del "puntoDisparo"
        Instantiate(prefabDisparo, puntoDisparo.position, Quaternion.identity);
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoJugador); // Sonido de disparo
        siguienteDisparoDisponible = Time.time + tiempoCooldown;
    }
    
    //Detectar si el disparo enemigo nos golpea
    private void OnTriggerEnter2D(Collider2D otroObjeto) 
        { 
            // Detectamos si nos golpea un disparo enemigo 
            if (otroObjeto.CompareTag("DisparoEnemigo")) 
            { 
                // 1. Reproducir sonido de explosión 
                if (AudioManager.Instancia != null) 
                { 
                    
                AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion); 
                } 
        
                // 2. Destruir el proyectil que nos golpeó 
                Destroy(otroObjeto.gameObject);  
        
                // 3. Avisar al GameManager que nos reste una vida 
                GameManager.Instancia.RestarVida(); 
        
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
        }

}
