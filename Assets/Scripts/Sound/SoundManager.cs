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
        _musicSource.volume = .5f;
        _characterMusicSource.Stop();
        _characterMusicSource.clip = clip;
        _characterMusicSource.time = _musicSource.time;
        _characterMusicSource.loop = true;
        _characterMusicSource.Play();
    }

    public void StopCharacterMusic() {
        _characterMusicSource.Stop();
        _musicSource.volume = originalMusicVolume;
    }

    public void PlayRandomSound(List<AudioClip> L_clip)
    {
    }

    public void PlayDialog(List<AudioClip> L_clip){
        if (!listPlaying) {
            listIndex = 0;
            listPlaying = true;
            StartCoroutine(PlayClipList(L_clip, _dialogSource));
        }
    }

    public void PlayRandomSwappingSound()
    {

    }

    public void Stop()
    {
    }

    IEnumerator PlayClipList (List<AudioClip> L_clip, AudioSource _source) {
        while (true) {
            currentClipTime = L_clip[listIndex].length;
            _source.PlayOneShot(L_clip[listIndex]);
            Debug.Log("Clip Being Played - " + (listIndex + 1) + " of " + L_clip.Count);
            listIndex++;

            if (listIndex < L_clip.Count) {
                yield return new WaitForSeconds(currentClipTime);
            } else {
                listPlaying = false;
                break;
            }
        }
    }

}


