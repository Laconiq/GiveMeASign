using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Title("Data")]
    [SerializeField] private TMP_Text npcDialogueText;
    [SerializeField] private GameObject playerResponsesParent;
    [SerializeField] private GameObject playerResponsePrefab;
    private DialogueScriptableObject _currentDialogueScriptableObject;
    private int _currentDialogueIndex;
    private int _totalDialogueItems;
    private PlayerController _playerController;
    
    private void Start()
    {
        npcDialogueText.text = "";
        _playerController = FindObjectOfType<PlayerController>();
    }
    
    public void StartDialogue(DialogueScriptableObject dialogueScriptableObject)
    {
        _currentDialogueScriptableObject = dialogueScriptableObject;
        LoadCurrentDialogueIndex();
        _totalDialogueItems = _currentDialogueScriptableObject.dialogueItems.Count;
        DisplayCurrentDialogue();
        _playerController.DisableControls();
    }

    private void LoadCurrentDialogueIndex()
    {
        if (_currentDialogueScriptableObject.isDialogueFinished)
            _currentDialogueIndex = _totalDialogueItems - 1;
        else
            _currentDialogueIndex = 0;
    }

    private void DisplayCurrentDialogue()
    {
        DestroyPlayerResponses();
        DialogueItem currentDialogueItem = _currentDialogueScriptableObject.dialogueItems[_currentDialogueIndex];
        npcDialogueText.text = currentDialogueItem.npcDialogue;
        foreach (string playerResponse in currentDialogueItem.playerResponses)
        {
            GameObject playerResponseGameObject = Instantiate(playerResponsePrefab, playerResponsesParent.transform);
            playerResponseGameObject.GetComponentInChildren<TMP_Text>().text = playerResponse;
        }
    }
    
    public void GoToNextDialogue()
    {
        if (_currentDialogueIndex < _totalDialogueItems - 1)
        {
            _currentDialogueIndex++;
            DisplayCurrentDialogue();
        }
        else
            CloseDialogue();
    }

    private void CloseDialogue()
    {
        npcDialogueText.text = "";
        DestroyPlayerResponses();
        _playerController.EnableControls();
        _currentDialogueScriptableObject.isDialogueFinished = true;
    }
    
    private void DestroyPlayerResponses()
    {
        foreach (Transform child in playerResponsesParent.transform)
            Destroy(child.gameObject);
    }
}
