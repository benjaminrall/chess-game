using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    
    public AudioMixer audioMixer;

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
}
