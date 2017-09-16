using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _axisMinimum = 0.01f;

    [SerializeField]
    private float _movementMultiplier = 100.0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(MovePlayer());
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

            Debug.Log(movement);

            yield return new WaitForEndOfFrame();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
