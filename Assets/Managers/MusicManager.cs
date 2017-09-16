using UnityEngine;
using System.Collections.Generic;
using System;

public class MusicManager : MonoBehaviour {
	private Dictionary<string, AudioClip> trackMap;
	private AudioSource musiceSource;

	public MusicManager() {
		// Need to initialize music source here
		//musiceSource = ((GameObject)GameObject.Instantiate(Resources.Load(""))).GetComponent<AudioSource>();
        musiceSource = new AudioSource();
        trackMap = new Dictionary<string, AudioClip>();
	}

	public void Play(string name) {
        musiceSource.clip = trackMap[name];
        musiceSource.loop = true;
        musiceSource.Play();
	}

	public void Stop() {
        musiceSource.Stop();
	}

	public void Unload() {

	}
}
