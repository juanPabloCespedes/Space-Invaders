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
        } 
        // Creamos esta función pública para que el Botón del mouse pueda llamarla
        public void CargarEscenaJuego(string escena)
        {
            SceneManager.LoadScene(escena); 
        }
        public void salirDelJuego()
        {
            Debug.Log("Saliendo del juego...");
            // Cierra la aplicación (Funciona en el juego exportado para PC/Android/etc.)
            Application.Quit();

            // CONDICIÓN ESPECIAL PARA EL EDITOR DE UNITY:
            // Como Application.Quit() no cierra el editor mientras estás probando, 
            // le decimos a Unity que detenga el modo Play para que notes que funciona.
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }