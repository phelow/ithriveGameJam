using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource _soundSource;
    public AudioSource _dialogSource;
    public AudioSource _musicSource;
    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

    }

    public void PlaySingleSound(AudioClip clip)
    {
        _soundSource.clip = clip;
        _soundSource.Play();
    }


    public void PlayRandomSound(List<AudioClip> L_clip)
    {
        int random = Random.Range(0, L_clip.Count);
        _soundSource.clip = L_clip[random];
        _soundSource.Play();
    }

    public void PlayDialog(List<AudioClip> L_clip)
    {
        int random = Random.Range(0, L_clip.Count);
        _dialogSource.PlayOneShot(L_clip[random]);

    }

    public void Stop()
    {
        _soundSource.Stop();
    }

}


