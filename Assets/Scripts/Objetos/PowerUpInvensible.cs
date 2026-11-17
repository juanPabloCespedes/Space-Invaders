using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvensible : PowerUpBase
{
    protected override void reproducirAudio()
    {
        AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion);
    }
    protected override void activarPowerUp(ControladorJugador jg)
    {
        duracion = Time.time + 4f;
        jg.activarPowerUp("invensible",duracion);
    }
}
