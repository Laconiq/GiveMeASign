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
    private Dialogue _currentDialogue;
    private DialogueItem _currentDialogueItem;
    private int _currentDialogueIndex;
    private int _totalDialogueItems;
    private PlayerController _playerController;
    
    public void Initialize()
    {
        npcDialogueText.text = "";
        _playerController = GameManager.Instance.playerController;
        _textRevealFeedback = textRevealFeedback.GetFeedbackOfType<MMF_TMPTextReveal>();
        dialogueContainer.SetActive(false);
    }

    private void RevealText(string str)
    {
        _textRevealFeedback.NewText = str;
        textRevealFeedback.PlayFeedbacks();
    }
    
    public void StartDialogue(Dialogue dialogue, string npcName)
    {
        dialogueContainer.SetActive(true);
        npcNameText.text = npcName;
        _currentDialogue = dialogue;
        LoadCurrentDialogueIndex();
        _totalDialogueItems = _currentDialogue.dialogueItems.Count;
        
        _playerController.DisableControls();
        _playerController.SetDialogueCamera();
        
        DisplayCurrentDialogue(_currentDialogue.isDialogueFinished ? _currentDialogue.repeatDialogueItem : _currentDialogue.dialogueItems[_currentDialogueIndex]);
    }

    private void LoadCurrentDialogueIndex()
    {
        if (_currentDialogue.isDialogueFinished)
            _currentDialogueIndex = _totalDialogueItems - 1;
        else
            _currentDialogueIndex = 0;
    }

    private void DisplayCurrentDialogue(DialogueItem dialogueItem)
    {
        _currentDialogueItem = dialogueItem;
        DestroyPlayerResponses();
        _playerController.LookAtTarget(_currentDialogueItem.lookAtTarget);
        RevealText(_currentDialogueItem.npcDialogue);
    }

    public void DisplayPlayerResponses()
    {
        foreach (string playerResponse in _currentDialogueItem.playerResponses)
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
            DisplayCurrentDialogue(_currentDialogue.dialogueItems[_currentDialogueIndex]);
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
        _currentDialogue.isDialogueFinished = true;
        _currentDialogue.UnlockProgression();
        dialogueContainer.SetActive(false);
    }
    
    private void DestroyPlayerResponses()
    {
        foreach (Transform child in playerResponsesParent.transform)
            Destroy(child.gameObject);
    }
}
