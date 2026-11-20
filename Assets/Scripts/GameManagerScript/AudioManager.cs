using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;

    private AudioSource fuenteAudio;

    // Coloca aquí tus clips de sonido desde el inspector de Unity
    public AudioClip sonidoDisparoJugador;
    public AudioClip sonidoDisparoEnemigo;
    public AudioClip sonidoDisparoTitan;
    public AudioClip sonidoExplosion;
    public AudioClip sonidoHit;
    public AudioClip sonidoHitDesviado;
    public AudioClip powerUpcadencia;
    public AudioClip powerUpRebote;
    public AudioClip bunkerRoto;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        fuenteAudio = GetComponent<AudioSource>();
    }

    // Función para reproducir cualquier sonido una sola vez
    public void ReproducirSonido(AudioClip clip,float volumen,float pitch)
    {
        if (clip != null)
        {
            fuenteAudio.pitch = pitch;
            fuenteAudio.PlayOneShot(clip,volumen);
        }
    }
}
