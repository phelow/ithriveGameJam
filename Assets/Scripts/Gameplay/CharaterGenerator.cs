using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaterGenerator : MonoBehaviour {
	public GameObject character;
	public List<GameObject> characters;
	public int numsOfCharaters;
	public float duaration;
	public float[] charaterStartTime;
	public Vector3[] charaterPostions;
	//private int tempIndex = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numsOfCharaters; ++i) {
			if (Time.fixedTime == charaterStartTime [i]) {
				GameObject clone = (GameObject)GameObject.Instantiate (character);
				clone.transform.position = charaterPostions[i];
				characters.Add (clone);
				Debug.Log (i);
			}
		}

		for (int i = 0; i < characters.Count; ++i) {
			//Debug.Log (characters.Count);
			if (Time.fixedTime >= charaterStartTime[i]+duaration) {
				characters [i].SetActive (false);
			}
		}
	}
}
