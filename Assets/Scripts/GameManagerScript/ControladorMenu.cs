using UnityEngine; 
using UnityEngine.SceneManagement; // Requerido para cambiar de escena 
public class ControladorMenu : MonoBehaviour 
{ 
    public AudioClip musicaNivel;
    void Start() 
        { 
            // Asegura que el tiempo corra normalmente por si venimos de un reinicio pausado 
            Time.timeScale = 1f;
            AudioManager.Instancia.ReproducirMusica(musicaNivel);  
        }  
        // Creamos esta función pública para que el Botón del mouse pueda llamarla
        public void CargarEscenaJuego(string escena)
        {
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoOpcion,1f,1f); 
            SceneManager.LoadScene(escena); 
        }
        public void salirDelJuego()
        {
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoOpcion,1f,1f);
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