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

    void Awake()
    {
        _dialogList = new List<Dialogue>();
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(InterpolateCharacters());
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
                    new Color(Color.white.r, Color.white.g, Color.white.b, _maximumAlpha), 
                    new Color(Color.white.r, Color.white.g, Color.white.b, _minimumAlpha), 
                    tPassed / _interpolationTime);

                tPassed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            tPassed = 0.0f;
            while (tPassed < _interpolationTime)
            {
                _renderer.color = Color.Lerp(
                    new Color(Color.white.r, Color.white.g, Color.white.b, _minimumAlpha), 
                    new Color(Color.white.r, Color.white.g, Color.white.b, _maximumAlpha), 
                    tPassed / _interpolationTime);

                tPassed += Time.deltaTime;
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
        SoundManager.instance.Play(_audioClips[Random.Range(0, _audioClips.Count)]);
    }
}
