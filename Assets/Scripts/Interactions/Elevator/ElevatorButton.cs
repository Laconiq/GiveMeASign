using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private int floorToGo;
    [SerializeField] private Animator animator;
    
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        elevator.GoToFloor(floorToGo);
        animator.Play("PressButton");
    }
}
