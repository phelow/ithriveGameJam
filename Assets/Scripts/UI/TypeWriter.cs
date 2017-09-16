using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour {

    public GameObject dialoguePanel;
    public Text textBox;

    private List<Sentence> sentences;
    private int currentPointer;

    private bool playing = false;
    private bool skip = false;

    // Use this for initialization
    void Awake () {
        dialoguePanel.SetActive(false);
    }

    private void LateUpdate () {
        if (playing == true && Input.GetButtonDown("Fire1")) {
            if (skip == false) {
                Global.dialogueManager.FinishCurrent();
            } else {
                Global.dialogueManager.SkipCurrent();
            }
        }
    }

    public bool PlayDialogue (Dialogue dialogue) {
        if (!playing) {
            this.sentences = dialogue.sentences;

            return PlayToEnd();
        } else {
            /* This Should Never Happen! */
            return false;
        }
    }

    public bool PlayToEnd () {
        if (this.sentences != null) {
            playing = true;
            currentPointer = 0;

            /* TO DO - Animate this transition */
            dialoguePanel.SetActive(true);
            
            StartCoroutine(AnimateText());
            return true;
        } else {
            Debug.LogError("TypeWriter started typing with no sentences!");
            return false;
        }
    }

    IEnumerator AnimateText () {
        for (int i = 0; i < (this.sentences[currentPointer].text.Length + 1); i++) {
            if (skip) {
                break;
            }

            textBox.text = sentences[currentPointer].text.Substring(0, i);
            /* TO DO - Add sound? */

            yield return new WaitForSeconds(sentences[currentPointer].speed);
        }

        // If skip == true && dialogue is not done playing
        textBox.text = sentences[currentPointer].text;
        skip = true;

        yield return new WaitForSeconds(sentences[currentPointer].waitTime);

        NextText();
    }

    public void NextText () {
        StopAllCoroutines();
        currentPointer++;
        skip = false;

        if (currentPointer < sentences.Count) {
            StartCoroutine(AnimateText());
        } else {
            /* TO DO - Animate this transition */
            dialoguePanel.SetActive(false);

            /* FINISHED PLAYING */
            playing = false;
        }
    }

    public void FinishCurrent () {
        if (playing) {
            skip = true;
        }
    }


}
