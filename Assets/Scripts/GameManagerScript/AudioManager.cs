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
    public AudioSource fuenteMusica;
    public AudioClip sonidoOpcion;
    public AudioClip sonidoNivelGanado;
    public AudioClip sonidoNivelPerdido;
    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        fuenteAudio = GetComponent<AudioSource>();
    }

    // Funcion para reproducir cualquier sonido una sola vez
    public void ReproducirSonido(AudioClip clip,float volumen,float pitch)
    {
        if (clip != null)
        {
            fuenteAudio.pitch = pitch;
            fuenteAudio.PlayOneShot(clip,volumen);
        }
    }
    // Reproducir musica en bucle
    public void ReproducirMusica(AudioClip musica)
    {
        if (musica == null) return;

        // Si es la misma cancion, no la reiniciamos
        if (fuenteMusica.clip == musica)
            return;

        fuenteMusica.clip = musica;
        fuenteMusica.loop = true;
        fuenteMusica.Play();
    }

    // Pausar musica
    public void PausarMusica()
    {
        fuenteMusica.Pause();
    }

    // Reanudar musica
    public void ReanudarMusica()
    {
        fuenteMusica.UnPause();
    }
}
