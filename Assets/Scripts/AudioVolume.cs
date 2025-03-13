using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public AudioSource musicSource;  // Reference til AudioSource komponenten

    void Start()
    {
        // Justerer den globale volumen til 50% for AudioListener
        AudioListener.volume = 0.5f;

        // Sørg for, at musikken looper
        musicSource.loop = true;  // Sætter AudioSource til at loope

        // Start musikken
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }
}
