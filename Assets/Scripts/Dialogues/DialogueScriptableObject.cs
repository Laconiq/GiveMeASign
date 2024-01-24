using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    public Transform lookAtTarget;
    
    public List<DialogueItem> dialogueItems;
    [HideInInspector] public bool isDialogueFinished;
    
    [Title("Progression Settings")]
    public bool unlockProgressionOnDialogueFinish;
    [ShowIf("unlockProgressionOnDialogueFinish")] public List<ProgressionScriptableObject> progressionsToUnlock;
    public bool requireProgressionToStart;
    [ShowIf("requireProgressionToStart")] public List<ProgressionScriptableObject> progressionsToStart;
    
    public void UnlockProgression()
    {
        if (!unlockProgressionOnDialogueFinish)
            return;
        foreach (ProgressionScriptableObject progression in progressionsToUnlock)
            progression.IsProgressionFinished = true;
    }
}