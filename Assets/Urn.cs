using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    [SerializeField]
    private GameObject _character;

    [SerializeField]
    private Pedestal _pedestal;

    //[SerializeField]
    //private Sprite _ghostSprite;

    //[SerializeField]
    //private Sprite _urnSprite;

    private static Urn _heldObject;
    

    

    // Use this for initialization
  

    private void ClearHeldObject()
    {
        if (_heldObject != null)
        {
            _heldObject.SetColor(Color.white);
            _heldObject = null;
        }
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
}
