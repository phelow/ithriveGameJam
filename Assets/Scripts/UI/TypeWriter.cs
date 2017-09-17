using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    public static TypeWriter s_instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI textBox;

    private List<Sentence> sentences;
    private int currentPointer;

    private bool playing = false;
    private bool skip = false;
    private bool _animateTextStarted = false;

    private Color _backgroundColorA;
    private Color _backgroundColorB;

    private Color _backgroundColor;

    private IEnumerator AnimateTextRoutine;


    private float _clearOffset;

    // Use this for initialization
    void Awake()
    {
        s_instance = this;
        AnimateTextRoutine = AnimateText();
        dialoguePanel.SetActive(false);

        StartCoroutine(PingPongBackground());
    }

    private void LateUpdate()
    {
        if (playing == true && Input.GetKeyDown(KeyCode.Space))
        {
            if (skip == false)
            {
                Global.dialogueManager.FinishCurrent();
            }
            else
            {
                Global.dialogueManager.SkipCurrent();
            }
        }
    }

    public bool PlayDialogue(Dialogue dialogue)
    {
        if (!playing)
        {
            this.sentences = dialogue._sentences;

            return PlayToEnd();
        }
        else
        {
            /* This Should Never Happen! */
            return false;
        }
    }

    public bool PlayToEnd()
    {
        if (this.sentences != null)
        {
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
        }
        else
        {
            Debug.LogError("TypeWriter started typing with no sentences!");
            return false;
        }
    }

    private static string ToHex(float redValue, float greenValue, float blueValue, float alpha = 1.0f)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}",
            (int)(alpha * 255),
            (int)(redValue * 255),
            (int)(greenValue * 255),
            (int)(blueValue * 255));
    }

    private IEnumerator PingPongBackground()
    {
        while (true)
        {
            _backgroundColor = Color.Lerp(
                Color.Lerp(_backgroundColorA, _backgroundColorB, Mathf.PingPong(Time.time, 1.0f)),
                Color.clear,
                Mathf.PingPong(Time.time + _clearOffset, 1.0f));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AnimateText()
    {
        _animateTextStarted = true;

        _backgroundColorA = new Color(
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f));

        _backgroundColorB = new Color(
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f));
        _clearOffset = Random.Range(0.0f, 1.0f);

        for (int i = 0; i < (this.sentences[currentPointer]._text.Length); i++)
        {
            if (skip)
            {
                break;
            }

            string currentSentence = sentences[currentPointer]._text;

            string typedText = currentSentence.Substring(0, i);
            string trimmedString = currentSentence.Substring(i);

            Color foregroundColor = Color.green;

            string text = string.Format("<{0}>{1}</color><{2}>{3}</color>",
                ToHex(Color.white.r, Color.white.g, Color.white.b), 
                typedText,
                ToHex(_backgroundColor.r, _backgroundColor.g, _backgroundColor.b, _backgroundColor.a), 
                trimmedString);

            textBox.text = text;
            /* TO DO - Add sound? */
            yield return new WaitForSeconds(sentences[currentPointer]._speed); //Lerp down character size
        }

        // If skip == true && dialogue is not done playing
        textBox.text = sentences[currentPointer]._text;

        yield return new WaitForSeconds(sentences[currentPointer]._waitTime);

        NextText();
    }

    public void NextText()
    {
        StopAllCoroutines();
        currentPointer++;
        skip = false;

        if (currentPointer < sentences.Count)
        {
            StartCoroutine(AnimateText());
        }
        else
        {
            /* TO DO - Animate this transition */
            dialoguePanel.SetActive(false);

            /* FINISHED PLAYING */
            playing = false;
        }
    }

    public void FinishCurrent()
    {
        if (playing)
        {
            skip = true;
        }
    }


}
