using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    private void Start() 
    {
        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height + " @" + resolutions[i].refreshRate + "Hz";
                if (!options.Contains(option))
                {
                    options.Add(option);
                }

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (volume > -40)
            audioMixer.SetFloat("MasterVolume", volume);
        else
            audioMixer.SetFloat("MasterVolume", -80);
    }

    public void SetMusicVolume(float volume)
    {        
        if (volume > -40)
            audioMixer.SetFloat("MusicVolume", volume);
        else
            audioMixer.SetFloat("MusicVolume", -80);
    }

    public void SetSoundVolume(float volume)
    {
        if (volume > -40)
            audioMixer.SetFloat("SoundVolume", volume);
        else
            audioMixer.SetFloat("SoundVolume", -80);
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
