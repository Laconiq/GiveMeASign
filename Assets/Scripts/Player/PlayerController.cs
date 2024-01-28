using System.Collections;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Title("Movement Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float standHeight = 2.0f;

    private Rigidbody _rigidbody;
    private BoxCollider _playerBoxCollider;
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
    public Controls DialogueControls;
    private Vector2 _moveInput;
    private bool _jumpInput;
    private bool _isCrouching;
    private float _currentSpeed;

    public void Initialize()
    {
        _feedbacks = GetComponent<PlayerFeedbacks>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerBoxCollider = GetComponent<BoxCollider>();
        _handleCursor = FindObjectOfType<HandleCursor>();
        _noise = cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _currentSpeed = movementSpeed;

        _controls = new Controls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Player.Jump.performed += _ => TryToJump();
        _controls.Player.Crouch.performed += _ => TryCrouch();
        _controls.Player.Talk.performed += _ => GetComponent<TalkToNpc>().TryTalkingToNpc();
        _controls.Player.Interact.performed += _ => Interact(throwForce);
        _controls.Player.DropObject.performed += _ => Interact(dropForce);

        _uiControls = new Controls();
        _uiControls.UI.Pause.performed += _ => FindObjectOfType<PauseCanvas>().SwitchPauseCanvas();
        _uiControls.UI.Enable();

        DialogueControls = new Controls();
        DialogueControls.Dialogue.SpeedUp.performed += _ => GameManager.Instance.dialogueManager.SetTextRevealSpeed(0.005f);
        DialogueControls.Dialogue.Disable();

        EnableControls();
        SetFPSCamera();
    }

    private void FixedUpdate()
    {
        GroundChecking();
        HandleMovement();
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
        float startHeight = _playerBoxCollider.size.y;
        float currentSpeed = _currentSpeed;

        while (currentTime < timeToCrouch)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / timeToCrouch;
            var size = _playerBoxCollider.size;
            size.y = Mathf.SmoothStep(startHeight, targetHeight, t);
            _playerBoxCollider.size = size;
            _currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, t);
            yield return null;
        }

        var vector3 = _playerBoxCollider.size;
        vector3.y = targetHeight;
        _playerBoxCollider.size = vector3;
        _currentSpeed = targetSpeed;
        _isCrouching = isCrouching;

        if (_isCrouching)
            _feedbacks.PlayCrouchFeedbackOn();
        else
            _feedbacks.PlayCrouchFeedbackOff();
    }

    private bool IsBlockedAbove()
    {
        Vector3 rayStart = transform.position + Vector3.up * (GetComponent<CapsuleCollider>().height / 2);
        bool hitSomething = Physics.Raycast(rayStart, Vector3.up, out _, standHeight - crouchHeight);
        return hitSomething;
    }

    private bool _isOnLadder;
    public void SetIsOnLadder(bool b) { _isOnLadder = b; }
    public bool GetIsOnLadder() { return _isOnLadder; }

    private void HandleMovement()
    {
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector3 moveDirection;

        if (_isOnLadder)
        {
            moveDirection = Vector3.up * _moveInput.y;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_currentSpeed * Time.deltaTime));
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
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_currentSpeed * Time.deltaTime));

            if (_moveInput != Vector2.zero)
            {
                _noise.m_FrequencyGain = _isCrouching ? 0.03f : 0.05f;
                PlayFootStep();
            }
            else
                _noise.m_FrequencyGain = 0.005f;
        }
    }

    private void GroundChecking()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        float rayLenght = _playerBoxCollider.size.y / 2 + 0.1f;
        _isGrounded = Physics.Raycast(rayStart, Vector3.down, rayLenght);
        Debug.DrawRay(rayStart, Vector3.down * rayLenght, Color.red);
    }

    private float _nextFootStepTime;
    private void PlayFootStep()
    {
        var cooldown = _isCrouching ? 0.5f : 0.3f;
        if (!(Time.time > _nextFootStepTime)) 
            return;
        _nextFootStepTime = Time.time + cooldown;
        //Debug.Log("Footstep");
    }

    private void TryToJump()
    {
        if (_isOnLadder)
            return;
        if (!_isGrounded) 
            return;
        _rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
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

    private void Interact(float force)
    {
        if (_heldObject != null)
        {
            _heldObjectRb.useGravity = true;
            _heldObjectRb.AddForce(_cameraTransform.forward * force, ForceMode.VelocityChange);
            _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(false);
            StopCoroutine(UpdateHoldPositionRoutine());
            _heldObject = null;
            _heldObjectRb = null;
        }
        else
        {
            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, 2f)) 
                return;
            if (IsObjectGrabbable(hit))
            {
                _heldObject = hit.collider.gameObject;
                _heldObjectRb = _heldObject.GetComponent<Rigidbody>();
                _heldObjectRb.useGravity = false;
                _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(true);
                StartCoroutine(UpdateHoldPositionRoutine());
            }
            else if (IsObjectInteractable(hit))
            {
                hit.collider.GetComponent<Interactable>().OnPlayerInteract();
            }
        }
    }

    private void CheckIfObjectIsGrabbable()
    {
        if (_heldObject is not null)
        {
            _handleCursor.SetGrabCursorVisibility(true);
            _handleCursor.SetUseCursorVisibility(false);
            _handleCursor.SetCrosshairVisibility(false);
            _handleCursor.SetGrabCursorClor(new Color(1f, 1f, 1f, 0.5f));
            return;
        }
        var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        if (Physics.Raycast(ray, out var hit, 2f) && IsObjectGrabbable(hit))
        {
            _handleCursor.SetGrabCursorClor(new Color(1, 1, 1, 1f));
            _handleCursor.SetGrabCursorVisibility(true);
        }
        else if (Physics.Raycast(ray, out hit, 2f) && IsObjectInteractable(hit))
            _handleCursor.SetUseCursorVisibility(true);
        else
        {
            _handleCursor.SetGrabCursorVisibility(false);
            _handleCursor.SetUseCursorVisibility(false);
            _handleCursor.SetCrosshairVisibility(true);
        }
    }

    private bool IsObjectGrabbable(RaycastHit hit)
    {
        var grabbableObject = hit.collider.GetComponent<GrabbableObject>();
        return hit.collider.CompareTag("Grabbable") && grabbableObject is not null && grabbableObject.isGrabbable;
    }
    
    private bool IsObjectInteractable(RaycastHit hit)
    {
        var interactableObject = hit.collider.GetComponent<Interactable>();
        if (interactableObject is null && hit.collider.CompareTag("Interactable"))
            Debug.Log("No interactable component found");
        return hit.collider.CompareTag("Interactable");
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
}
