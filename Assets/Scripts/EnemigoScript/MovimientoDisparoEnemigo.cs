using UnityEngine;

public class MovimientoDisparoEnemigo : MonoBehaviour
{
    // Velocidad a la que sube el láser
    private float velocidadDisparo; 
    private int restaDePuntos;
    void Update()
    {
        // Mueve el disparo constantemente hacia abajo
        transform.Translate(Vector2.down * velocidadDisparo * Time.deltaTime);

        // Si el disparo sube demasiado y sale de la pantalla, se destruye para no gastar memoria
        if (transform.position.y < -7.5f)
        {
            Destroy(gameObject);
        }
    }
    public void setVelocidadDeDisparo(float valor)
    {
        velocidadDisparo = valor;
    }
    public void setRestaDePuntos(int valor)
    {
        restaDePuntos = valor *2;
    }
    public int getRestaDePuntos()
    {
        return restaDePuntos;
    }
}
