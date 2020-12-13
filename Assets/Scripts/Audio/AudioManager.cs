using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource AS;
    public AudioClip dropAudio;

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
