using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    private bool _triggered = false;

    [SerializeField]
    private SpriteRenderer _renderer;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            _triggered = true;
            TopDownCharacterController.s_instance.SetTriggeredObject(this);
            _renderer.color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            _triggered = false;
            TopDownCharacterController.s_instance.SetTriggeredObject(null);
            _renderer.color = Color.white;
        }
    }

    public virtual void OnInteraction()
    {
        Debug.Log("Base class interactable called");
    }
}
