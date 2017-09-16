using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TEST_STUFF : MonoBehaviour {

    private void Awake () {

        List<Sentence> sl = new List<Sentence>();
        sl.Add(new Sentence("This is a test dialogue, which serves no purpose other than actually trying if the dialogue manager works!", .06f, 4));
        sl.Add(new Sentence("Hopefully this works as intended and I can move on to something else!", .06f, 4));

        Dialogue d = new Dialogue(sl);

        Global.dialogueManager.PlayDialogue(d);
    }

}
