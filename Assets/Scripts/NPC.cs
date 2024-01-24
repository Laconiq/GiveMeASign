using System.Collections.Generic;
using UnityEngine;

public class Npc : Interactable
{
    [SerializeField] private List<DialogueScriptableObject> dialogueScriptableObject;
    private int _currentDialogueIndex;
    private void Awake()
    {
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        foreach (DialogueScriptableObject dialogue in dialogueScriptableObject)
            dialogue.isDialogueFinished = false;
    }
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _currentDialogueIndex = ResearchDialogue();
        FindObjectOfType<DialogueManager>().StartDialogue(dialogueScriptableObject[_currentDialogueIndex]);
    }
    
    private int ResearchDialogue()
    {
        int tempDialogueIndex = _currentDialogueIndex;
        
        // If the dialogue is not finished, we start it
        if (!dialogueScriptableObject[tempDialogueIndex].isDialogueFinished)
            return tempDialogueIndex;

        // If the dialogue is finished and there is no next dialogue, we repeat the last one
        if (dialogueScriptableObject[tempDialogueIndex].isDialogueFinished && tempDialogueIndex + 1 == dialogueScriptableObject.Count)
            return tempDialogueIndex;

        // If the dialogue is finished and there is a next dialogue without condition, we start it
        if (dialogueScriptableObject[tempDialogueIndex+1].requireProgressionToStart == false)
            return tempDialogueIndex+1;
        
        // If the dialogue is finished and there is a next dialogue with condition, we check if the condition is met
        bool allProgressionsFinished = true;
        foreach(var progression in dialogueScriptableObject[tempDialogueIndex+1].progressionsToStart)
            if(!progression.IsProgressionFinished)
            {
                allProgressionsFinished = false;
                break;
            }
        if (dialogueScriptableObject[tempDialogueIndex+1].requireProgressionToStart && allProgressionsFinished)
            return tempDialogueIndex+1;

        // If the dialogue is finished and there is a next dialogue with condition, we repeat the last one
        return tempDialogueIndex;
    }
}
