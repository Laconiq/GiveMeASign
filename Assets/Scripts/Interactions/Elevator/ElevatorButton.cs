using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private int floorToGo;
    
    public override void OnPlayerInteract()
    {
        elevator.GoToFloor(floorToGo);
        base.OnPlayerInteract();
    }
}
