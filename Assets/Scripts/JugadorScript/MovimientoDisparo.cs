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
        if (transform.position.y > 10f)
        {
            Destroy(gameObject);
        }
    } 
   //Funcion donde gestionamos como afecta al enemigo el disparo
    private void OnTriggerEnter2D(Collider2D otroObjeto)
    {
        // Si tocamos a un enemigo...
        if (otroObjeto.CompareTag("Enemigo"))
        {
            Rigidbody2D rb = otroObjeto.gameObject.GetComponent<Rigidbody2D>();//obtenemos el rigid body para cambiar su gravedad
            BoxCollider2D bc= otroObjeto.gameObject.GetComponent<BoxCollider2D>();// obtenemos el box collider para evitar colisiones innesesarias
            EnemigoGestor eg = otroObjeto.gameObject.GetComponent<EnemigoGestor>();// obtenemos el enemigo gestor, script esencial para el manejo individual de los enemigos 
            AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion,1f,eg.getPitch()); // Sonido de explosión con un pitch distinto dependa del enemigo
            eg.cambiarEstadoDeVida();// usado idealmente para gestionar comportamiento fuera de tiempo
            GameManager.Instancia.SumarPuntos(eg.getPuntos()); //Suma puntos, conforme el enemigo que eliminemos
            rb.gravityScale=-2f;// cambia la gravedad de la nave  
            rb.angularVelocity = Random.Range(-360f, 360f);//Rota la nave 
            bc.enabled = false;
            Destroy(gameObject); // Destruye el disparo
            Destroy(otroObjeto.gameObject,1f); // Destruye al enemigo despues de un tiempo
        }
    }
}
