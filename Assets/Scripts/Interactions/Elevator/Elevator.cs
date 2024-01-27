using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float elevatorSpeed = 1f;
    [SerializeField] private Animator elevatorAnimator;
    [SerializeField] private int currentFloor;
    private int _targetFloor;
    private List<Vector3> _floorPositions = new List<Vector3>();
    private bool _isMoving;
    private bool _isDoorOpen;
    
    [SerializeField] private List<Animator> frontDoorAnimators;
    
    public void GoToFloor(int floor)
    {
        if (_isMoving)
            return;
        _targetFloor = floor;
        _isMoving = true;

        if (currentFloor == _targetFloor)
            OpenElevator();
        else
            StartCoroutine(MoveElevator());
    }
    
    private IEnumerator MoveElevator()
    {
        CloseElevator();
        while (transform.position != _floorPositions[_targetFloor])
        {
            transform.position = Vector3.MoveTowards(transform.position, _floorPositions[_targetFloor], elevatorSpeed * Time.deltaTime);
            yield return null;
        }
        _isMoving = false;
        currentFloor = _targetFloor;
        OpenElevator();
    }
    
    private void OpenElevator()
    {
        if (_isDoorOpen)
            return;
        _isDoorOpen = true;
        elevatorAnimator.Play("OpenDoor");
        frontDoorAnimators[_targetFloor].Play("OpenDoor");
    }
    
    private void CloseElevator()
    {
        _isDoorOpen = false;
        elevatorAnimator.Play("CloseDoor");
        frontDoorAnimators[currentFloor].Play("CloseDoor");
    }
    
}
