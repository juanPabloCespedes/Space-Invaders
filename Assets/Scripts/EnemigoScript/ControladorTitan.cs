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
    private EnemigoGestor enemigo;
    void Start()
    {
        cronometroDisparo = tiempoEntreDisparos;
        enemigo = GetComponent<EnemigoGestor>();
    }
    void Update()
    {
         transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);
         if (transform.position.x < limiteLateral)
            {
                Debug.Log("Destruyendo nave suepro el limite");
                Destroy(gameObject);
            }
        //Control del disparo aleatorio
        cronometroDisparo -= Time.deltaTime;
        if (cronometroDisparo <= 0f)
        {
            DispararEnemigoAleatorio();
            cronometroDisparo = tiempoEntreDisparos; // Reiniciar cronómetro
            Debug.Log("Disparooo!!");
        }
    }
    private void DispararEnemigoAleatorio()
    {   

         
        // Genera el disparo en la posición de ese enemigo si es que esta vivo
        if(enemigo.getEstaVivo())
        {
            //Disparo
            GameObject disparo = Instantiate(prefabDisparoEnemigo, transform.position, Quaternion.identity);
            MovimientoDisparoEnemigo md = disparo.gameObject.GetComponent<MovimientoDisparoEnemigo>();
            md.setVelocidadDeDisparo(velocidadDeDisparo);
            // Sonido de disparo enemigo
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoDisparoTitan,1f,1f);    
        }   
    }
}