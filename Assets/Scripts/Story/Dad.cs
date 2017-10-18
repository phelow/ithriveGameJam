using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Dad : Character {
    public float SecondsToDisappear;

    void Start()
    {
        ShowCharacter();
        Talk();
        StartCoroutine(Delayed());
    }

    IEnumerator Delayed() {
        yield return new WaitForSeconds(SecondsToDisappear);
        Destroy(gameObject);
    }

}
