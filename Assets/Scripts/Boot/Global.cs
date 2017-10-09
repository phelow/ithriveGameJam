using UnityEngine;
using System.Collections;
using System;

public class Global : MonoBehaviour {
    
    private static SoundManager _soundManager;
	public static SoundManager soundManager
    {
        get
        {
            if(_soundManager== null)
            {
                _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            }
            return _soundManager;
        }
    }
    private static DialogueManager _dialogueManager;
    public static DialogueManager dialogueManager
    {
        get
        {
            if (_dialogueManager == null)
            {
                GameObject dialogueUI = GameObject.Find("UI_Dialogue");
                _dialogueManager = dialogueUI.GetComponent<DialogueManager>();
                isBuild = true;
            }
            
            return _dialogueManager;
        }
    }

    public static bool isBuild = false;

    private void Awake() {
        if (isBuild)
        {
            Destroy(gameObject);
        }
    }


}
