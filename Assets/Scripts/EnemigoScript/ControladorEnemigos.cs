using UnityEngine;

public class ControladorEnemigos : MonoBehaviour
{
    public static ControladorEnemigos Instancia;
    public float velocidad = 3f;
    public float velocidadInicial =3f;
    public float distanciaAlBajar = 0.5f;
    private float limiteLateral = 14f;
    public int direccion = 1;  // Dirección actual: 1 significa derecha, -1 significa izquierda
    public int enemigosVivos;//enemigos vivos
    //Variables para el disparo enemigo
    public GameObject prefabDisparoEnemigo;
    public float tiempoEntreDisparos = 1.5f; 
    private float cronometroDisparo;
    private float duracionPowerUp;//duracion del power up activo 
    private bool powerUpActivo =false;//gestionar el power up 
    //Límite inferior donde si los enemigos llegan, pierdes
    public float limitePerderY = -3f; 
    public float velocidadDeDisparo;
    void Awake()
    {
        // Configura el patrón Singleton para acceso global
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }
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
            if ((direccion == 1 && enemigo.position.x >= limiteLateral) || (direccion == -1 && enemigo.position.x <= -limiteLateral-1))
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
        // bloque para detener efectos activos
        if (powerUpActivo && Time.time >= duracionPowerUp)
        {
            setVelocidad(velocidadInicial);
            powerUpActivo = false;
        }
        ContarEnemigos();       
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
            GameObject disparo = Instantiate(prefabDisparoEnemigo, enemigoElegido.position, Quaternion.identity);
            MovimientoDisparoEnemigo md = disparo.gameObject.GetComponent<MovimientoDisparoEnemigo>();
            md.setVelocidadDeDisparo(velocidadDeDisparo);
            // Sonido de disparo enemigo
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoEnemigo,1f,1f);    
        }   
    }
    // funcion para activar la lentitud en los enmigos
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
    //funcion para obtener la cantidad de enemigos restantes
    public int ContarEnemigos()
    {
        EnemigoGestor[] enemigos = GetComponentsInChildren<EnemigoGestor>();
        enemigosVivos = enemigos.Length;
        return enemigosVivos;
    }
    //bool que sirve para determinar el endgame 
    public bool  cantidadDeEnemigos()
    {
        EnemigoGestor[] enemigos = GetComponentsInChildren<EnemigoGestor>();
        enemigosVivos = enemigos.Length;
        return enemigosVivos <= 0;
    } 
}

