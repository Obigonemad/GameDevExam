using UnityEngine;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip gameMusic;        // Spilmusik
    [SerializeField] private AudioClip buttonClickSound; // Knap klik lyd
    [SerializeField] private AudioClip buttonHoverSound; // Knap hover lyd
    [SerializeField] private AudioClip pauseSound;       // Lyd når spillet pauses
    [SerializeField] private AudioClip unpauseSound;     // Lyd når spillet genoptages

    [Header("Volume Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float pausedMusicVolume = 0.25f;

    private void Awake()
    {
        // Singleton pattern for denne scene
        if (Instance == null)
        {
            Instance = this;
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupAudioSources()
    {
        // Opsæt musik source
        if (musicSource != null && gameMusic != null)
        {
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.playOnAwake = true;
            musicSource.Play();
        }

        // Opsæt SFX source
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
            sfxSource.playOnAwake = false;
        }
    }

    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    public void PlayButtonHover()
    {
        if (sfxSource != null && buttonHoverSound != null)
        {
            sfxSource.PlayOneShot(buttonHoverSound);
        }
    }

    public void OnGamePaused()
    {
        if (musicSource != null)
        {
            musicSource.volume = pausedMusicVolume;
        }

        if (sfxSource != null && pauseSound != null)
        {
            sfxSource.PlayOneShot(pauseSound);
        }
    }

    public void OnGameResumed()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null && unpauseSound != null)
        {
            sfxSource.PlayOneShot(unpauseSound);
        }
    }
}