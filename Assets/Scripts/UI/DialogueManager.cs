using UnityEngine;
using System.Collections;

public class DialogueManager:MonoBehaviour {

    private TypeWriter dialogueUI;

    private void Awake() {
        if(Global.dialogueManager != this)
        {
            Destroy(gameObject);
        }
        this.dialogueUI = GetComponent<TypeWriter>();
    }
    

    public bool PlayDialogue (Dialogue d) {
        return dialogueUI.PlayDialogue(d);
    }

}
