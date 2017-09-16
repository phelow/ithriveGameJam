using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource footsteps;
    public AudioSource ghostTalk;
    
    public void Play(AudioSource audio)
    {
        audio.Play();
        
    }
}
