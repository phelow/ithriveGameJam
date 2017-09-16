using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : Interactable {
    [SerializeField]
    private TextAsset _dialogFile;
    private Dialogue _dialog;

    void Awake()
    {
        _dialog = Dialogue.DialogueFactory(_dialogFile);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public override void OnInteraction()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);
    }
}
