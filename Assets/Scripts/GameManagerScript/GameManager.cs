using UnityEngine;
using TMPro; // Necesario para controlar textos de TextMeshPro
using UnityEngine.SceneManagement; // Necesario para reiniciar escenas

public class GameManager : MonoBehaviour
{
    // Instancia estática para que cualquier script pueda acceder al GameManager fácilmente
    public static GameManager Instancia;
    public int vidas = 3;
    public int puntuacion = 0;
    public TMP_Text textoVidas;  
    public TMP_Text textoPuntuacion;
    public TMP_Text textoGameOver; // Asegúrate de ocultarlo al empezar en Unity

    private bool juegoTerminado = false;

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
        ActualizarInterfaz();
    }

    void Update()
    {
        // Si perdimos y el jugador presiona R, reiniciamos la partida
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Función pública para que otros scripts sumen puntos (ej. al destruir un enemigo)
    public void SumarPuntos(int puntosAMentat)
    {
        if (juegoTerminado) return;
        puntuacion += puntosAMentat;
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
            ActivarGameOver(); 
        } 
    }

    void ActualizarInterfaz()
    {
         if (textoPuntuacion != null) textoPuntuacion.text = "Puntos: " + puntuacion; 
        // NUEVO: Actualiza el texto de vidas en la pantalla 
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
    }

    // Función pública para activar el fin del juego (ej. si la nave muere o los enemigos bajan demasiado)
    public void ActivarGameOver() 
    { 
        juegoTerminado = true; 
        if (textoGameOver != null) 
        { 
            textoGameOver.gameObject.SetActive(true); 
            textoGameOver.text = "¡GAME OVER!\nPresiona 'R' para reiniciar"; 
        } 
        //pausar el juego
        Time.timeScale = 0f;  
    } 
}

