using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    AudioSource aSelf;
    public AudioClip[] clips;
    
    void Awake()
    {
        aSelf = GetComponent<AudioSource>();
    }

    public void Play(string name)
    {
        //if (!aSelf.isPlaying)
        {
            foreach (AudioClip clip in clips)
                {
                    if (clip.name == name)
                    {
                        aSelf.Stop();
                        aSelf.clip = clip;
                        aSelf.Play();
                    }
                }
        }
        
    }
}
