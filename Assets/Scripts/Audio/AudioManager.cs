using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound currSound in sounds)
        {
            currSound.source = gameObject.AddComponent<AudioSource>();
            currSound.source.clip = currSound.clip;

            currSound.source.volume = currSound.volume;
            currSound.source.pitch = currSound.pitch;
        }
    }

    public void Play (string name)
    {
        Sound currSound = Array.Find(sounds, sound => sound.name == name);

        if (currSound == null)
        {
            Debug.Log("ERROR | Unable to find sound called: " + name);
            return;
        }
        currSound.source.Play();
    }
}
