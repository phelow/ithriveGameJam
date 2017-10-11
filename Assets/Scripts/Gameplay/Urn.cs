using System;
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

    private float urnMoveTime = .8f;
    
    private Vector3 originalScale;

    private SpriteRenderer ghostSprite;
    private Character ghost;
    public Vector3 ghostPosition;
    public Vector3 ghostScale;
    public Quaternion ghostRotation;

    // Use this for initialization
    private void Awake()
    {
        s_urnsLocked = false;
        _originalColor = spriteRenderer.color;
        originalScale = spriteRenderer.transform.localScale;
        ghostSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        ghost = gameObject.GetComponentInChildren<Character>();
        ghostPosition = ghostSprite.transform.localPosition;
        ghostScale = ghostSprite.transform.localScale;
        ghostRotation = ghostSprite.transform.localRotation;
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

    private void OnMouseEnter() {
        if(s_urnsLocked || LevelManager.s_instance.GetStage() != LevelManager.LevelStage.Morning)
        {
            return;
        }
        spriteRenderer.transform.localScale = Vector3.Scale(new Vector3(1.25f, 1.25f), spriteRenderer.transform.localScale);
    }
    private void OnMouseExit() {
        if (s_urnsLocked || LevelManager.s_instance.GetStage() != LevelManager.LevelStage.Morning)
        {
            return;
        }
        spriteRenderer.transform.localScale = originalScale;
    }

    public bool VictoryCheck()
    {
        return _pedestal.VictoryCheck(this);
    }

    public void OnInteraction()
    {
        InteractWithHoldable(this);
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

            ghostPosition = ghostSprite.transform.localPosition;
            ghostRotation = ghostSprite.transform.localRotation;
            ghostScale = ghostSprite.transform.localScale;

            _heldObject.ghostPosition = _heldObject.ghostSprite.transform.localPosition;
            _heldObject.ghostRotation = _heldObject.ghostSprite.transform.localRotation;
            _heldObject.ghostScale = _heldObject.ghostSprite.transform.localScale;


            Pedestal savedPedestal = holdable._pedestal;
            holdable._pedestal = _heldObject._pedestal;
            _heldObject._pedestal = savedPedestal;

            holdable.spriteRenderer.color = Color.white;
            holdable.spriteRenderer.transform.localScale = originalScale;
            _heldObject.spriteRenderer.color = Color.white;
            Vector3 newPosition = _heldObject.transform.position;
            _heldObject.StartCoroutine(_heldObject.MoveUrnToPosition(
                _heldObject.transform.position, 
                Vector3.Lerp(_heldObject.transform.position, holdable.transform.position, .5f), 
                holdable.transform.position, holdable));
            _heldObject.StartCoroutine(
                holdable.MoveUrnToPosition(holdable.transform.position,
                Vector3.Lerp(holdable.transform.position, newPosition + Vector3.up * 5.0f, .5f), 
                newPosition, _heldObject));

            ghost.Fade(0f, .2f);
            _heldObject.ghost.Fade(0f, .2f);

            Global.soundManager.PlayRandomSwappingSound();
            _heldObject.ClearHeldObject();

            
        }
    }

    private void FixGhostTransforms(Urn holdable) {
        var tran = holdable.transform;
        
    }

    private IEnumerator MoveUrnToPosition(Vector3 originalPosition, Vector3 midpoint, Vector3 targetPosition, Urn urn)
    {
        s_urnsLocked = true;
        float tPassed = 0.0f;
        float totalLerpTime = urnMoveTime;

       

        while (tPassed < totalLerpTime)
        {
            transform.position = 
                Vector3.Slerp(originalPosition, midpoint, tPassed / totalLerpTime);

            

            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tPassed = 0.0f;
        totalLerpTime = urnMoveTime;
        while (tPassed < totalLerpTime)
        {
            transform.position =
                Vector3.Slerp(midpoint, targetPosition, tPassed / totalLerpTime);

            
            tPassed += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        ghostSprite.transform.localPosition = urn.ghostPosition;
        ghostSprite.transform.localRotation = urn.ghostRotation;
        ghostSprite.transform.localScale = urn.ghostScale;

        urn.ghostSprite.transform.localPosition = ghostPosition;
        urn.ghostSprite.transform.localScale = ghostScale;
        urn.ghostSprite.transform.localRotation = ghostRotation;

        s_urnsLocked = false;
        ghost.Fade(LevelManager.ghostFadeAlpha, .2f);
        urn.ghost.Fade(LevelManager.ghostFadeAlpha, .2f);
    }
}
