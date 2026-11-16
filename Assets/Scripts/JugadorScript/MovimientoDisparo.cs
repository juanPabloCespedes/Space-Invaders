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
            EnemigoGestor eg = otroObjeto.gameObject.GetComponent<EnemigoGestor>();
            eg.cambiarEstadoDeVida();// usado para gestionar disparos innecesarios
            //ControladorEnemigos.Instancia.restarEnemigos(); //actualiza la variable de enemigos vivos
            GameManager.Instancia.SumarPuntos(eg.getPuntos()); //Suma puntos
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion); // Sonido de explosión
            rb.gravityScale=-2f;// cambia la gravedad de la nave  
            rb.angularVelocity = Random.Range(-360f, 360f);//Rota la nave 
            Destroy(gameObject); // Destruye el disparo
            Destroy(otroObjeto.gameObject,0.8f); // Destruye al enemigo despues de un tiempo
        }
    }
}
