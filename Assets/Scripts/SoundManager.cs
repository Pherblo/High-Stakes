using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject settings;
    public GameObject backgroundMusic;
    public GameObject soundEffects;

    private float musicVolume;
    private float effectsVolume;
    void Start()
    {
        
    }

    
    void Update()
    {
        setVolume();
    }

    private void setVolume()
    {
        musicVolume = settings.GetComponent<OptionsManager>().musicSlider.value;
        effectsVolume = settings.GetComponent<OptionsManager>().sfxSlider.value;

        if (backgroundMusic.GetComponent<AudioSource>() != null)
        {
            backgroundMusic.GetComponent<AudioSource>().volume = musicVolume;
        }

        if (soundEffects.GetComponent<AudioSource>() != null)
        {
            soundEffects.GetComponent<AudioSource>().volume = effectsVolume;
        }
    }
}
