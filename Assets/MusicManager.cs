using UnityEngine;
using System.Collections.Generic;
using System;

public class MusicManager : MonoBehaviour {
	private Dictionary<string, AudioClip> trackMap;
	private AudioSource musiceSource;

	public MusicManager() {
		// Need to initialize music source here
		musiceSource = ((GameObject)GameObject.Instatiatle(Resources.Load(""))).GetComponent<AudioSource>();
		trackMap = new Dictionary<string, AudioClip> ();
	}

	public void Play(string name) {

	}

	public void Stop() {

	}

	public void Unload() {

	}
}
