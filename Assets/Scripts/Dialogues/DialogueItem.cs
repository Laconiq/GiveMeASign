using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueItem
{
    [Multiline] public string npcDialogue;
    public List<string> playerResponses;

    public DialogueItem(string npcDialogue, List<string> playerResponses)
    {
        this.npcDialogue = npcDialogue;
        this.playerResponses = playerResponses;
    }
}