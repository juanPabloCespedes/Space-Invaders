using UnityEngine; 
public class EscudoDestruible : MonoBehaviour 
{ 
    // Resistencia del escudo (cuántos disparos aguanta antes de romperse por completo) 
    public int saludMaxima = 4; 
    private int saludActual; 
    // Referencia al componente SpriteRenderer para cambiar el color del escudo según su daño 
    [SerializeField] private Sprite[] spritesDanio;// manejo de lista de sprites donde se cargan del sprite mas nuevo al mas roto
    private SpriteRenderer renderizadorSprite; //manejar el sprite
 
    void Start() 
    { 
        saludActual = saludMaxima;
        renderizadorSprite = GetComponent<SpriteRenderer>(); 
    }  
    // También detectamos si los enemigos FÍSICAMENTE pisan el escudo al bajar 
    private void OnTriggerEnter2D(Collider2D otroObjeto) 
    { 
        if (otroObjeto.CompareTag("Enemigo"))
        {
            RecibirDanio(saludActual);
            return;
        }
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
 
        // Si la salud llega a cero, el búnker se destruye por completo 
        if (saludActual <= 0) 
        { 
            if (AudioManager.Instancia != null) 
            { 
                AudioManager.Instancia.ReproducirSonido(AudioManager.Instancia.bunkerRoto,0.5f,1f); 
            } 
            Destroy(gameObject);
            return; 
        }
        int indiceSprite = saludMaxima - saludActual-1;
        indiceSprite = Mathf.Clamp(indiceSprite, 0, spritesDanio.Length - 1);
        renderizadorSprite.sprite = spritesDanio[indiceSprite]; 
    } 
}