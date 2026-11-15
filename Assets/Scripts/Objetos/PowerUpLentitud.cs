using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLentitud : PowerUpBase
{
    protected override void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion);
    }
    protected override void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        ControladorEnemigos ce = FindFirstObjectByType<ControladorEnemigos>();
        ce.activarLentitud(duracion);
    }
}
