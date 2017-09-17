using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _character;

    [SerializeField]
    public Pedestal _pedestal;

    //[SerializeField]
    //private Sprite _ghostSprite;

    //[SerializeField]
    //private Sprite _urnSprite;

    private static Urn _heldObject;
    private Material _material;

    public Color _originalColor;

    // Use this for initialization
    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void ClearHeldObject()
    {
        Deselect();
        _heldObject = null;
    }

    private void Select()
    {
        
        _originalColor = _material.color;
        _material.color = Color.yellow;
        
    }

    private void Deselect()
    {
        GetComponent<Renderer>().material.color = _originalColor;
        
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
        
        //_spriteRenderer.color = newColor;
    }

    

    public void InteractWithHoldable(Urn holdable)
    {

        if (LevelManager.s_instance.GetStage() == LevelManager.LevelStage.Morning)
        {

            if (_heldObject == null)
            {
                _heldObject = holdable;

                Select();
                return;
            }

            if (_heldObject == this)
            {
                _heldObject.ClearHeldObject();
                return;
            }

            _heldObject.ClearHeldObject();

            Pedestal savedPedestal = holdable._pedestal;
            holdable._pedestal = _heldObject._pedestal;
            _heldObject._pedestal = savedPedestal;

            Vector3 newPosition = _heldObject.transform.position;
            _heldObject.transform.position = holdable.transform.position;
            holdable.transform.position = newPosition;
        }
    }
}
