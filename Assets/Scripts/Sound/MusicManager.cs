using UnityEngine;
using System.Collections.Generic;
using System;

public class MusicManager {
	private Dictionary<string, AudioClip> trackMap;
	private AudioSource musiceSource;

	public MusicManager() {
		// Need to initialize music source here
		musiceSource = ((GameObject)GameObject.Instantiate(Resources.Load(""))).GetComponent<AudioSource>();
		trackMap = new Dictionary<string, AudioClip> ();
	}

	public void Play(string name) {
        musiceSource.clip = trackMap[name];
        musiceSource.Play();
	}

	public void Stop() {
        musiceSource.Stop();
	}

	public void Unload() {

	}
}
