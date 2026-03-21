using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    
    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        PlayMusic(backgroundMusic);
    }

    public void PlaySound(AudioClip clip)
    {
        sfxAudioSource.clip = clip;
        sfxAudioSource.pitch = Random.Range(0.8f, 1.2f);
        sfxAudioSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}
