using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRebote : PowerUpBase
{
    protected override void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.powerUpRebote,1f,1f);
    }
    protected override void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 6f;
        jg.activarPowerUp("rebote",duracion);
    }
}
