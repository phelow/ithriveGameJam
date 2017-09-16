using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : Interactable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public override void OnInteraction()
    {
        Debug.Log("Interacting with the grave.");
    }
}
