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
            Rigidbody2D rb = otroObjeto.gameObject.GetComponent<Rigidbody2D>();
            BoxCollider2D bc= otroObjeto.gameObject.GetComponent<BoxCollider2D>(); 
            GameManager.Instancia.SumarPuntos(10); //Suma 10 puntos
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion); // Sonido de explosión
            rb.gravityScale=-2f;// cambia la gravedad de la nave  
            otroObjeto.transform.Rotate(0f, 0f, 350f);//Rota la nave
            Destroy(gameObject); // Destruye el disparo
            Destroy(otroObjeto.gameObject,0.8f); // Destruye el disparo

        }
    }
}
