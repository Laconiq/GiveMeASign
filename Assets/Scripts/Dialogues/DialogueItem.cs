using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueItem
{
    [Multiline] public string npcDialogue;
    public List<string> playerResponses;
    public Transform lookAtTarget;

    public DialogueItem(string npcDialogue, List<string> playerResponses, Transform lookAtTarget)
    {
        this.npcDialogue = npcDialogue;
        this.playerResponses = playerResponses;
        this.lookAtTarget = lookAtTarget;
    }
}