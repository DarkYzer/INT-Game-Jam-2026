using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SloundSliders : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        audioMixer.GetFloat("musicVolume", out var value);
        musicSlider.value = Mathf.Exp(value / 20f);
        audioMixer.GetFloat("sfxVolume", out value);
        sfxSlider.value = Mathf.Exp(value / 20f);
    }

    public void ChangeMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVolume", value);
    }

    public void ChangeSfxVolume(float value)
    {
        audioMixer.SetFloat("sfxVolume", value);
    }
}
