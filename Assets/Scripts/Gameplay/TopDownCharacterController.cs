using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour {
    public static TopDownCharacterController s_instance;

    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _axisMinimum = 0.01f;

    [SerializeField]
    private float _movementMultiplier = 100.0f;

    private Holdable _heldObject;
    private Interactable _triggeredObject;

    private bool _shouldInteract;

    void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(MovePlayer());
        StartCoroutine(WaitForInteraction());
	}

    public void SetTriggeredObject(Interactable triggeredObject)
    {
        _triggeredObject = triggeredObject;
    }

    public void PickupObject(Holdable holdable)
    {
        _heldObject = holdable;
        holdable.transform.parent = this.transform;
    }

    private void DropHeldObject()
    {
        _heldObject.transform.parent = null;
        _heldObject = null;
    }

    private IEnumerator WaitForInteraction()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if(!_shouldInteract)
            {
                continue;
            }

            _shouldInteract = false;
            if(_triggeredObject == null && _heldObject == null)
            {
                continue;
            }

            if(_triggeredObject != null)
            {
                _triggeredObject.OnInteraction();
                continue;
            }

            if(_heldObject != null)
            {
                DropHeldObject();
            }

        }
    }

    private IEnumerator MovePlayer()
    {
        while (true)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector2 movement = Vector2.zero;
            
            if ( Mathf.Abs(vertical) > _axisMinimum || Mathf.Abs(horizontal) > _axisMinimum)
            {
                movement = new Vector2(horizontal, vertical);
            }
            else
            {
                movement = (new Vector2(worldPos.x, worldPos.y) - new Vector2(transform.position.x, transform.position.y));
            }
                     
            if (movement.magnitude > .03f)
            {
                _rigidbody.AddForce(movement.normalized * _movementMultiplier * Time.deltaTime);
            }
            
            yield return new WaitForEndOfFrame();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            _shouldInteract = true;
        }
	}
}
