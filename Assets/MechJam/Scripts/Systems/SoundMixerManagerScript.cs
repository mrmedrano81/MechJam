using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundMixerManagerScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider vfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("_masterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMasterVolume();
            SetSoundFXVolume();
            SetMusicVolume();
        }
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("_masterVolume", volume);
    }

    public void SetSoundFXVolume()
    {

        float volume = vfxSlider.value;
        audioMixer.SetFloat("VFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("_vfxVolume", volume);
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("_musicVolume", volume);

    }

    void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("_masterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("_musicVolume");
        vfxSlider.value = PlayerPrefs.GetFloat("_vfxVolume");


        SetMasterVolume();
        SetMusicVolume();
        SetSoundFXVolume();
    }

}
