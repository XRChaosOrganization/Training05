using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSettings : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        sliders[0].value = PlayerPrefs.GetFloat("Master_Volume", 1);
        sliders[1].value = PlayerPrefs.GetFloat("BGM_Volume", 1);
        sliders[2].value = PlayerPrefs.GetFloat("SFX_Volume", 1);
    }

    public void SetMasterVolume(float slidervalue)
    {
        mixer.SetFloat("Master_Volume", Mathf.Log10(slidervalue) * 20);
        PlayerPrefs.SetFloat("Master_Volume", slidervalue);
    }

    public void SetMusicVolume(float slidervalue)
    {
        mixer.SetFloat("BGM_Volume", Mathf.Log10(slidervalue) * 20);
        PlayerPrefs.SetFloat("BGM_Volume", slidervalue);
    }

    public void SetSoundVolume(float slidervalue)
    {
        mixer.SetFloat("SFX_Volume", Mathf.Log10(slidervalue) * 20);
        PlayerPrefs.SetFloat("SFX_Volume", slidervalue);
    }


}
