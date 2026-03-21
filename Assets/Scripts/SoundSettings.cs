using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{

    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
/*
    public void SetMusicVolume() {
        float coeff = musicSlider.value;
        myMixer.setFloat("MusicParameter", coeff);

    }
*/



}
