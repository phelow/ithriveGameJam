﻿using UnityEngine;
using System.Collections;

public class DialogueManager {

    private TypeWriter dialogueUI;

	public DialogueManager(GameObject dialogueUI) {
        this.dialogueUI = dialogueUI.GetComponent<TypeWriter>();
    }


    public bool PlayDialogue (Dialogue d) {
        return dialogueUI.PlayDialogue(d);
    }

}
