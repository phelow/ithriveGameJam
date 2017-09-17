using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;

    public List<AudioClip> _audioClips;

    private Dialogue _dialog;
    private int randomnumber;

    void Awake()
    {
        _dialogList = new List<Dialogue>();
        _audioClips = new List<AudioClip>();
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
    }

    public void ShowCharacter()
    {
        // Debug.Log("SIZE:" + _dialogList.Count);
        randomnumber = Random.Range(0, _dialogList.Count);
        _dialog = _dialogList[randomnumber];
    }

    
   

    private void OnMouseDown()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);
        SoundManager.instance.Play(_audioClips[randomnumber]);
    }
}
