using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TEST_STUFF : MonoBehaviour {

    private void Awake () {

        List<Sentence> sl = new List<Sentence>();

        Dialogue d = new Dialogue(sl);

        Global.dialogueManager.PlayDialogue(d);
    }

}
