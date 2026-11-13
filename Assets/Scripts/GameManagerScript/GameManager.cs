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
    private bool juegoTerminado = false;
    public GameObject prefabPowerUpCadencia;
    public int cantidadDePowerUpsActivos = 0; 
    public int cantidadMaximaDePowerUps = 2; 
    private float duracion = 10f;
    List<int> posicionesOcupadas = new List<int>();
    int numeroRandom;
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
        gestionarYGenerarPowerUps();
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Time.time >= duracion && posicionesOcupadas.Count > 0)
        {
            cantidadDePowerUpsActivos--;
            posicionesOcupadas.RemoveAt(0);
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
    public void generarPowerUp()
    {
        numeroRandom = Random.Range(1,5);
        Vector2 lc=  localizacionRandom(Random.Range(1,5));
        validarPosicion(numeroRandom);
        switch (numeroRandom)
        {
            case 1:
                Instantiate(prefabPowerUpCadencia, lc, Quaternion.identity);
                agregarPosicion(1);
                setDuracion(10f);
                break;
            case 2:
                Instantiate(prefabPowerUpCadencia, lc, Quaternion.identity);
                agregarPosicion(2);
                setDuracion(10f);
                break;
            case 3:
                Instantiate(prefabPowerUpCadencia, lc, Quaternion.identity);
                agregarPosicion(3);
                setDuracion(10f);
                break;
            default:
                Debug.Log("no hay nada que tirar cooldown de 3 segundos");
                setDuracion(8f);
                break;
        }
    }
    public Vector2 localizacionRandom(int numero)
    {
        switch (numero)
        {
            case 1:
                return new Vector2(-8.07f, -3.964f);         
            case 2:
                return new Vector2(3.87f, -3.964f); 
            default:
                return new Vector2(-1f, -3.964f); 
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
            generarPowerUp();
            setDuracion(15f);
        }
    }
    public void validarPosicion(int posicion)
    {
        if (!posicionesOcupadas.Contains(posicion))
        {
            numeroRandom = posicion;
        }
        else
        {
            agregarFaltante(posicion);
        }
        
    }
    public void agregarPosicion(int posicion)
    {
        cantidadDePowerUpsActivos++;
        posicionesOcupadas.Add(posicion);
    }
    public void agregarFaltante(int posicion)
    {
        if (!posicionesOcupadas.Contains(3))
        {
            numeroRandom = 3;
        }
        if (!posicionesOcupadas.Contains(2))
        {
            numeroRandom = 2;
        }
        if (!posicionesOcupadas.Contains(1))
        {
            numeroRandom = 1;
        }
        else
        {
            numeroRandom = posicion;
        }
    }
}