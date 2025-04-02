using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public AudioSource musicAudioSource;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("SoundManager");
                    _instance = singletonObject.AddComponent<SoundManager>();
                    _instance.musicAudioSource = singletonObject.AddComponent<AudioSource>();
                }
            }
            return _instance;
        }
    }

    public void Play(GameObject caller, AudioClip clip)
    {
        AudioSource audioSource = caller.GetComponent<AudioSource>();
        if (!audioSource)
            audioSource = caller.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}
