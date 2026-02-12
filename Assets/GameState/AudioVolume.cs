
// Library
using UnityEngine;
//Author:Jia
//Description: This script manages the audio volume settings for music and sound effects in the game.
public class AudioVolume : MonoBehaviour
{
    //set the keys for Music and SFX volume
    private const string VolumeMusicKey = "MusicVol";
    private const string VolumeSFXKey = "SFXVol";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Apply saved volume settings on start
        MusicVolOnly();
        SFXVolOnly();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Music Volume Only
    public static void MusicVolOnly()
    {
        float volume = PlayerPrefs.GetFloat(VolumeMusicKey, 1f);

        // Get all Music Audio Sources in scene
        AudioSource[] sources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource src in sources)
        {
            // Only change AudioSources Volume that is tagged with "Music"
            if (src.CompareTag("Music"))
            {
                src.volume = volume;
            }
        }
    }

    // SFX Volume Only
    public static void SFXVolOnly()
    {
        float volume = PlayerPrefs.GetFloat(VolumeSFXKey, 1f);

        // Get all the SFX Audio Sources in the scene
        AudioSource[] sources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource src in sources)
        {
            // Only change AudioSources Volume that is tagged with "SFX"
            if (src.CompareTag("SFX"))
            {
                src.volume = volume;
            }
        }
    }

}
