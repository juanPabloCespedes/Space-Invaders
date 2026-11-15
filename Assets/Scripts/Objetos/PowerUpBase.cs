using UnityEngine; 
public abstract class PowerUpBase : MonoBehaviour
{
    protected float duracion;
    protected virtual void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.powerUpcadencia);
    }
    protected virtual void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si el jugador colisiona con el power up
        if (otroObjeto.CompareTag("Jugador"))
        {
            ControladorJugador jg = otroObjeto.gameObject.GetComponent<ControladorJugador>();
            activarPowerUp(jg);
            GameManager.Instancia.agregarTiempo(duracion);
            reproducirAudio(); // Sonido de Power Up  
            Destroy(gameObject);
        }
    }
    protected virtual void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        jg.activarPowerUp("cadencia",duracion);
    }
} 
