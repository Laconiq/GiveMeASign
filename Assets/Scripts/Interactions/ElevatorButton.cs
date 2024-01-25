using System.Collections;
using UnityEngine;

public class ElevatorButton : InteractableObject
{
    [SerializeField] private GameObject elevator;
    [SerializeField] private float elevatorSpeed;
    [SerializeField] private Transform elevatorDestination;
    [SerializeField] private string openDoorTriggerName;
    [SerializeField] private string closeDoorTriggerName;
    
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        StartCoroutine(MoveElevator());
    }
    
    private IEnumerator MoveElevator()
    {
        OpenElevatorDoor(false);
        while (elevator.transform.position != elevatorDestination.position)
        {
            elevator.transform.position = Vector3.MoveTowards(elevator.transform.position, elevatorDestination.position, elevatorSpeed * Time.deltaTime);
            yield return null;
        }
        OpenElevatorDoor(true);
    }
    
    private void OpenElevatorDoor(bool b)
    {
        Animator elevatorAnimator = elevator.GetComponent<Animator>();
        elevatorAnimator.SetTrigger(b ? openDoorTriggerName : closeDoorTriggerName);
    }
}
