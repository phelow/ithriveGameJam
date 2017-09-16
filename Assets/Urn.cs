using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Pedestal _pedestal;

    [SerializeField]
    private Sprite _ghostSprite;

    [SerializeField]
    private Sprite _urnSprite;

    private static Urn _heldObject;
    [SerializeField]
    List<TextAsset> _ghostDialogFileList;
    List<Dialogue> _ghostDialogList;

    Dialogue _dialog;

    // Use this for initialization
    void Start()
    {
        _ghostDialogList = new List<Dialogue>();
        foreach (TextAsset text in _ghostDialogFileList)
        {
            _ghostDialogList.Add(Dialogue.DialogueFactory(text));
        }
    }

    public void EnterGhostmode()
    {
        _spriteRenderer.sprite = _ghostSprite;
        _dialog = _ghostDialogList[Random.Range(0, _ghostDialogList.Count)];
    }

    public void SetSpriteToUrn()
    {
        _spriteRenderer.sprite = _urnSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        OnInteraction();
    }

    public bool VictoryCheck()
    {
        return _pedestal.VictoryCheck(this);
    }

    public void OnInteraction()
    {
        InteractWithHoldable(this);
    }

    public void SetColor(Color newColor)
    {
        _spriteRenderer.color = newColor;
    }

    public void InteractWithHoldable(Urn holdable)
    {
        if (LevelManager.s_instance.GetStage() == LevelManager.LevelStage.Night)
        {
            TypeWriter.s_instance.PlayDialogue(_dialog);
            return;
        }

        if (_heldObject == null)
        {
            _heldObject = holdable;
            _heldObject.SetColor(Color.yellow);
            return;
        }

        Pedestal savedPedestal = holdable._pedestal;
        holdable._pedestal = _heldObject._pedestal;
        _heldObject._pedestal = savedPedestal;

        Vector3 newPosition = _heldObject.transform.position;
        _heldObject.transform.position = holdable.transform.position;
        holdable.transform.position = newPosition;
        _heldObject.SetColor(Color.white);
        holdable.SetColor(Color.white);
        _heldObject = null;
    }
}
