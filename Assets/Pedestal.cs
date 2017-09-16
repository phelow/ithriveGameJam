using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour {
    // if all the targeted urns are on all the pedestals win the game.
    [SerializeField]
    private Urn _targetUrn;
    
    public bool VictoryCheck(Urn checkUrn)
    {
        return checkUrn == _targetUrn;
    }
}
