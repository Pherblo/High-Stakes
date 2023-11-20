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

    private float musicVolume;
    private float effectsVolume;

    public UnityEvent<CardEvent> OnCardPicked;      // When a card has been picked from the deck.
    void Start()
    {
        
    }

    
    void Update()
    {
        SetVolume();
        CardEffect();
    }

    private void SetVolume()
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

    public void CardEffect()
    {
        if(CardSFX.GetComponent<AudioSource>() != null)
        {
            CardSFX.GetComponent<AudioSource>().Play();
        }
    }
}
