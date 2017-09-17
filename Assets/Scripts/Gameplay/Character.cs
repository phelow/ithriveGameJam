using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;

    public List<AudioClip> _audioClips;

    private Dialogue _dialog;

    void Awake()
    {
        _dialogList = new List<Dialogue>();
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
    }

    public void ShowCharacter()
    {
        // Debug.Log("SIZE:" + _dialogList.Count);
        _dialog = _dialogList[Random.Range(0, _dialogList.Count)];
    }

    
   

    private void OnMouseDown()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);
        SoundManager.instance.Play(_audioClips[Random.Range(0, _audioClips.Count)]);
    }
}
