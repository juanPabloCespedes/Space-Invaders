using UnityEngine;

public class ControladorTitan : MonoBehaviour
{
    public float velocidad = 3f;
    private float limiteLateral = -16f;
    private int direccion = -1; 
    public GameObject prefabDisparoEnemigo;
    public float tiempoEntreDisparos = 1.5f; 
    private float cronometroDisparo;
    public float velocidadDeDisparo;
    private EnemigoGestor enemigo;//controlar la vida
    void Start()
    {
        cronometroDisparo = tiempoEntreDisparos;
        enemigo = GetComponent<EnemigoGestor>();
    }
    void Update()
    {   
        //direccion del titan
         transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);
         if (transform.position.x < limiteLateral)//si passa los limites se destruye
            {
                Debug.Log("Destruyendo nave suepro el limite");
                Destroy(gameObject);
            }
        //Control del disparo aleatorio
        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0f)
        {
             Debug.Log("Se cumplió el tiempo para disparar");
            DispararEnemigoAleatorio();
            cronometroDisparo = tiempoEntreDisparos; // Reiniciar cronómetro
            Debug.Log("Disparooo!!");
        }
    }
    //funcion de disparo aleatorio
    private void DispararEnemigoAleatorio()
    {   
        // Genera el disparo en la posición de ese enemigo si es que esta vivo
        if(enemigo.getEstaVivo())
        {
            //Disparo
            GameObject disparo = Instantiate(prefabDisparoEnemigo, transform.position, Quaternion.identity);
            MovimientoDisparoEnemigo md = disparo.gameObject.GetComponent<MovimientoDisparoEnemigo>();
            md.setVelocidadDeDisparo(velocidadDeDisparo);// seteamos la velocidad de disparo conforme el nivel
            // Sonido de disparo enemigo
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoTitan,0.8f,1f);    
        }   
    }
    //sets de velocidad
    public void setVelocidadDeDisparo(float velocidad)
    {
        velocidadDeDisparo = velocidad;
    }
    public void setVelocidad(float valor)
    {
        velocidad = valor;
    }
    public void setTiempoEntreDisparos(float valor)
    {
        tiempoEntreDisparos = valor;
    }
    //funcion donde recibe los argumentos para asignar su propia velocidad, velocidad de disparo y tiempo entre disparo
    public void asignarValoresDeVelocidad(float velocidad,float velocidadDeDisparo, float tiempoEntreDisparo)
    {
        setVelocidad(velocidad);
        setVelocidadDeDisparo(velocidadDeDisparo);
        setTiempoEntreDisparos(tiempoEntreDisparo);
        
    }
}