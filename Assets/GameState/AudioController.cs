
// Libraries
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    //set the keys for Music and SFX volume
    private const string VolumeMusicKey = "MusicVol";
    private const string VolumeSFXKey = "SFXVol";

    // References to UI Sliders
    public Slider musicSlider;
    public Slider sfxSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize sliders with saved values
        float savedVolume = PlayerPrefs.GetFloat(VolumeMusicKey, 1f);
        musicSlider.value = savedVolume;
        musicSlider.onValueChanged.AddListener(MusicVolumeChange);

        // SFX Slider Initialization
        float savedSFXVolume = PlayerPrefs.GetFloat(VolumeSFXKey, 1f);
        sfxSlider.value = savedSFXVolume;
        sfxSlider.onValueChanged.AddListener(SFXVolumeChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MusicVolumeChange(float value)
    {
        // Apply immediately to music AudioSources
        PlayerPrefs.SetFloat(VolumeMusicKey, value);
        PlayerPrefs.Save();
        AudioVolume.MusicVolOnly();
    }

    void SFXVolumeChange(float value)
    {
        // Apply immediately to SFX AudioSources
        PlayerPrefs.SetFloat(VolumeSFXKey, value);
        PlayerPrefs.Save();
        AudioVolume.SFXVolOnly();
    }
}
