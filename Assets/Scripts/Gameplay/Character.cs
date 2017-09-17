using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;

    public List<AudioClip> _audioClips;

    private Dialogue _dialog;

    [SerializeField]
    private float _maximumAlpha = 1.0f;
    [SerializeField]
    private float _minimumAlpha = 0.0f;
    [SerializeField]
    private float _interpolationTime = 1.0f;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private float _startingOffset;

    [SerializeField]
    private Color _characterBaseColor = Color.black;

    [SerializeField]
    private float _colorInterpolationTime = 1.0f;

    [SerializeField]
    private Color _characterActiveColor;

    private bool isTalking = false;


    void Awake()
    {
        _dialogList = new List<Dialogue>();
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(InterpolateCharacters());
        StartCoroutine(UpdateActiveColor());
    }

    private IEnumerator UpdateActiveColor()
    {
        while (true)
        {
            float tPassed = 0.0f;
            while(tPassed < _colorInterpolationTime)
            {
                _characterActiveColor = Color.Lerp(Color.white, _characterBaseColor, tPassed / _colorInterpolationTime);
                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            while (tPassed > 0.0f)
            {
                _characterActiveColor = Color.Lerp(Color.white, _characterBaseColor, tPassed / _colorInterpolationTime);
                tPassed -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private IEnumerator InterpolateCharacters()
    {
        yield return new WaitForSeconds(_startingOffset);

        while (true)
        {
            float tPassed = 0.0f;
            while(tPassed < _interpolationTime)
            {
                _renderer.color = Color.Lerp(
                    new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, _maximumAlpha), 
                    new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, _minimumAlpha), 
                    tPassed / _interpolationTime);

                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            while (tPassed > 0.0f)
            {
                _renderer.color = Color.Lerp(
                    new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, _maximumAlpha), 
                    new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, _minimumAlpha), 
                    tPassed / _interpolationTime);

                tPassed -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

        }
    }

    public void ShowCharacter()
    {
        // Debug.Log("SIZE:" + _dialogList.Count);
        _dialog = _dialogList[Random.Range(0, _dialogList.Count)];
    }

    
   

    private void OnMouseDown()
    {
        Talk();
    }

    public void Talk()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);


        LevelManager.s_instance.SetTalking(_renderer.sprite);
        isTalking = true;
    }



    public void Update()
    {
        if (TypeWriter.s_instance.IsTextPlaying() && isTalking)
        {
            if (!SoundManager.instance._dialogSource.isPlaying)
            {
                SoundManager.instance.PlayDialog(_audioClips);
            }
        }
        else
        {
            SoundManager.instance._dialogSource.Stop();
            isTalking = false;
        }
    }

}
