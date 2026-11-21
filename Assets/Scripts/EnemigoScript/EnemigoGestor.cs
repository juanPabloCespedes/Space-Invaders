using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoGestor : MonoBehaviour
{
    //clase para gestionar los puntos sonidos y estado de vida
    public int puntos;// puntos que otorga el enemigo
    public float pitchSonido;// pitch variado para el audio
    private bool vivo = true;//gestion de vida para evitar errores fuera de tiempo
    public int getPuntos()
    {
        return puntos;
    }
    public float getPitch()
    {
        return pitchSonido;
    }
    public bool getEstaVivo()
    {
        return vivo;
    }
    public void cambiarEstadoDeVida()
    {
        vivo = false;
    }
    void Update()
    {
        
    }
}
