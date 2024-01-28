using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float elevatorSpeed = 1f;
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private int currentFloor;
    private int _targetFloor;
    [SerializeField] private List<Vector3> floorPositions = new List<Vector3>();
    private bool _isMoving;
    private bool _isDoorOpen;
    
    [SerializeField] private AK.Wwise.Event elevatorDingSound;
    [SerializeField] private AK.Wwise.Event elevatorStartMovingSound;
    [SerializeField] private AK.Wwise.Event elevatorStopMovingSound;
    [SerializeField] private AK.Wwise.Event elevatorOpenDoorSound;
    [SerializeField] private AK.Wwise.Event elevatorCloseDoorSound;
    
    [SerializeField] private List<Animator> frontDoorAnimators;
    
    public void GoToFloor(int floor)
    {
        if (_isMoving)
            return;
        _targetFloor = floor;
        if (currentFloor == _targetFloor)
        {            
            OpenElevator();
            Debug.Log("Elevator is already on floor " + _targetFloor);
        }
        else
            StartCoroutine(MoveElevator());
    }
    
    private IEnumerator MoveElevator()
    {
        elevatorStartMovingSound.Post(gameObject);
        _isMoving = true;
        CloseElevator();
        while (transform.position != floorPositions[_targetFloor])
        {
            transform.position = Vector3.MoveTowards(transform.position, floorPositions[_targetFloor], elevatorSpeed * Time.deltaTime);
            yield return null;
        }
        elevatorStopMovingSound.Post(gameObject);
        elevatorDingSound.Post(gameObject);
        _isMoving = false;
        currentFloor = _targetFloor;
        OpenElevator();
    }
    
    private void OpenElevator()
    {
        if (_isDoorOpen)
            return;
        elevatorOpenDoorSound.Post(gameObject);
        _isDoorOpen = true;
        elevatorAnimator.Play("OpenDoor");
        frontDoorAnimators[_targetFloor].Play("OpenDoor");
    }
    
    private void CloseElevator()
    {
        if (!_isDoorOpen)
            return;
        elevatorCloseDoorSound.Post(gameObject);
        _isDoorOpen = false;
        elevatorAnimator.Play("CloseDoor");
        frontDoorAnimators[currentFloor].Play("CloseDoor");
    }
}
