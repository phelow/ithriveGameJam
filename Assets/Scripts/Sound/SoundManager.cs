using UnityEngine;
using System.Collections;

public class SoundManager {

    public AudioSource footsteps;
    public AudioSource ghostTalk;

    public SoundManager() {

    }
    
    public void Play(AudioSource audio)
    {
        audio.Play();
    }

}
