using UnityEngine;
using UnityEngine.UI;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip menuMusic;        // Baggrundsmusik
    [SerializeField] private AudioClip buttonClickSound; // Knap klik lyd
    [SerializeField] private AudioClip buttonHoverSound; // Knap hover lyd

    [Header("Volume Settings")]
    [SerializeField] private float musicVolume = 0.5f;
    [SerializeField] private float sfxVolume = 1f;
    [SerializeField] private float pausedMusicVolume = 0.25f;

    private void Awake()
    {
        // Sikrer at AudioManager overlever scene skift
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        if (musicSource != null)
        {
            musicSource.clip = menuMusic;
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

    // Kald denne når en knap klikkes
    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    // Kald denne når musen hover over en knap
    public void PlayButtonHover()
    {
        if (sfxSource != null && buttonHoverSound != null)
        {
            sfxSource.PlayOneShot(buttonHoverSound);
        }
    }

    // Kald denne når spillet pauses
    public void OnGamePaused()
    {
        if (musicSource != null)
        {
            musicSource.volume = pausedMusicVolume;
        }
    }

    // Kald denne når spillet genoptages
    public void OnGameResumed()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
}