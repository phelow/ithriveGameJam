using UnityEngine;
using System.Collections.Generic;
using System;

public class MusicManager {
	private Dictionary<string, AudioClip> trackMap;
	private AudioSource musicSource;

	public MusicManager(GameObject musicSourceObj) {
        musicSource = musicSourceObj.GetComponent<AudioSource>();
		trackMap = new Dictionary<string, AudioClip> ();
	}

	public void Play(string name) {
        musicSource.clip = trackMap[name];
        musicSource.Play();
	}

	public void Stop() {
        musicSource.Stop();
	}

	public void Unload() {

	}
}
