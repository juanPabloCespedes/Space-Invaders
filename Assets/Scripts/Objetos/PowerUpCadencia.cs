using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCadencia : MonoBehaviour
{
    private float duracion;

    private void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si el jugador colisiona con el power up
        if (otroObjeto.CompareTag("Jugador"))
        {
            ControladorJugador jg = otroObjeto.gameObject.GetComponent<ControladorJugador>();
            activarPowerUp(jg);
            Destroy(gameObject);
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.powerUpcadencia); // Sonido de Power Up  
        }
    }
    private void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        jg.activarPowerUp("cadencia",duracion);
    }
}
