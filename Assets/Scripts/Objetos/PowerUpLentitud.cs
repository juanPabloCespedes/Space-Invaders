using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLentitud : PowerUpBase
{
    //funcion reescrita para cambiar el sonido del power up
    protected override void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion,1f,1f);
    }
    //funcion para activar el power up distinto
    protected override void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        ControladorEnemigos ce = FindFirstObjectByType<ControladorEnemigos>();
        ce.activarLentitud(duracion);
    }
}
