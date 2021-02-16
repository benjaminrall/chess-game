using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

	void Awake()
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
		}
	}

    private AudioSource AS;
    public AudioClip dropAudio;

    public float masterVolume;
    public float musicVolume;
    public float soundVolume;

    void Start()
    {
        AS = GetComponent<AudioSource>();
    }

    public void DropPiece()
    {
        AS.clip = dropAudio;
        AS.Play();
    }
}
