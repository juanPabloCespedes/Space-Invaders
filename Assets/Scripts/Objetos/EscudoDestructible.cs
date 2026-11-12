using UnityEngine; 
public class EscudoDestruible : MonoBehaviour 
{ 
    // Resistencia del escudo (cuántos disparos aguanta antes de romperse por completo) 
    public int saludMaxima = 4; 
    private int saludActual; 
    // Referencia al componente SpriteRenderer para cambiar el color del escudo según su daño 
    private SpriteRenderer renderizadorSprite; 
 
    void Start() 
    { 
        saludActual = saludMaxima; 
        renderizadorSprite = GetComponent<SpriteRenderer>(); 
    } 
    
     private void OnCollisionEnter2D(Collision2D colision) 
    { 
        // Comprobamos si lo que chocó contra el escudo fue un disparo del jugador o del enemigo 
        if (colision.gameObject.CompareTag("Enemigo")) 
        { 
            // 1. Destruimos el proyectil que impactó el escudo 
            Destroy(colision.gameObject); 
 
            // 2. Restamos salud al escudo 
            RecibirDanio(4); 
        } 
    }
    // También detectamos si los enemigos FÍSICAMENTE pisan el escudo al bajar 
    private void OnTriggerEnter2D(Collider2D otroObjeto) 
    { 
        if (otroObjeto.CompareTag("DisparoEnemigo")||otroObjeto.CompareTag("Disparo") ) 
        { 
            // Si un marciano dispara a el búnker, lo rompe un poco 
            Destroy(otroObjeto.gameObject); 
            RecibirDanio(1);
        } 
    } 
 
    void RecibirDanio(int cantidad) 
    { 
        saludActual -= cantidad; 
 
        // Efecto visual básico: El escudo se vuelve más transparente o cambia de color a medida que se rompe 
        if (renderizadorSprite != null) 
        { 
            // Calculamos un porcentaje de la vida restante 
            float porcentajeSalud = (float)saludActual / saludMaxima; 
              
            // Cambiamos el color hacia tonos más rojos/oscuros o reducimos la opacidad (Alpha) 
            renderizadorSprite.color = new Color(1f, porcentajeSalud, porcentajeSalud, 1f); 
        } 
 
        // Si la salud llega a cero, el búnker se destruye por completo 
        if (saludActual <= 0) 
        { 
            if (AudioManager.Instancia != null) 
            { 
                AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.sonidoExplosion); 
            } 
            Destroy(gameObject); 
        } 
    } 
}