using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeElevatorButton : InteractableObject
{
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private Animator frontDoorAnimator;
    [SerializeField] private bool isFrontButton;
    
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        if (isFrontButton)
            OpenDoor();
        else
            CloseDore();
    }   
    private void OpenDoor()
    {
        elevatorAnimator.SetTrigger("OpenDoor");
        frontDoorAnimator.SetTrigger("OpenDoor");
    }

    private void CloseDore()
    {
        if (elevatorAnimator.GetCurrentAnimatorStateInfo(0).IsName("CloseDoor"))
        {
            Debug.Log("Door is already closed");
            return;
        }
        elevatorAnimator.SetTrigger("CloseDoor");
        frontDoorAnimator.SetTrigger("CloseDoor");
        Invoke(nameof(OpenDoor), 10f);
    }
}
