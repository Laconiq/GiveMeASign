using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Title("Movement Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float standHeight = 2.0f;

    public static PlayerController Instance;
    private CharacterController _controller;
    private PlayerFeedbacks _feedbacks;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private Transform _cameraTransform;
    private HandleCursor _handleCursor;
    private CinemachineBasicMultiChannelPerlin _noise;
    
    [Title("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCameraDialogue;

    private Controls _controls;
    private Controls _uiControls;
    private Vector2 _moveInput;
    private bool _jumpInput;
    private bool _isCrouching;
    private float _currentSpeed;

    public void Initialize()
    {
        _feedbacks = GetComponent<PlayerFeedbacks>();
        _controller = GetComponent<CharacterController>();
        _handleCursor = FindObjectOfType<HandleCursor>();
        _noise = cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _currentSpeed = movementSpeed;

        _controls = new Controls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Player.Jump.performed += _ => _jumpInput = true;
        _controls.Player.Jump.canceled += _ => _jumpInput = false;
        _controls.Player.Crouch.performed += _ => TryCrouch();
        _controls.Player.Interact.performed += _ => TryToInteract();
        _controls.Player.HoldObject.performed += _ => HoldObject(throwForce);
        _controls.Player.DropObject.performed += _ => HoldObject(dropForce);
    
        _uiControls = new Controls();
        _uiControls.UI.Pause.performed += _ => FindObjectOfType<PauseCanvas>().SwitchPauseCanvas();
        _uiControls.UI.Enable();
    
        EnableControls();
        SetFPSCamera();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
        HandleJumpAndGravity();
        CheckIfObjectIsGrabbable();
    }
    private void TryCrouch()
    {
        if (_isCrouching)
        {
            if (IsBlockedAbove())
                return;
            StartCoroutine(ChangeCrouchState(standHeight, movementSpeed, false));
        }
        else
            StartCoroutine(ChangeCrouchState(crouchHeight, crouchSpeed, true));
    }

    private IEnumerator ChangeCrouchState(float targetHeight, float targetSpeed, bool isCrouching)
    {
        float timeToCrouch = 0.5f;
        float currentTime = 0;
        float startHeight = _controller.height;
        float currentSpeed = _currentSpeed;

        while (currentTime < timeToCrouch)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / timeToCrouch;
            _controller.height = Mathf.SmoothStep(startHeight, targetHeight, t);
            _currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, t);
            yield return null;
        }

        _controller.height = targetHeight;
        _currentSpeed = targetSpeed;
        _isCrouching = isCrouching;

        if (_isCrouching)
            _feedbacks.PlayCrouchFeedbackOn();
        else
            _feedbacks.PlayCrouchFeedbackOff();
    }
    
    private bool IsBlockedAbove()
    {
        bool hitSomething = Physics.Raycast(_controller.transform.position + _controller.center, Vector3.up, out _, standHeight - crouchHeight);
        return hitSomething;
    }

    private bool _isOnLadder;
    public void SetIsOnLadder(bool b) { _isOnLadder = b; }
    public bool GetIsOnLadder() { return _isOnLadder; }
    
    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector3 moveDirection;

        if (_isOnLadder)
        {
            moveDirection = Vector3.up * _moveInput.y;
            _controller.Move(moveDirection * (_currentSpeed * Time.deltaTime));
        }
        else
        {
            Vector3 forward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            moveDirection = forward * _moveInput.y + right * _moveInput.x;
            _controller.Move(moveDirection * (_currentSpeed * Time.deltaTime));

            if (_moveInput != Vector2.zero)
                _noise.m_FrequencyGain = _isCrouching ? 0.03f : 0.05f;
            else
                _noise.m_FrequencyGain = 0.005f;
        }
    }

    private void HandleJumpAndGravity()
    {
        if (_isOnLadder)
            return;
        if (_jumpInput && _isGrounded)
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    
    private List<Interactable> _nearbyInteractables = new List<Interactable>();
    public void AddNearbyInteractable(Interactable interactable)
    {
        if (!_nearbyInteractables.Contains(interactable))
            _nearbyInteractables.Add(interactable);
    }
    public void RemoveNearbyInteractable(Interactable interactable)
    {
        if (_nearbyInteractables.Contains(interactable))
            _nearbyInteractables.Remove(interactable);
    }
    
    private Interactable GetNearestInteractable()
    {
        if (_nearbyInteractables.Count == 0)
            return null;
        Interactable nearestInteractable = _nearbyInteractables[0];
        float nearestDistance = Vector3.Distance(transform.position, nearestInteractable.transform.position);
        foreach (Interactable interactable in _nearbyInteractables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestInteractable = interactable;
            }
        }
        return nearestInteractable;
    }
    
    private void TryToInteract()
    {
        var nearestInteractable = GetNearestInteractable();
        if (nearestInteractable != null)
            nearestInteractable.OnPlayerInteract();
    }
    
    //Sensitivity
    [HideInInspector] public float sensitivity = 300f;
    public void DisableControls()
    {
        var povComponent = cineMachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        povComponent.m_HorizontalAxis.m_MaxSpeed = 0;
        povComponent.m_VerticalAxis.m_MaxSpeed = 0;
        _controls.Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void EnableControls()
    {
        var povComponent = cineMachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        povComponent.m_HorizontalAxis.m_MaxSpeed = sensitivity;
        povComponent.m_VerticalAxis.m_MaxSpeed = sensitivity;
        _controls.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Camera
    public void SetDialogueCamera()
    {
        cineMachineVirtualCamera.Priority = 9;
        cineMachineVirtualCameraDialogue.Priority = 11;
    }
    
    public void SetFPSCamera()
    {
        cineMachineVirtualCamera.Priority = 11;
        cineMachineVirtualCameraDialogue.Priority = 9;
        cineMachineVirtualCameraDialogue.LookAt = null;
    }
    
    public void LookAtTarget(Transform target)
    {
        cineMachineVirtualCameraDialogue.LookAt = target;
    }
    
    //Physics Objects
    [Title("Physics Objects")]
    [SerializeField] private Transform holdPosition;
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private float dropForce = 2.0f;
    private GameObject _heldObject;
    private Rigidbody _heldObjectRb;

    private void HoldObject(float force)
    {
        if (_heldObject == null)
        {
            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, 2f)) 
                return;
            if (hit.collider.GetComponent<GrabbableObject>() == null || !hit.collider.GetComponent<GrabbableObject>().isGrabbable) 
                return;
            _heldObject = hit.collider.gameObject;
            _heldObjectRb = _heldObject.GetComponent<Rigidbody>();
            _heldObjectRb.useGravity = false;
            _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(true);
            StartCoroutine(UpdateHoldPositionRoutine());
            
        }
        else
        {
            _heldObjectRb.useGravity = true;
            _heldObjectRb.AddForce(_cameraTransform.forward * force, ForceMode.VelocityChange);
            _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(false);
            StopCoroutine(UpdateHoldPositionRoutine());
            _heldObject = null;
            _heldObjectRb = null;
        }
    }

    private IEnumerator UpdateHoldPositionRoutine()
    {
        while (_heldObject is not null && _heldObjectRb is not null)
        {
            Vector3 desiredVelocity = (holdPosition.position - _heldObject.transform.position) * 8f;
            _heldObjectRb.velocity = Vector3.Lerp(_heldObjectRb.velocity, desiredVelocity, 0.1f);
            yield return new WaitForFixedUpdate();
        }
    }


    private void CheckIfObjectIsGrabbable()
    {
        if (_heldObject != null)
            _handleCursor.SetCursorVisibility(true);
        else if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, 2f) || 
                 hit.collider.GetComponent<GrabbableObject>() is null || 
                 !hit.collider.GetComponent<GrabbableObject>().isGrabbable)
            _handleCursor.SetCursorVisibility(false);
        else
            _handleCursor.SetCursorVisibility(true);
    }
}
