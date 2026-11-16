using UnityEngine;
using TMPro; // Necesario para controlar textos de TextMeshPro
using UnityEngine.SceneManagement; // Necesario para reiniciar escenas
using System.Collections.Generic;// gestionar spawns de powerups 
public class GameManager : MonoBehaviour
{
    // Instancia estática para que cualquier script pueda acceder al GameManager fácilmente
    public static GameManager Instancia;
    public int vidas = 3;
    public int puntuacion = 0;
    public TMP_Text textoVidas;  
    public TMP_Text textoPuntuacion;
    public TMP_Text textoGameOver; // Asegúrate de ocultarlo al empezar en Unity
    public TMP_Text textoGameWin; // Asegúrate de ocultarlo al empezar en Unity
    private bool juegoTerminado = false;
    public GameObject prefabPowerUpCadencia;
    public GameObject prefabPowerUpLentitud; 
    public int cantidadDePowerUpsActivos = 0; // power ups activos
    public int cantidadMaximaDePowerUps; // maximo de power ups
    public int enemigosVivos; // cantidad de enemigos vivos
    private float duracion = 5f;//duracion de power up 
    List<int> posicionesOcupadas = new List<int>();//lista para manejar spawn de power ups
    List<float> listaDeTiempos = new List<float>();//lista para manejar los tiempos de los power ups
    List<int> ordenDePosicionesOcupadas = new List<int>();//lista para manejar el orden de uso de posiciones
    int numeroRandom;// numero random para manejar tipos de power up 
    int posicionRandom;// numero random para manejar diferentes posiciones de power up 
    float tiempoActual;// tiempo variable de bonificacion de puntos al final del nivel
    int tiempoEntero; // WiP
    private int posicionOcupada;
  
    void Awake()
    {
        // Configura el patrón Singleton para acceso global
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Al empezar, nos aseguramos de que el texto de Game Over esté oculto
         Time.timeScale = 1f; // Nos aseguramos de que el juego no empiece pausado
        if (textoGameOver != null) textoGameOver.gameObject.SetActive(false);
        if (textoGameWin != null) textoGameWin.gameObject.SetActive(false);
        ActualizarInterfaz();
    }

    void Update()
    {
        gestionarYGenerarPowerUps();
        // Si perdimos y el jugador presiona R, reiniciamos la partida
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        eliminarPowerUpPasadoDeTiempo();
        terminarJuegoSiMurieronLosEnemigos();
    }
    public void terminarJuegoSiMurieronLosEnemigos()
    {
        if(enemigosVivos == 0)
        {
            ActivarGameOver("ganaste"); 
        }
    }
    public void eliminarPowerUpPasadoDeTiempo()
    {
        if(listaDeTiempos.Count>0)
        {
            eliminarPowerUpActivo();
        }
    }
    public void eliminarPowerUpActivo()
    {
        posicionOcupada = ordenDePosicionesOcupadas[0];
        if(Time.time >= listaDeTiempos[0])
        {
            cantidadDePowerUpsActivos--;
            listaDeTiempos.RemoveAt(0);
            posicionesOcupadas.Remove(posicionOcupada);
            ordenDePosicionesOcupadas.RemoveAt(0);
        }
    }
    public void agregarTiempo(float tiempo)
    {
        listaDeTiempos.Add(tiempo);
    }

    // Función pública para que otros scripts sumen puntos (ej. al destruir un enemigo)
    public void SumarPuntos(int puntosAMentat)
    {
        if (juegoTerminado)return;  
        puntuacion += puntosAMentat;
        ActualizarInterfaz();
    }
    public void restarPuntos(int puntosAMentat)
    {
        if (juegoTerminado) return;
        puntuacion -= puntosAMentat;
        if(puntuacion<0 )
        {
            puntuacion = 0;
        }
        ActualizarInterfaz();
    }
     // Función para restar vidas cuando el jugador es golpeado 
    public void RestarVida() 
    { 
        if (juegoTerminado) return; 
 
        vidas--; // Resta 1 a las vidas actuales 
        ActualizarInterfaz(); 
 
        if (vidas <= 0) 
        { 
            ActivarGameOver("perdiste"); 
        } 
    }

    void ActualizarInterfaz()
    {
         if (textoPuntuacion != null) textoPuntuacion.text = "Puntos: " + puntuacion; 
        // NUEVO: Actualiza el texto de vidas en la pantalla 
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
    }

    // Función pública para activar el fin del juego (ej. si la nave muere o los enemigos bajan demasiado)
    public void ActivarGameOver(string condicion) 
    { 
        juegoTerminado = true; 
        Time.timeScale = 0f;  //pausar el juego
        if (condicion == "perdiste") 
        { 
            textoGameOver.gameObject.SetActive(true);  
        }
        else
        {
            textoGameWin.gameObject.SetActive(true);
        } 
    } 
    public void sumarPuntosExtras()
    {
        if(juegoTerminado)
        {
            puntuacion += 5;
        }
    }
    public void generarPowerUp()
    {
        Debug.Log("SPAWN ejecutado en: " + Time.time);
        numeroRandom = Random.Range(1,4);
        posicionRandom = Random.Range(1,4);
        Debug.Log("el numero que salio fue "+ numeroRandom+ " en la posicion " + posicionRandom);
        Vector2 lc=  localizacionRandom(posicionRandom);
        if(validarPosicion(posicionRandom))
        {
            switch (numeroRandom)
            {
                case 1:
                    Instantiate(prefabPowerUpCadencia, lc, Quaternion.identity).GetComponent<PowerUpCadencia>().asignarPosicion(posicionRandom);
                    setDuracion(1f);
                    break;
                case 2:
                    Instantiate(prefabPowerUpLentitud, lc, Quaternion.identity).GetComponent<PowerUpLentitud>().asignarPosicion(posicionRandom);
                    setDuracion(1f);
                    break;
                default:
                    Instantiate(prefabPowerUpLentitud, lc, Quaternion.identity).GetComponent<PowerUpLentitud>().asignarPosicion(posicionRandom);
                    setDuracion(1f);//10
                    break;
            }
            agregarPosicion(posicionRandom);
        }
        else
        {
            Debug.Log("NumeroRepetido cooldown de 2 segundos");
            setDuracion(2f);//8
        }
    }
    public Vector2 localizacionRandom(int numero)
    {
        switch (numero)
        {
            case 1:
                return new Vector2(-8.07f, -4.438f);         
            case 2:
                return new Vector2(8.48f, -4.438f); 
            default:
                return new Vector2(-1f, -4.438f); 
        }
    }
    public void setDuracion(float tiempo)
    {
        duracion = Time.time + tiempo;
    }
    
    public void gestionarYGenerarPowerUps()
    {
        if(cantidadDePowerUpsActivos <= cantidadMaximaDePowerUps && Time.time>= duracion)
        {
            setDuracion(1f);//15
            generarPowerUp();
        }
    }
    public bool validarPosicion(int posicion)
    {
        return !posicionesOcupadas.Contains(posicion);
        
    }
    public void agregarPosicion(int posicion)
    {
        cantidadDePowerUpsActivos++;
        posicionesOcupadas.Add(posicion);
    }
    public void eliminarEnemigo()
    {
        enemigosVivos --;
    }
    public void agregarPosicionOcupada(int posicion)
    {
        ordenDePosicionesOcupadas.Add(posicion);
    }
}