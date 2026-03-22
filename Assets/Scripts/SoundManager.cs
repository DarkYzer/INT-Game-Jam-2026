using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource[] sfxAudioSources;
    private int _lastPlayedSfxAudioSource;
    
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
        _lastPlayedSfxAudioSource ++;
        if(_lastPlayedSfxAudioSource >= sfxAudioSources.Length) _lastPlayedSfxAudioSource = 0;
        sfxAudioSources[_lastPlayedSfxAudioSource].clip = clip;
        sfxAudioSources[_lastPlayedSfxAudioSource].pitch = Random.Range(0.8f, 1.2f);
        sfxAudioSources[_lastPlayedSfxAudioSource].Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}
