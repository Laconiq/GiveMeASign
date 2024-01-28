using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private int floorToGo;
    [SerializeField] private AK.Wwise.Event elevatorButtonSound;
    
    public override void OnPlayerInteract()
    {
        elevator.GoToFloor(floorToGo);
        elevatorButtonSound.Post(gameObject);
        base.OnPlayerInteract();
    }
}
