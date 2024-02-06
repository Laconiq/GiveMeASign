using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private int floorToGo;
    [SerializeField] private Clock clockToStart;
    
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        if (!canInteract && eventToCheck.GetProgressionStatus() == false)
            return;
        if (clockToStart != null)
            clockToStart.StartClock();
        elevator.GoToFloor(floorToGo);
    }
}
