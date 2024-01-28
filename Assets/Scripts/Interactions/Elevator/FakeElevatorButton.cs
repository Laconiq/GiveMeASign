using UnityEngine;

public class FakeElevatorButton : InteractableObject
{
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private Animator frontDoorAnimator;
    [SerializeField] private bool isFrontButton;
    
    [SerializeField] private AK.Wwise.Event openDoorSound;
    [SerializeField] private AK.Wwise.Event closeDoorSound;
    [SerializeField] private AK.Wwise.Event elevatorDingSound;
    
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
        openDoorSound.Post(gameObject);
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
        closeDoorSound.Post(gameObject);
        elevatorAnimator.SetTrigger("CloseDoor");
        frontDoorAnimator.SetTrigger("CloseDoor");
        float delay = 10f;
        Invoke(nameof(OpenDoor), delay);
        Invoke(nameof(PlayerSoundDing), delay);
    }
    
    private void PlayerSoundDing()
    {
        elevatorDingSound.Post(gameObject);
    }
}
