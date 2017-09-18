using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource _soundSource;
    public AudioSource _dialogSource;
    public AudioSource _musicSource;
    public AudioSource _characterMusicSource;
    public static SoundManager instance = null;

    public AudioClip nightMusic;
    public AudioClip dayMusic;

    private void Awake()
    {

    }

    public void PlaySingleSound(AudioClip clip)
    {
    }

    public void PlayMusic(AudioClip clip)
    {
    }

    public void PlayRandomSound(List<AudioClip> L_clip)
    {
    }

    public void PlayDialog(List<AudioClip> L_clip)
    {

    }

    public void Stop()
    {
    }

}


