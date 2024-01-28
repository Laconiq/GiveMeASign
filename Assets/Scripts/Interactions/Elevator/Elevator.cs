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
        if (floor == -1)
            FakeGoToFloor();
        _targetFloor = floor;
        if (currentFloor == _targetFloor)
        {            
            OpenElevator();
            Debug.Log("Elevator is already on floor " + _targetFloor);
        }
        else
            StartCoroutine(MoveElevator());
    }
    
    private void FakeGoToFloor()
    {
        _isMoving = true;
        CloseElevator();
        float delay = 10f;
        Invoke(nameof(OpenElevator), delay);
        Invoke(nameof(PlayDingSound), delay);
    }
    
    private IEnumerator MoveElevator()
    {
        _isMoving = true;
        CloseElevator();
        while (transform.position != floorPositions[_targetFloor])
        {
            transform.position = Vector3.MoveTowards(transform.position, floorPositions[_targetFloor], elevatorSpeed * Time.deltaTime);
            yield return null;
        }
        PlayDingSound();
        _isMoving = false;
        currentFloor = _targetFloor;
        OpenElevator();
    }
    
    private void PlayDingSound()
    {
        elevatorDingSound.Post(gameObject);
    }
    
    private void OpenElevator()
    {
        if (_isDoorOpen)
            return;
        elevatorStopMovingSound.Post(gameObject);
        elevatorOpenDoorSound.Post(gameObject);
        _isDoorOpen = true;
        elevatorAnimator.Play("OpenDoor");
        frontDoorAnimators[currentFloor].Play("OpenDoor");
    }
    
    private void CloseElevator()
    {
        if (!_isDoorOpen)
            return;
        elevatorStartMovingSound.Post(gameObject);
        elevatorCloseDoorSound.Post(gameObject);
        _isDoorOpen = false;
        elevatorAnimator.Play("CloseDoor");
        frontDoorAnimators[currentFloor].Play("CloseDoor");
    }
}
