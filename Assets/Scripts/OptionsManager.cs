using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
    [Header("Video")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowModeDropdown;

    [Header("Audio")]
    public Slider generalSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    [Space]
    public AudioMixer mixer;

    private Resolution[] resolutions;

    private void Start()
    {
        // fill up allowed resolutions
        resolutions = GetResolutions();
        List<string> resolutionTexts = new List<string>();

        for (int i = 0; i < resolutions.Length - 1; i++)
            resolutionTexts.Add(resolutions[i].width + "x" + resolutions[i].height);

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionTexts);

        // set gui to show current settings used

        // general settings
        resolutionDropdown.value = GetCurrentResolutionIndex();
        windowModeDropdown.value = (int)Screen.fullScreenMode - 1;

        // load sound values
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("MASTER_VOLUME"));
        mixer.SetFloat("Music", PlayerPrefs.GetFloat("MUSIC_VOLUME"));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX_VOLUME"));

        // audio settings
        mixer.GetFloat("Master", out var volume);
        generalSlider.value = AudioToSlider(volume);

        mixer.GetFloat("Music", out volume);
        musicSlider.value = AudioToSlider(volume);

        mixer.GetFloat("SFX", out volume);
        sfxSlider.value = AudioToSlider(volume);
    }

    #region General
    private Resolution[] GetResolutions()  // filters out duplicate resolutions, and reverses the order
    {
        List<Resolution> res = new List<Resolution>();
        res.Add(Screen.resolutions[Screen.resolutions.Length - 1]);

        for (int i = Screen.resolutions.Length - 2; i > -1; i--)
        {
            if (Screen.resolutions[i].width != Screen.resolutions[i + 1].width || Screen.resolutions[i].height != Screen.resolutions[i + 1].height) // ignore refresh rate changes
                res.Add(Screen.resolutions[i]);
        }

        res.Add(Screen.resolutions[0]);
        return res.ToArray();
    }
    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == PlayerPrefs.GetInt("RESOLUTION_X") &&
                resolutions[i].height == PlayerPrefs.GetInt("RESOLUTION_Y"))
            {
                return i;
            }
        }

        return resolutions.Length - 1;
    }
    public void OnChangeWindowMode()
    {
        PlayerPrefs.SetInt("WINDOW_MODE", windowModeDropdown.value + 1);
        Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("WINDOW_MODE");
    }

    public void OnChangeResolution()
    {
        PlayerPrefs.SetInt("RESOLUTION_X", resolutions[resolutionDropdown.value].width);
        PlayerPrefs.SetInt("RESOLUTION_Y", resolutions[resolutionDropdown.value].height);
        Screen.SetResolution(PlayerPrefs.GetInt("RESOLUTION_X"), PlayerPrefs.GetInt("RESOLUTION_Y"), Screen.fullScreenMode);
    }
    #endregion

    #region Audio
    public void OnChangeMaster(float value)
    {
        float audio = SliderToAudio(value);
        PlayerPrefs.SetFloat("MASTER_VOLUME", audio);
        mixer.SetFloat("Master", audio);

    }
    public void OnChangeMusic(float value)
    {
        float audio = SliderToAudio(value);
        PlayerPrefs.SetFloat("MUSIC_VOLUME", audio);
        mixer.SetFloat("Music", audio);
    }
    public void OnChangeSFX(float value)
    {
        float audio = SliderToAudio(value);
        PlayerPrefs.SetFloat("SFX_VOLUME", audio);
        mixer.SetFloat("SFX", audio);
    }

    private static float SliderToAudio(float value)
    {
        if (value == 0)
        {
            return -80;
        }
        else
        {
            return Mathf.Log10(value) * 20;
        }
    }

    private static float AudioToSlider(float value)
    {
        if (Math.Abs(value - (-80)) < 0.3f)
            return 0;

        return (float)Math.Pow(10, value / 20f);
    }
    #endregion
}