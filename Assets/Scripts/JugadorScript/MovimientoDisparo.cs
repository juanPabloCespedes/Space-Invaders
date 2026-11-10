using UnityEngine;

public class MovimientoDisparo : MonoBehaviour
{
    // Velocidad a la que sube el láser
    public float velocidadDisparo = 15f; 

    void Update()
    {
        // Mueve el disparo constantemente hacia arriba
        transform.Translate(Vector2.up * velocidadDisparo * Time.deltaTime);

        // Si el disparo sube demasiado y sale de la pantalla, se destruye para no gastar memoria
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }

    // Se ejecuta automáticamente cuando el disparo toca a otro objeto con Collider 2D
    private void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si tocamos a un enemigo...
        if (otroObjeto.CompareTag("Enemigo"))
        {
            GameManager.Instancia.SumarPuntos(10); //Suma 10 puntos
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion); // Sonido de explosión
            Destroy(otroObjeto.gameObject); // Destruye al enemigo
            Destroy(gameObject);            // Destruye el disparo
        }
    }
}
