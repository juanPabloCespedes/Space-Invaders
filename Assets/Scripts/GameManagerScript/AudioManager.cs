using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;

    private AudioSource fuenteAudio;

    // Coloca aquí tus clips de sonido desde el inspector de Unity
    public AudioClip sonidoDisparoJugador;
    public AudioClip sonidoDisparoEnemigo;
    public AudioClip sonidoExplosion;
    public AudioClip powerUpcadencia;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        fuenteAudio = GetComponent<AudioSource>();
    }

    // Función para reproducir cualquier sonido una sola vez
    public void ReproducirSonido(AudioClip clip)
    {
        if (clip != null)
        {
            fuenteAudio.PlayOneShot(clip);
        }
    }
}
