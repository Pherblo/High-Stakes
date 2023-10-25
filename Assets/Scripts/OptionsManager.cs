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
        // figure out allowed resolutions and convert to strings
        resolutions = GetResolutions();
        List<string> resolutionTexts = new List<string>();

        for (int i = 0; i < resolutions.Length - 1; i++)
            resolutionTexts.Add(resolutions[i].width + "x" + resolutions[i].height);

        // add resolution options to dropbox
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionTexts);

        // set video dropdowns to screen settings
        resolutionDropdown.value = GetCurrentResolutionIndex();
        windowModeDropdown.value = (int)Screen.fullScreenMode - 1;

        // load sound values from playerprefs
        mixer.SetFloat("Master", PlayerPrefs.GetFloat("MASTER_VOLUME", 0));
        mixer.SetFloat("Music", PlayerPrefs.GetFloat("MUSIC_VOLUME", 0));
        mixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX_VOLUME", 0));

        // set audio sliders to audio values
        mixer.GetFloat("Master", out var volume);
        generalSlider.value = volume;

        mixer.GetFloat("Music", out volume);
        musicSlider.value = volume;

        mixer.GetFloat("SFX", out volume);
        sfxSlider.value = volume;
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
    private int GetCurrentResolutionIndex() // find what resolution were at from playerprefs
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
        PlayerPrefs.SetFloat("MASTER_VOLUME", value);
        mixer.SetFloat("Master", value);

    }
    public void OnChangeMusic(float value)
    {
        PlayerPrefs.SetFloat("MUSIC_VOLUME", value);
        mixer.SetFloat("Music", value);
    }
    public void OnChangeSFX(float value)
    {
        PlayerPrefs.SetFloat("SFX_VOLUME", value);
        mixer.SetFloat("SFX", value);
    }
    #endregion
}