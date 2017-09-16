using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour {

    private void Awake () {

        /* The game is being loaded correctly */
        Global.isBuild = true;

        /* Find UI Elements */
        GameObject dialogueUI = GameObject.Find("UI_Dialogue");
        GameObject musicSource = GameObject.Find("Audio_Music");

        /* Create Managers */
        Global.storyManager = new StoryManager();
        Global.musicManager = new MusicManager(musicSource);
        Global.soundManager = new SoundManager();
        Global.dialogueManager = new DialogueManager(dialogueUI);
        Global.timeManager = new TimeManager(5);

        /* Load Title Scene */
        // Make sure we're in the Bootloader scene, you never know...

        if (SceneManager.GetActiveScene().buildIndex == 0) {
            SceneManager.LoadScene(1);
        }

    }

}
