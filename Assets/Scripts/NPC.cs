using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] private DialogueScriptableObject dialogueScriptableObject;

    private void Awake()
    {
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        dialogueScriptableObject.isDialogueFinished = false;
    }
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueScriptableObject);
    }
}
