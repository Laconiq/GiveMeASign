using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class Npc : Interactable
{
    [SerializeField] private string npcName;
    private List<Dialogue> _dialogues;
    private int _currentDialogueIndex;
    private DialogueManager _dialogueManager;
    private void Start()
    {
        _dialogueManager = GameManager.Instance.dialogueManager;
        _dialogues = new List<Dialogue>();
        foreach (Transform child in transform)
        {
            Dialogue dialogue = child.GetComponent<Dialogue>();
            if (dialogue != null)
                _dialogues.Add(dialogue);
        }
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        foreach (Dialogue dialogue in _dialogues)
            dialogue.isDialogueFinished = false;
    }
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _currentDialogueIndex = ResearchDialogue();
        _dialogueManager.StartDialogue(_dialogues[_currentDialogueIndex], npcName);
    }
    
    private int ResearchDialogue()
    {
        int tempDialogueIndex = _currentDialogueIndex;
        
        // If the dialogue is not finished, we start it
        if (!_dialogues[tempDialogueIndex].isDialogueFinished)
            return tempDialogueIndex;

        // If the dialogue is finished and there is no next dialogue, we repeat the last one
        if (_dialogues[tempDialogueIndex].isDialogueFinished && tempDialogueIndex + 1 == _dialogues.Count)
            return tempDialogueIndex;

        // If the dialogue is finished and there is a next dialogue without condition, we start it
        if (_dialogues[tempDialogueIndex+1].requireProgressionToStart == false)
            return tempDialogueIndex+1;
        
        // If the dialogue is finished and there is a next dialogue with condition, we check if the condition is met
        bool allProgressionsFinished = _dialogues[tempDialogueIndex + 1].progressionsToStart.All(progression => progression.GetProgressionStatus());
        if (_dialogues[tempDialogueIndex+1].requireProgressionToStart && allProgressionsFinished)
            return tempDialogueIndex+1;

        // If the dialogue is finished and there is a next dialogue with condition, we repeat the last one
        return tempDialogueIndex;
    }
    
#if UNITY_EDITOR
    [Button("Add Dialogue")]
    private void AddDialogueEditor()
    {
        // Charge le prefab depuis le dossier Resources
        GameObject dialoguePrefab = Resources.Load<GameObject>("Dialogue");
        if (dialoguePrefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(dialoguePrefab, transform);
            instance.name = "Dialogue";
            Undo.RegisterCreatedObjectUndo(instance, "Create progression instance");
        }
        else
            Debug.LogWarning("Dialogue prefab not found in Resources.");
    }
#endif
}
