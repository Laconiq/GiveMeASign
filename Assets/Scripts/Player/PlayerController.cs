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

    private CharacterController _controller;
    private PlayerFeedbacks _feedbacks;
    private Vector3 _playerVelocity;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private bool _isGrounded;
    private Transform _cameraTransform;
    private Interactable _nearbyInteractable;

    private Controls _controls;
    private Vector2 _moveInput;
    private bool _jumpInput;
    private bool _isCrouching;
    private float _currentSpeed;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _feedbacks = GetComponent<PlayerFeedbacks>();
        _controller = GetComponent<CharacterController>();
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _currentSpeed = movementSpeed;

        _controls = new Controls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Player.Jump.performed += _ => _jumpInput = true;
        _controls.Player.Jump.canceled += _ => _jumpInput = false;
        _controls.Player.Crouch.performed += _ => TryCrouch();
        _controls.Player.Interact.performed += _ => TryToInteract();
        
        EnableControls();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJumpAndGravity();
    }
    
    private void TryCrouch()
    {
        if (_isCrouching)
        {
            if (IsBlockedAbove()) 
                return;
            _controller.height = standHeight;
            _currentSpeed = movementSpeed;
            _isCrouching = false;
            _feedbacks.PlayCrouchFeedbackOff();
        }
        else
        {
            _controller.height = crouchHeight;
            _currentSpeed = crouchSpeed;
            _isCrouching = true;
            _feedbacks.PlayCrouchFeedbackOn();
        }
    }
    
    private bool IsBlockedAbove()
    {
        bool hitSomething = Physics.Raycast(_controller.transform.position + _controller.center, Vector3.up, out _, standHeight - crouchHeight);
        return hitSomething;
    }

    private void HandleMovement()
    {
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;
        _controller.Move(moveDirection * (_currentSpeed * Time.deltaTime));
    }

    private void HandleJumpAndGravity()
    {
        if (_jumpInput && _isGrounded)
            _playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        _playerVelocity.y += gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
            _nearbyInteractable = other.GetComponent<Interactable>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
            _nearbyInteractable = null;
    }
    
    private void TryToInteract()
    {
        if (_nearbyInteractable != null)
            _nearbyInteractable.OnPlayerInteract();
    }
    
    public void DisableControls()
    {
        var povComponent = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        povComponent.m_HorizontalAxis.m_MaxSpeed = 0;
        povComponent.m_VerticalAxis.m_MaxSpeed = 0;
        _controls.Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void EnableControls()
    {
        var povComponent = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        povComponent.m_HorizontalAxis.m_MaxSpeed = 300;
        povComponent.m_VerticalAxis.m_MaxSpeed = 300;
        _controls.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
