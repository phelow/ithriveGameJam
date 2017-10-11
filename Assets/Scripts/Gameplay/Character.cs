using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;

    private Dialogue _dialog;
    private int _dialogIndex;

    [SerializeField]
    private float _maximumAlpha = 1.0f;
    [SerializeField]
    private float _minimumAlpha = 0.0f;
    [SerializeField]
    private float _interpolationTime = 1.0f;
    [SerializeField]
    private AnimationCurve alphaCurve;

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

    [SerializeField]
    public AudioClip _characterMusic;

    public bool isGhost;
    private BoxCollider2D _collider;

    private float fadeTime = 10f;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _dialogList = new List<Dialogue>();
        _collider = GetComponent<BoxCollider2D>();
        
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
        
    }

    private IEnumerator UpdateActiveColor()
    {
        while (true)
        {
            float tPassed = 0.0f;
            while (tPassed < _colorInterpolationTime)
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

        float alpha;
        
        while (true)
        {
            float tPassed = 0.0f;
            while (tPassed < _interpolationTime)
            {

                alpha = (_minimumAlpha - _maximumAlpha) * alphaCurve.Evaluate(tPassed / _interpolationTime) + _maximumAlpha;
                _renderer.color = new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, alpha);
                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            while (tPassed > 0.0f)
            {
                alpha = (_minimumAlpha - _maximumAlpha) * alphaCurve.Evaluate(tPassed / _interpolationTime) + _maximumAlpha;
                _renderer.color = new Color(_characterActiveColor.r, _characterActiveColor.g, _characterActiveColor.b, alpha);
                tPassed -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

        }
    }

    public void ShowCharacter()
    {
        /*_dialog = _dialogList[Random.Range(0, _dialogList.Count)];*/
        if (_dialogList.Count > 1) {
            _dialogIndex = UnityEngine.Random.Range(0, _dialogList.Count);
        }else {
            _dialogIndex = 0;
        }

        if (isGhost)
        {
            StartCoroutine(InterpolateCharacters());
            StartCoroutine(UpdateActiveColor());
        }
    }




    private void OnMouseDown()
    {
        Talk();
    }

    private void OnMouseEnter() {
        _renderer.transform.localScale = Vector3.Scale(new Vector3(1.1f, 1.1f), _renderer.transform.localScale);
    }
    private void OnMouseExit() {
        _renderer.transform.localScale = Vector3.Scale(new Vector3(.9f, .9f), _renderer.transform.localScale);
    }

    public void Talk()
    {
        if (TypeWriter.s_instance.IsTextPlaying())
        {
            return;
        }

        TypeWriter.s_instance.PlayDialogue(_dialogList[_dialogIndex]);
        if (_dialogList.Count > 1) {
            _dialogIndex++;
            if(_dialogIndex >= _dialogList.Count) {
                _dialogIndex = 0;
            }
        } else {
            _dialogIndex = 0;
        }

        if(_characterMusic != null)
        {
            Global.soundManager.PlayCharacterMusic(_characterMusic);
        }

        LevelManager.s_instance.SetTalking(_renderer.sprite);
    }

    public void DisableTalk() {
        _collider.enabled = false;
        
    }

    public void EnableTalk() {
        _collider.enabled = true;
    }
    
    public void Fade(float alpha, float time) {
        StopAllCoroutines();
        StartCoroutine(FadeOut(alpha, time));
    }

    private IEnumerator FadeOut(float a, float t) {
        float alpha = _renderer.color.a;
        float startA = alpha;
        float time = t * (_maximumAlpha - alpha);
        float tPassed = 0f;

        while (tPassed < time)
        {
            alpha = startA - alphaCurve.Evaluate(tPassed / time)*(startA -a);
            _renderer.color = new Color(1f, 1f, 1f, alpha);
            tPassed += Time.deltaTime;
            yield return null;
        }
    }
}
