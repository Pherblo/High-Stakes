using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public GameObject settings;
    public GameObject backgroundMusic;
    public GameObject CardSFX;
    public GameObject soundEffects;
    public GameObject shatterSFX;
    public GameObject overflowSFX;

    private float musicVolume;
    private float effectsVolume;

    void Start()
    {
        
    }

    
    void Update()
    {
        SetVolume();

        
    }

    private void SetVolume()
    {
        musicVolume = settings.GetComponent<OptionsManager>().musicSlider.value;
        effectsVolume = settings.GetComponent<OptionsManager>().sfxSlider.value;

        if (backgroundMusic.GetComponent<AudioSource>() != null)
        {
            backgroundMusic.GetComponent<AudioSource>().volume = musicVolume;
        }

        if (soundEffects.GetComponentInChildren<AudioSource>() != null)
        {
            foreach (AudioSource source in soundEffects.GetComponentsInChildren<AudioSource>())
            {
                source.volume = effectsVolume;
            }
           
        }
    }

    //to be called by OnCardPicked event 
    public void CardEffect()
    {
        if(CardSFX.GetComponent<AudioSource>() != null)
        {
            CardSFX.GetComponent<AudioSource>().Play();
        }
    }

    //to be called by OnShatter event
    public void ShatterEffect()
    {
        if (shatterSFX.GetComponent<AudioSource>() != null)
        {
            shatterSFX.GetComponent<AudioSource>().Play();
        }
    }

    public void OverflowEffect()
    {
        if (overflowSFX.GetComponent<AudioSource>() != null)
        {
            overflowSFX.GetComponent<AudioSource>().Play();
        }
    }
}
