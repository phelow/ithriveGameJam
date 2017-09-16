using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Pedestal _pedestal;

    private static Urn _heldObject;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        OnInteraction();
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
