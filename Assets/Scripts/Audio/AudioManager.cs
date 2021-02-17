using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;

    public SoundType soundType;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume = 1f;
    [Range(0.1f,3f)]
    public float pitch = 1f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public enum SoundType
{
    MASTER,
    MUSIC,
    SOUND
}

public class AudioManager : MonoBehaviour
{
    private AudioSource AS;
    
    public Sound[] sounds;

    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;

    public static AudioManager instance;

    private void Awake()
	{
		if (instance != null)
		{
			if (instance != this)
			{
				Destroy(this.gameObject);
			}
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(this);
            foreach(Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                if (s.soundType == SoundType.MASTER) 
                    s.source.outputAudioMixerGroup  = masterMixer;
                else if (s.soundType == SoundType.MUSIC)
                    s.source.outputAudioMixerGroup  = musicMixer;
                else if (s.soundType == SoundType.SOUND)
                    s.source.outputAudioMixerGroup  = soundMixer;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
		}
	}

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) 
        {
            Debug.LogError($"Sound {name} not found.");
            return;
        }
        s.source.Play();
    }
}
