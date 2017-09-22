using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource _soundSource;
    public AudioSource _dialogSource;
    public AudioSource _musicSource;
    public AudioSource _characterMusicSource;

    public AudioClip nightMusic;
    public AudioClip dayMusic;
    public AudioClip[] voices;

    private int listIndex;
    private bool listPlaying = false;
    private float currentClipTime;

    private float fadeSpeed = 16;

    private float originalMusicVolume = 1f;

    private void Awake(){
        
    }

    public void PlaySingleSound(AudioClip clip)
    {
        _soundSource.Stop();
        _soundSource.PlayOneShot(clip);
    }

    public void PlaySingleDialogue (AudioClip clip) {
        _dialogSource.Stop();
        _dialogSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip) {
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlayCharacterMusic(AudioClip clip) {
        originalMusicVolume = _musicSource.volume;

        StartCoroutine(fadeDown(_musicSource, originalMusicVolume * .8f, fadeSpeed * .2f));
        
        _characterMusicSource.Stop();
        _characterMusicSource.clip = clip;
        _characterMusicSource.volume = 0;
        _characterMusicSource.time = _musicSource.time;
        _characterMusicSource.loop = true;
        _characterMusicSource.Play();

        StartCoroutine(fadeUp(_characterMusicSource, originalMusicVolume, fadeSpeed));
    }

    public void StopCharacterMusic() {
        StartCoroutine(fadeDown(_characterMusicSource, 0, fadeSpeed));
        StartCoroutine(fadeUp(_musicSource, originalMusicVolume, fadeSpeed * .2f));
    }

    public void PlayRandomSound(List<AudioClip> L_clip) {
    }

    public void PlayRandomSwappingSound() {

    }

    public void Stop() {
    }

    IEnumerator fadeUp (AudioSource source, float fadeTo, float speed) {
        if(source.volume < fadeTo) {
            source.volume += speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }else{
            source.volume = fadeTo;
        }
    }

    IEnumerator fadeDown (AudioSource source, float fadeTo, float speed) {
        if (source.volume > fadeTo) {
            source.volume -= speed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        } else {
            source.volume = fadeTo;
            if(fadeTo == 0) {
                source.Stop();
            }
        }
    }

}


