using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour {
    public static TypeWriter s_instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI textBox;

    private List<Sentence> sentences;
    private int currentPointer;

    private bool playing = false;
    private bool skip = false;
    private bool _animateTextStarted = false;

    private IEnumerator AnimateTextRoutine;

    // Use this for initialization
    void Awake () {
        s_instance = this;
        AnimateTextRoutine = AnimateText();
        dialoguePanel.SetActive(false);
    }

    private void LateUpdate () {
        if (playing == true && Input.GetKeyDown(KeyCode.Space)) {
            if (skip == false) {
                Global.dialogueManager.FinishCurrent();
            } else {
                Global.dialogueManager.SkipCurrent();
            }
        }
    }

    public bool PlayDialogue (Dialogue dialogue) {
        if (!playing) {
            this.sentences = dialogue._sentences;

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
            if (_animateTextStarted)
            {
                StopCoroutine(AnimateTextRoutine);
                AnimateTextRoutine = AnimateText();
            }

            skip = false;
            StartCoroutine(AnimateTextRoutine);
            return true;
        } else {
            Debug.LogError("TypeWriter started typing with no sentences!");
            return false;
        }
    }

    IEnumerator AnimateText () {
        _animateTextStarted = true;
        for (int i = 0; i < (this.sentences[currentPointer]._text.Length + 1); i++) {
            if (skip) {
                break;
            }

            textBox.text = sentences[currentPointer]._text.Substring(0, i);
            /* TO DO - Add sound? */

            yield return new WaitForSeconds(sentences[currentPointer]._speed);
        }

        // If skip == true && dialogue is not done playing
        textBox.text = sentences[currentPointer]._text;

        yield return new WaitForSeconds(sentences[currentPointer]._waitTime);

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
