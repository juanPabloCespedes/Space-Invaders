using UnityEngine; 
using UnityEngine.SceneManagement; // Requerido para cambiar de escena 
public class ControladorMenu : MonoBehaviour 
{ 
    void Start() 
        { 
            // Asegura que el tiempo corra normalmente por si venimos de un reinicio pausado 
            Time.timeScale = 1f;  
        } 
    
        void Update() 
        { 
            // Detecta si el jugador presiona la tecla Enter (Return) o el Enter del teclado numérico 
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) 
            { 
                // Carga la escena del juego. Reemplaza "EscenaJuego" por el nombre exacto de tu escena principal. 
                SceneManager.LoadScene("Juego");  
            } 
        } 
    }