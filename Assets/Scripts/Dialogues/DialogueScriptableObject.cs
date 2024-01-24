using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    public List<DialogueItem> dialogueItems;
    public bool isDialogueFinished;
}