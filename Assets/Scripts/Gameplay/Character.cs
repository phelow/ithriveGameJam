using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    [SerializeField]
    private List<TextAsset> _dialogFileList;
    private List<Dialogue> _dialogList;

    [SerializeField]
    private GameObject _character;

    Dialogue _dialog;

    void Start()
    {
        _dialogList = new List<Dialogue>();
        foreach (TextAsset text in _dialogFileList)
        {
            _dialogList.Add(Dialogue.DialogueFactory(text));
        }
    }

    public void ShowCharacter()
    {
        _character.SetActive(true);
        _dialog = _dialogList[Random.Range(0, _dialogList.Count)];
    }

    public void HideCharacter()
    {
        _character.SetActive(false);
    }

    public void TalkWith()
    {
        TypeWriter.s_instance.PlayDialogue(_dialog);
        
    }
}
