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
    public GameObject blur;

    private List<Sentence> sentences;
    private int currentPointer;

    private bool playing = false;
    private bool skip = false;
	private bool dialogueStart = true;
    private bool _animateTextStarted = false;

    private Color _backgroundColorA;
    private Color _backgroundColorB;

    private Color _backgroundColor;

    private IEnumerator AnimateTextRoutine;

    [SerializeField]
    private GameObject _ghostLetter;


    private float _clearOffset;

    // Use this for initialization
    void Awake()
    {
        if(s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;
        AnimateTextRoutine = AnimateText();
        dialoguePanel.SetActive(false);
        blur.SetActive(false);

        StartCoroutine(PingPongBackground());
    }

    void Update()
    {
        if (playing && Input.GetButtonDown("Fire1"))
        {
			if(!dialogueStart) {
				skip = true;
			}else{
				dialogueStart = false;
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
            blur.SetActive(true);
            LevelManager.s_instance.SetAdvanceButtonVisible(playing);
            currentPointer = 0;

            /* TO DO - Animate this transition */
            dialoguePanel.SetActive(true);
            if (_animateTextStarted)
            {
                StopCoroutine(AnimateTextRoutine);
                AnimateTextRoutine = AnimateText();
            }

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
        int voiceIndex = sentences[currentPointer]._soundIndex;
        if (voiceIndex >= 0) {
            Global.soundManager.PlaySingleDialogue(Global.soundManager.voices[voiceIndex]);
        }
        _animateTextStarted = true;
        skip = false;
        _backgroundColorA = new Color(
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f));

        _backgroundColorB = new Color(
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f),
            Random.Range(0.1f, 1.0f));
        _clearOffset = Random.Range(0.0f, 1.0f);
        //float whiteOffset = Random.Range(0.0f, 1.0f);
        if (currentPointer > this.sentences.Count)
        {
            currentPointer = 0;
        }

        for (int i = 0; i < (this.sentences[currentPointer]._text.Length); i++)
        {

            string currentSentence = sentences[currentPointer]._text;

            string typedText = currentSentence.Substring(0, i);
            string trimmedString = currentSentence.Substring(i);
            /*
            Color whiteShade = Color.Lerp(
                Color.clear,
                Color.white,
                .6f + Mathf.PingPong(Time.time + whiteOffset, .4f));
                */
            if (typedText.Length > 0)
            {
                int substringStartIndex = Random.Range(0, i);
                float spaceOutAmount = Screen.height;

                int charactersCreated = (int)Random.Range(1.0f, Mathf.Max(1.0f, sentences[currentPointer]._spookiness));

                for (int c = 0; c < charactersCreated; c++)
                {
                    GameObject createdText = GameObject.Instantiate(
                    _ghostLetter,
                    this.textBox.transform.position + new Vector3(Random.Range(-spaceOutAmount, spaceOutAmount), Random.Range(-spaceOutAmount, spaceOutAmount), 0.0f),
                    this.transform.rotation,
                    this.transform);

                    createdText.GetComponent<FadingLetter>().SetSpookiness(sentences[currentPointer]._spookiness);

                    if (sentences[currentPointer]._words.Length <= 1)
                    {
                        int createdTextStringLength = Random.Range(0, typedText.Length - substringStartIndex);

                        string createdTextString = typedText.Substring(
                                    substringStartIndex,
                                    createdTextStringLength);
                        createdText.GetComponentInChildren<TextMeshProUGUI>().SetText(createdTextString);
                    }
                    else
                    {

                        createdText.GetComponentInChildren<TextMeshProUGUI>().SetText(sentences[currentPointer]._words[Random.Range(0, sentences[currentPointer]._words.Length)]);
                    }
                }
            }

            string text = string.Format("<{0}>{1}</color><{2}>{3}</color>",
                ToHex(Color.white.r, Color.white.g, Color.white.b),
                typedText,
                ToHex(_backgroundColor.r, _backgroundColor.g, _backgroundColor.b, _backgroundColor.a),
                trimmedString);

            textBox.text = text;
            /* TO DO - Add sound? */

            if (skip)
            {
                continue;
            }

            yield return new WaitForSeconds(sentences[currentPointer]._speed * .4f); //Lerp down character size
        }

        // If skip == true && dialogue is not done playing
        textBox.text = sentences[currentPointer]._text;
        skip = false;

        float tPassed = 0.0f;
        while (tPassed < sentences[currentPointer]._waitTime * 5 && !skip)
        {
            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        skip = false;
        NextText();
    }

    public bool IsTextPlaying()
    {
        return playing;
    }

    public void NextText()
    {
        StopAllCoroutines();
        currentPointer++;

        if (currentPointer < sentences.Count)
        {
            StartCoroutine(AnimateText());
        }
        else
        {
            /* TO DO - Animate this transition */
            dialoguePanel.SetActive(false);
            
            FinishedPlaying();
        }
    }

    private void FinishedPlaying() {
        playing = false;
        dialogueStart = true;
        blur.SetActive(false);
        LevelManager.s_instance.SetAdvanceButtonVisible(playing);
        LevelManager.s_instance.ClearTalking();
        Global.soundManager.StopCharacterMusic();
    }

    public void FinishCurrent()
    {
        if (playing)
        {
            skip = true;
        }
    }


}
