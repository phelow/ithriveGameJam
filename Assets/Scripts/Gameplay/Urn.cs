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
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public Color _originalColor;

    public List<AudioClip> _soundSwap;
    private static bool s_urnsLocked = false;


    // Use this for initialization
    private void Awake()
    {
        s_urnsLocked = false;
        _originalColor = spriteRenderer.color;
    }

    private void ClearHeldObject()
    {
        Deselect();
        _heldObject = null;
    }

    private void Select()
    {
        
        spriteRenderer.color = Color.yellow;
        
    }

    private void Deselect()
    {
        spriteRenderer.color = _originalColor;
        
    }

    public void OnMouseDown()
    {
        if (s_urnsLocked)
        {
            return;

        }
        
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
        
        //spriteRenderer.color = newColor;
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

            

            Pedestal savedPedestal = holdable._pedestal;
            holdable._pedestal = _heldObject._pedestal;
            _heldObject._pedestal = savedPedestal;

            holdable.spriteRenderer.color = Color.white;
            _heldObject.spriteRenderer.color = Color.white;
            Vector3 newPosition = _heldObject.transform.position;
            _heldObject.StartCoroutine(_heldObject.MoveUrnToPosition(
                _heldObject.transform.position, 
                Vector3.Lerp(_heldObject.transform.position, holdable.transform.position, .5f), 
                holdable.transform.position));
            _heldObject.StartCoroutine(
                holdable.MoveUrnToPosition(holdable.transform.position,
                Vector3.Lerp(holdable.transform.position, newPosition + Vector3.up * 5.0f, .5f), 
                newPosition));
            SoundManager.instance.PlayRandomSwappingSound();
            _heldObject.ClearHeldObject();
        }
    }

    private IEnumerator MoveUrnToPosition(Vector3 originalPosition, Vector3 midpoint, Vector3 targetPosition)
    {
        s_urnsLocked = true;
        float tPassed = 0.0f;
        float totalLerpTime = 1.0f;
        while (tPassed < totalLerpTime)
        {
            transform.position = 
                Vector3.Slerp(originalPosition, midpoint, tPassed / totalLerpTime);
            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tPassed = 0.0f;
        totalLerpTime = 1.0f;
        while (tPassed < totalLerpTime)
        {
            transform.position =
                Vector3.Slerp(midpoint, targetPosition, tPassed / totalLerpTime);
            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        s_urnsLocked = false;
    }
}
