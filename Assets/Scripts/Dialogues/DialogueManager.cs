using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Title("Data")]
    [SerializeField] private TMP_Text npcDialogueText;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private GameObject playerResponsesParent;
    [SerializeField] private GameObject playerResponsePrefab;
    [SerializeField] private MMF_Player textRevealFeedback;
    [SerializeField] private GameObject dialogueContainer;
    private MMF_TMPTextReveal _textRevealFeedback;
    private DialogueScriptableObject _currentDialogueScriptableObject;
    private int _currentDialogueIndex;
    private int _totalDialogueItems;
    private PlayerController _playerController;
    
    private void Start()
    {
        npcDialogueText.text = "";
        _playerController = FindObjectOfType<PlayerController>();
        _textRevealFeedback = textRevealFeedback.GetFeedbackOfType<MMF_TMPTextReveal>();
        dialogueContainer.SetActive(false);
    }

    private void RevealText(string str)
    {
        _textRevealFeedback.NewText = str;
        textRevealFeedback.PlayFeedbacks();
    }
    
    public void StartDialogue(DialogueScriptableObject dialogueScriptableObject, string npcName)
    {
        dialogueContainer.SetActive(true);
        npcNameText.text = npcName;
        _currentDialogueScriptableObject = dialogueScriptableObject;
        LoadCurrentDialogueIndex();
        _totalDialogueItems = _currentDialogueScriptableObject.dialogueItems.Count;
        
        _playerController.DisableControls();
        _playerController.SetDialogueCamera();
        if (_currentDialogueScriptableObject.lookAtTarget != null)
            _playerController.LookAtTarget(_currentDialogueScriptableObject.lookAtTarget);
        
        DialogueItem dialogueItemToLoad = _currentDialogueScriptableObject.isDialogueFinished ? _currentDialogueScriptableObject.repeatDialogueItem : _currentDialogueScriptableObject.dialogueItems[_currentDialogueIndex];
        DisplayCurrentDialogue(dialogueItemToLoad);
    }

    private void LoadCurrentDialogueIndex()
    {
        if (_currentDialogueScriptableObject.isDialogueFinished)
            _currentDialogueIndex = _totalDialogueItems - 1;
        else
            _currentDialogueIndex = 0;
    }

    private void DisplayCurrentDialogue(DialogueItem dialogueItem)
    {
        DestroyPlayerResponses();
        RevealText(dialogueItem.npcDialogue);
        foreach (string playerResponse in dialogueItem.playerResponses)
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
            DisplayCurrentDialogue(_currentDialogueScriptableObject.dialogueItems[_currentDialogueIndex]);
        }
        else
            CloseDialogue();
    }

    private void CloseDialogue()
    {
        npcDialogueText.text = "";
        DestroyPlayerResponses();
        _playerController.EnableControls();
        _playerController.SetFPSCamera();
        _currentDialogueScriptableObject.isDialogueFinished = true;
        _currentDialogueScriptableObject.UnlockProgression();
        dialogueContainer.SetActive(false);
    }
    
    private void DestroyPlayerResponses()
    {
        foreach (Transform child in playerResponsesParent.transform)
            Destroy(child.gameObject);
    }
}
