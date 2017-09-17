using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;
    
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

    
    public void TalkWith()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);
        
    }
}
