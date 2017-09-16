using UnityEngine;
using System.Collections;

public class SoundManager {

    public AudioSource footsteps;
    public AudioSource ghostTalk;
    
    public void Play(AudioSource audio)
    {
        audio.Play();
        
    }
}
