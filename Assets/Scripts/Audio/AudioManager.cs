using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource AS;
    public AudioClip dropAudio;
    public float currentVolume;

    void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    public void dropPiece()
    {
        AS.clip = dropAudio;
        AS.Play();
    }
}
