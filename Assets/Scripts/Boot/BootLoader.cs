using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour {

    private void Awake () {

        // Make sure we're in the correct scene...
        

        /* The game is being loaded correctly */
        Global.isBuild = true;

        /* Find UI Elements */
        GameObject dialogueUI = GameObject.Find("UI_Dialogue");
        //GameObject musicSource = GameObject.Find("Audio_Music");

        /* Create Managers */
       
        Global.soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Global.dialogueManager = new DialogueManager(dialogueUI);

    }

}
