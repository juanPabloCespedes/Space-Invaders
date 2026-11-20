using UnityEngine;

public class MovimientoDisparoEnemigo : MonoBehaviour
{
    // Velocidad a la que sube el láser
    private float velocidadDisparo; 

    void Update()
    {
        // Mueve el disparo constantemente hacia abajo
        transform.Translate(Vector2.down * velocidadDisparo * Time.deltaTime);

        // Si el disparo sube demasiado y sale de la pantalla, se destruye para no gastar memoria
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
    public void setVelocidadDeDisparo(float valor)
    {
        velocidadDisparo = valor;
    }
}
