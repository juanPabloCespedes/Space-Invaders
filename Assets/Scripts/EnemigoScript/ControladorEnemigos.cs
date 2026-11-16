using UnityEngine;

public class ControladorEnemigos : MonoBehaviour
{
    public float velocidad = 3f;
    public float velocidadInicial =3f;
    public float distanciaAlBajar = 0.5f;
    public float limiteLateral = 7f;
    public int direccion = 1;  // Dirección actual: 1 significa derecha, -1 significa izquierda
    
    //Variables para el disparo enemigo
    public GameObject prefabDisparoEnemigo;
    public float tiempoEntreDisparos = 1.5f; 
    private float cronometroDisparo;
    private float duracionPowerUp;
    private bool powerUpActivo =false;
    //Límite inferior donde si los enemigos llegan, pierdes
    public float limitePerderY = -3f; 
    void Start()
    {
        cronometroDisparo = tiempoEntreDisparos;
    }

    void Update()
    {
        // Mueve a todo el grupo en la dirección actual
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);

        // Revisar la posición de cada hijo (cada enemigo individual)
        foreach (Transform enemigo in transform)
        {
            // Si algún enemigo toca el límite derecho yendo a la derecha, o el izquierdo yendo a la izquierda
            if ((direccion == 1 && enemigo.position.x >= limiteLateral) || (direccion == -1 && enemigo.position.x <= -limiteLateral))
            {
                CambiarDireccionYInvertir();
                break; // Salimos del bucle para evitar que baje múltiples veces en el mismo frame
            }

            if (enemigo.position.y <= limitePerderY)
            {
                GameManager.Instancia.ActivarGameOver("perdiste");
            }

        }
        //Control del disparo aleatorio
        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0f && transform.childCount > 0)
        {
            DispararEnemigoAleatorio();
            cronometroDisparo = tiempoEntreDisparos; // Reiniciar cronómetro
        }
        if (powerUpActivo && Time.time >= duracionPowerUp)
        {
            setVelocidad(velocidadInicial);
            powerUpActivo = false;
        }         
    }

    void CambiarDireccionYInvertir()
    {
        // Invierte la dirección (si era 1 pasa a -1, si era -1 pasa a 1)
        direccion *= -1;

        // Hace que todo el bloque de enemigos baje un poco en el eje Y
        transform.position = new Vector2(transform.position.x, transform.position.y - distanciaAlBajar);
    }
    void DispararEnemigoAleatorio()
    {
        // Elige un enemigo al azar de los que queden vivos
        int indiceAleatorio = Random.Range(0, transform.childCount);
        Transform enemigoElegido = transform.GetChild(indiceAleatorio);
        EnemigoGestor enemigo = enemigoElegido.gameObject.GetComponent<EnemigoGestor>();
        
        // Genera el disparo en la posición de ese enemigo si es que esta vivo
        if(enemigo.getEstaVivo())
        {
            //Disparo
            Instantiate(prefabDisparoEnemigo, enemigoElegido.position, Quaternion.identity);
            // Sonido de disparo enemigo
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoEnemigo);    
        }
        
    }
    public void activarLentitud(float duracion)
    {
        powerUpActivo =true;
        duracionPowerUp = duracion;
        velocidad = velocidad / 2;
    }
    private void setVelocidad(float valor)
    {
        velocidad = valor;
    }
}

