using UnityEngine;

public class GoToNextDialogue : MonoBehaviour
{
    public void OnButtonClick() { FindObjectOfType<DialogueManager>().GoToNextDialogue(); }
}
