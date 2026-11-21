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
    public TMP_Text textoTiempo; 
    public TMP_Text textoResta; 
    public TMP_Text textoPausa; 
    private bool juegoTerminado = false;
    public GameObject prefabPowerUpCadencia;
    public GameObject prefabPowerUpLentitud; 
    public GameObject prefabPowerUpRebote; 
    public GameObject prefabTitan;
    public int cantidadDePowerUpsActivos = 0; // power ups activos
    public int cantidadMaximaDePowerUps; // maximo de power ups
    private float duracion = 5f;//cooldwon de powerup
    List<int> posicionesOcupadas = new List<int>();//lista para manejar spawn de power ups
    List<float> listaDeTiempos = new List<float>();//lista para manejar los tiempos de los power ups
    List<int> ordenDePosicionesOcupadas = new List<int>();//lista para manejar el orden de uso de posiciones
    private int numeroRandom;// numero random para manejar tipos de power up 
    private int posicionRandom;// numero random para manejar diferentes posiciones de power up 
    private int posicionOcupada;//numero para obtener la posicion ocupada
    private float tiempoInicio;//manejo de reloj 
    private int ultimoSegundo = -1;
    private float advertenciaDeResta;//advertencia de resta de puntos
    public int probabilidadDeSpawnTitan;
    public float respawnDeTitan;
    private float tiempoDeReaparicionDeTitan;
    private int probabilidadRandom;

    void Awake()
    {
        // Configura el patrón Singleton para acceso global
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
        tiempoInicio = Time.time;
        ActualizarTiempo();
    }

    void Start()
    {
        // Al empezar, nos aseguramos de que el texto de Game Over esté oculto y los demas tambien
         Time.timeScale = 1f; // Nos aseguramos de que el juego no empiece pausado
        if (textoGameOver != null) textoGameOver.gameObject.SetActive(false);
        if (textoGameWin != null) textoGameWin.gameObject.SetActive(false);
        if (textoResta != null) textoResta.gameObject.SetActive(false);
        if (textoResta != null) textoPausa.gameObject.SetActive(false);
        
        ActualizarInterfaz();
        tiempoDeReaparicionDeTitan = respawnDeTitan;
    }

    void Update()
    {
        gestionarYGenerarTitan();
        gestionarYGenerarPowerUps();//funcion corazon que maneja los spawns de los powerUps
        // Si perdimos y el jugador presiona R, reiniciamos la partida
        if (!juegoTerminado && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;  
            textoPausa.gameObject.SetActive(true);
        }
        eliminarPowerUpPasadoDeTiempo();//gestiona los power ups que pasaron de tiempo
        terminarJuegoSiMurieronLosEnemigos();
        ActualizarTiempo();
        eliminarAdvertencia();
    }
    //funcion que oculta la advertencia de resta de puntos
    private void eliminarAdvertencia()
    {
        if(Time.time >= advertenciaDeResta )
        {
            textoResta.gameObject.SetActive(false);
        }
    }
    //funcion para manejar el relog
     private void ActualizarTiempo()
        {
            int tiempoTotal = Mathf.FloorToInt(Time.time - tiempoInicio);
            if (tiempoTotal == ultimoSegundo) return;
            ultimoSegundo = tiempoTotal;
            int minutos = tiempoTotal / 60;
            int segundos = tiempoTotal % 60;
            textoTiempo.text = $"{minutos:00}:{segundos:00}";
        }
    //funcion para gestionar el endgame
    public void terminarJuegoSiMurieronLosEnemigos()
    {
        if(todosLosEnemigosMuertos())
        {
            SumarPuntos(bonusPorTiempo());
            ActivarGameOver("ganaste"); 
        }
    }
    //funcion que accede a la lista y elimina el powerUp activo
    public void eliminarPowerUpPasadoDeTiempo()
    {
        if(listaDeTiempos.Count>0)
        {
            eliminarPowerUpActivo();
        }
    }
    //funcion que se encarga de Eliminar el powerup activo  
    public void eliminarPowerUpActivo()
    {
        posicionOcupada = ordenDePosicionesOcupadas[0];//esta lista maneja los power ups que usa el jugador y de esta forma se ordenan cual agarro primero y cual ultimo
        if(Time.time >= listaDeTiempos[0])
        {
            cantidadDePowerUpsActivos--;
            listaDeTiempos.RemoveAt(0);// eliminamos de la lista de tiempos el tiempo indice 0, ya que seria el primero que agarro el jugador 
            posicionesOcupadas.Remove(posicionOcupada);// eliminamos la posicion ocupada que obtuvimos gracias a la posicion de spawn del power up
            ordenDePosicionesOcupadas.RemoveAt(0);// eliminamos del orden de los cuales agarro el jugador, el del indice 0 tambien
        }
    }
    //funcion para agregar tiempo
    public void agregarTiempo(float tiempo)
    {
        listaDeTiempos.Add(tiempo);
    }
    //funcion para sacar posicion ocupada 
    public void desocuparPosicion(int posicion)
    {
        cantidadDePowerUpsActivos--;
        posicionesOcupadas.Remove(posicion);
    }
    // Función pública para que otros scripts sumen puntos (ej. al destruir un enemigo)
    public void SumarPuntos(int puntosAMentat)
    {
        if (juegoTerminado)return;  
        puntuacion += puntosAMentat;
        ActualizarInterfaz();
    }
    //funcion que sirve para restar puntos y mostrar la advertencia  
    public void restarPuntos(int puntosAMentat)
    {
        if (juegoTerminado) return;
        puntuacion -= puntosAMentat;
        if(puntuacion<0 )
        {
            puntuacion = 0;
        }
        advertenciaDeResta = Time.time + 2f;
        textoResta.gameObject.SetActive(true);
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
        textoPuntuacion.text = "" + puntuacion; 
        textoVidas.text = "" + vidas;
    }

    // Función pública para activar el fin del juego (ej. si la nave muere o los enemigos bajan demasiado o el jugador gano eliminando a todos)
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
    //funcion que elige de manera aleatoria, el power up y el lugar 
    public void generarPowerUp()
    {
        numeroRandom = Random.Range(1,4);
        posicionRandom = Random.Range(1,4);
        Vector2 lc=  localizacionRandom(posicionRandom);//funcion que retorna un vector para instanciarlo en el prefab
        if(validarPosicion(posicionRandom))//funcion de validacion si no esta ocupada la posicion 
        {
            switch (numeroRandom)
            {
                case 1:
                    Instantiate(prefabPowerUpCadencia, lc, Quaternion.identity).GetComponent<PowerUpCadencia>().asignarPosicion(posicionRandom);
                    setDuracion(1f);// cooldown de power ups en general
                    break;
                case 2:
                    Instantiate(prefabPowerUpLentitud, lc, Quaternion.identity).GetComponent<PowerUpLentitud>().asignarPosicion(posicionRandom);
                    setDuracion(1f);
                    break;
                default:
                    Instantiate(prefabPowerUpRebote, lc, Quaternion.identity).GetComponent<PowerUpRebote>().asignarPosicion(posicionRandom);
                    setDuracion(1f);//10
                    break;
            }
            agregarPosicion(posicionRandom);//se agrega la posicion en la que se tomo este power up 
        }
        else
        {
            Debug.Log("NumeroRepetido cooldown de 2 segundos");
            setDuracion(2f);//8
        }
    }
    //funcion que mediante un numero entero obtenemos un vector con coordenadas
    public Vector2 localizacionRandom(int numero)
    {
        switch (numero)
        {
            case 1:
                return new Vector2(-8.998f, -4.438f);         
            case 2:
                return new Vector2(9.005f, -4.438f); 
            default:
                return new Vector2(0f, -4.438f); 
        }
    }
    //void para setear duracion 
    public void setDuracion(float tiempo)
    {
        duracion = Time.time + tiempo;
    }
    // funcion que administra los power ups, si la cantidad de power ups activos es menor que la maxima y el tiempo de cooldown paso, se genera un nuevo power up 
    public void gestionarYGenerarPowerUps()
    {
        if(cantidadDePowerUpsActivos < cantidadMaximaDePowerUps && Time.time>= duracion)
        {
            setDuracion(1f);//15
            generarPowerUp();//genera el power up 
        }
    }
    //funcion que valida la posicion por parametro
    public bool validarPosicion(int posicion)
    {
        return !posicionesOcupadas.Contains(posicion);// consultamos si contiene dicha posicion
        
    }
    //agrega la posicion
    public void agregarPosicion(int posicion)
    {
        cantidadDePowerUpsActivos++;
        posicionesOcupadas.Add(posicion);
    }
    //agrega la posicion en lista ocupada
    public void agregarPosicionOcupada(int posicion)
    {
        ordenDePosicionesOcupadas.Add(posicion);
    }
    //funcion que recompensa al jugador si termino el nivel en x tiempo recibe x cantidad extra de puntos 
    public int bonusPorTiempo()
    {
        if(Time.time <= 60f)//60
        {
            return 500;
        }
        else if (Time.time <= 90f )//90
        {
            return 250;
        }
        else if (Time.time <= 120f)//120
        {
            return 125;
        }
        else
        {
            return 0;
        }
    }
    //funcion util para activar el gameOver "ganaste"
    public bool todosLosEnemigosMuertos()
    {        
        return ControladorEnemigos.Instancia.cantidadDeEnemigos();// instanciamos el controaldor de enemigos y le consultamos si esa funcion booleana es verdadera
    }
    public void reintentar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void siguienteNivel(string nivel)
    {
        SceneManager.LoadScene(nivel);
    }
    public void gestionarYGenerarTitan()
    {
        probabilidadRandom = Random.Range(1,101);
        if(Time.time >= tiempoDeReaparicionDeTitan && probabilidadRandom <= probabilidadDeSpawnTitan)
        {
            Instantiate(prefabTitan, new Vector2(14.22f, 5.397f), Quaternion.identity);
            tiempoDeReaparicionDeTitan = Time.time + respawnDeTitan;
        }
    }
    public void continuarNivel()
    {
        textoPausa.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}