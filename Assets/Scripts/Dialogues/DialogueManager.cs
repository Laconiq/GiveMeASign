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
    private PlayerCamera _playerCamera;
    
    public void Initialize()
    {
        npcDialogueText.text = "";
        _playerController = GameManager.Instance.playerController;
        _playerCamera = _playerController.GetComponent<PlayerCamera>();
        _textRevealFeedback = textRevealFeedback.GetFeedbackOfType<MMF_TMPTextReveal>();
        dialogueContainer.SetActive(false);
    }

    private void RevealText(string str)
    {
        _textRevealFeedback.NewText = str;
        textRevealFeedback.PlayFeedbacks();
    }
    
    public void SetTextRevealSpeed(float f)
    {
        _textRevealFeedback.IntervalBetweenReveals = f;
    }
    
    public void StartDialogue(Dialogue dialogue, string npcName)
    {
        _currentDialogue = dialogue;
        
        dialogueContainer.SetActive(true);
        npcNameText.text = npcName;
        _totalDialogueItems = _currentDialogue.dialogueItems.Count;
        
        _playerController.DisableControls();
        _playerCamera.SetDialogueCamera();

        if (_currentDialogue.isDialogueFinished)
        {
            _currentDialogueIndex = _currentDialogue.dialogueItems.Count;
            DisplayCurrentDialogue(_currentDialogue.repeatDialogueItem);
        }
        else
        {
            _currentDialogueIndex = 0;
            DisplayCurrentDialogue(_currentDialogue.dialogueItems[0]);
        }
        _playerController.DialogueControls.Enable();
    }

    private void DisplayCurrentDialogue(DialogueItem dialogueItem)
    {
        SetTextRevealSpeed(0.05f);
        _currentDialogueItem = dialogueItem;
        DestroyPlayerResponses();
        _playerCamera.LookAtTarget(_currentDialogueItem.lookAtTarget);
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
        _playerCamera.SetFPSCamera();
        _currentDialogue.isDialogueFinished = true;
        _currentDialogue.UnlockProgression();
        dialogueContainer.SetActive(false);
        _playerController.DialogueControls.Disable();
    }
    
    private void DestroyPlayerResponses()
    {
        foreach (Transform child in playerResponsesParent.transform)
            Destroy(child.gameObject);
    }
}
