using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoGestor : MonoBehaviour
{
    public int puntos;
    private bool vivo = true;
    public int getPuntos()
    {
        return puntos;
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
