using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public List<DialogueItem> dialogueItems;
    [HideInInspector] public bool isDialogueFinished;
    public DialogueItem repeatDialogueItem;
    
    [Title("Progression Settings")]
    public bool unlockProgressionOnDialogueFinish;
    [ShowIf("unlockProgressionOnDialogueFinish")] public List<Progression> progressionsToUnlock;
    public bool requireProgressionToStart;
    [ShowIf("requireProgressionToStart")] public List<Progression> progressionsToStart;
    
    public void UnlockProgression()
    {
        if (!unlockProgressionOnDialogueFinish)
            return;
        foreach (Progression progression in progressionsToUnlock)
            progression.SetProgressionStatus(true);
    }
}
