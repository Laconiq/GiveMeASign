using Sirenix.OdinInspector;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private bool doorIsOpen;
    [SerializeField] protected Animator animator;
    [SerializeField] private float delayBeforeTriggeringAnimation;
    [SerializeField] private bool canInteract = true;
    [SerializeField, HideIf("canInteract")] private Progression progressionToCheck;

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();

        
        if (!canInteract && progressionToCheck.GetProgressionStatus() == false)
            Invoke(nameof(DoorIsLocked), delayBeforeTriggeringAnimation);
        else
            Invoke(doorIsOpen ? nameof(CloseDoor) : nameof(OpenDoor), delayBeforeTriggeringAnimation);
    }
    
    private void OpenDoor()
    {
        animator.SetBool("OpenDoor", true);
        doorIsOpen = true;
    }
    
    private void CloseDoor()
    {
        animator.SetBool("OpenDoor", false);
        doorIsOpen = false;
    }
    
    private void DoorIsLocked()
    {
        animator.SetTrigger("DoorIsLocked");
    }
}
