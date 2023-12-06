using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound effect objects: ")]
    public GameObject backgroundMusic;
    public GameObject CardSFX;
    public GameObject soundEffects;
    public GameObject shatterSFX;
    public GameObject overflowSFX;

    [Header("Reference to menu settings: ")]
    public GameObject settings;

    [Header("Volume variables: ")]
    private float musicVolume;
    private float sfxVolume;

    void Start()
    {
    }

    void Update()
    {
        SetVolume();
    }

    //function to set volume based off the menu settings
    private void SetVolume()
    {
        musicVolume = settings.GetComponent<OptionsManager>().musicSlider.value; //set music volume to musicSlider value
        sfxVolume = settings.GetComponent<OptionsManager>().sfxSlider.value; //set sfx volume to sfxSlider value

        if (backgroundMusic.GetComponent<AudioSource>() != null)
        {
            backgroundMusic.GetComponent<AudioSource>().volume = musicVolume; //change the background music volume to the set value by slider
        }

        if (soundEffects.GetComponentInChildren<AudioSource>() != null)
        {
            foreach (AudioSource source in soundEffects.GetComponentsInChildren<AudioSource>())  //for each audio source in soundEffects children change the volume to the set value by slider
            {
                source.volume = sfxVolume;
            }
        }
    }

    //to be called by OnCardPicked event or one of stats events
    public void PlayEffect(GameObject effect)
    {
        if (effect.GetComponent<AudioSource>() != null)
        {
            effect.GetComponent<AudioSource>().Play(); //play sound
        }
    }

}
