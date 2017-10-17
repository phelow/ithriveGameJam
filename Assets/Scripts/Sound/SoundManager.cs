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
    public AudioClip[] sounds;

    private int listIndex;
    //private bool listPlaying = false;
    private float currentClipTime;

    //private float fadeSpeed = 8;
    private float musicFadeSpeed = 2;
    private float characterMusicFadeSpeed = 1;

    private float musicMinVolume = .7f;
    private float characterMusicMinVolume = .5f;


    public float masterVolume = 1f;
    private Dictionary<AudioSource, float> volumes = new Dictionary<AudioSource, float>();
        

    private void Awake(){
        if(Global.soundManager != this)
        {
            Destroy(gameObject);
            return;
        }

        volumes.Add(_soundSource, 1f);
        volumes.Add(_dialogSource, 1f);
        volumes.Add(_musicSource, 1f);
        volumes.Add(_characterMusicSource, 1f);
        PlayMusic();
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

    public void PlayRandomSwappingSound() {

    }

    public void PlayMusic() {
        _musicSource.Stop();
        _musicSource.clip = dayMusic;
        _musicSource.loop = true;
        UpdateVolume(_musicSource);
        _musicSource.Play();
    }

    public void DayToNight() {
        StopAllCoroutines();
        StartCoroutine(fadeTo(_musicSource, nightMusic, musicFadeSpeed));
        if (_characterMusicSource.isPlaying)
        {
            StartCoroutine(fadeDown(_characterMusicSource, 0, musicFadeSpeed));
        }
    }

    public void NightToDay() {
        StopAllCoroutines();
        StartCoroutine(fadeTo(_musicSource, dayMusic, musicFadeSpeed));
        if (_characterMusicSource.isPlaying)
        {
            StartCoroutine(fadeDown(_characterMusicSource, 0, musicFadeSpeed));
        }
    }

    public void PlayCharacterMusic(AudioClip clip) {
        StopAllCoroutines();

        StartCoroutine(fadeDown(_musicSource, musicMinVolume, characterMusicFadeSpeed));

        if (clip != _characterMusicSource.clip)
        {
            StartCoroutine(fadeTo(_characterMusicSource, clip, characterMusicFadeSpeed, true));
            _characterMusicSource.loop = true;
        }
        else
        {
            StartCoroutine(fadeUp(_characterMusicSource, 1f, characterMusicFadeSpeed));
        }
    }

    public void StopCharacterMusic() {
        StartCoroutine(fadeDown(_characterMusicSource, characterMusicMinVolume, characterMusicFadeSpeed));
        StartCoroutine(fadeUp(_musicSource, 1f, characterMusicFadeSpeed));
    }
    
    IEnumerator fadeUp (AudioSource source, float fadeTo, float speed) {
        var currentVol = GetVolume(source);
        
        while (currentVol < fadeTo) {
            currentVol += speed * Time.fixedDeltaTime;
            UpdateVolume(source, currentVol);
            yield return new WaitForFixedUpdate();
        }
        UpdateVolume(source, fadeTo);
    }

    IEnumerator fadeDown (AudioSource source, float fadeTo, float speed) {
        var currentVol = GetVolume(source);

        while (currentVol > fadeTo) {
            currentVol -= speed * Time.fixedDeltaTime;
            UpdateVolume(source, currentVol);
            yield return new WaitForFixedUpdate();
        }
        UpdateVolume(source,fadeTo);
        if(fadeTo == 0) {
            source.Stop();
        }
    }

    IEnumerator fadeTo (AudioSource source, AudioClip clip, float speed, bool keepTime = false) {
        var currentVol = GetVolume(source);

        while (currentVol > 0) {
            currentVol -= speed * Time.fixedDeltaTime;
            UpdateVolume(source, currentVol);
            yield return new WaitForFixedUpdate();
        }
        UpdateVolume(source, 0);
        currentVol = 0;
        var previousTime = source.time;

        source.Stop();
        source.clip = clip;
        if (keepTime)
        {
            source.time = previousTime;
        }
        source.Play();
        while (currentVol < 1f) {
            currentVol += speed * Time.fixedDeltaTime;
            UpdateVolume(source, currentVol);
            yield return new WaitForFixedUpdate();
        }
        UpdateVolume(source);
    }

    public void SetVolumes(float master = 1f, float music = 1f, float effects = 1f) {
        var musicVol = GetVolume(_musicSource);
        var characterMusicVol = GetVolume(_characterMusicSource);
        var soundVol = GetVolume(_soundSource);
        var dialogVol = GetVolume(_dialogSource);

        masterVolume = master;
        volumes[_musicSource] = music;
        volumes[_characterMusicSource] = music;
        volumes[_soundSource] = effects;
        volumes[_dialogSource] = effects;

        UpdateVolume(_musicSource, musicVol);
        UpdateVolume(_characterMusicSource, characterMusicVol);
        UpdateVolume(_soundSource, soundVol);
        UpdateVolume(_dialogSource, dialogVol);
    }

    private void UpdateVolume(AudioSource audio, float volume = 1f) {
        
        audio.volume = volumes[audio] * masterVolume * volume;
    }

    private float GetVolume(AudioSource audio) {
        return audio.volume / volumes[audio] / masterVolume;
    }

}


