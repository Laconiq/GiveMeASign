using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Title("Movement Settings")]
    [SerializeField] private float movementSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 2.0f;

    [Title("Player Height")]
    [SerializeField] private float standHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    
    [Title("Force")]
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private float dropForce = 2.0f;
    
    [Title("Head Bobbing")]
    [SerializeField] private float walkingHeadBobSpeed = 0.05f;
    [SerializeField] private float crouchingHeadBobSpeed = 0.03f;
    
    [HideInInspector] public float sensitivity = 300f;
    [HideInInspector] public Transform cameraTransform;
    private Rigidbody _playerRigidBody;
    private CapsuleCollider _playerBoxCollider;
    private PlayerFeedbacks _feedbacks;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private PlayerCamera _playerCamera;
    private float _slopeAngle;

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
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerBoxCollider = GetComponent<CapsuleCollider>();
        _playerCamera = GetComponent<PlayerCamera>();
        if (Camera.main != null) cameraTransform = Camera.main.transform;
        _currentSpeed = movementSpeed;

        _controls = new Controls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Player.Crouch.performed += _ => TryCrouch();
        _controls.Player.Talk.performed += _ => GetComponent<PlayerTalkToNpc>().TryTalkingToNpc();
        _controls.Player.Interact.performed += _ => GetComponent<PlayerInteraction>().Interact(throwForce);
        _controls.Player.DropObject.performed += _ => GetComponent<PlayerInteraction>().Interact(dropForce);

        _uiControls = new Controls();
        _uiControls.UI.Pause.performed += _ => FindObjectOfType<PauseCanvas>().SwitchPauseCanvas();
        _uiControls.UI.Enable();

        DialogueControls = new Controls();
        DialogueControls.Dialogue.SpeedUp.performed += _ => GameManager.Instance.dialogueManager.SetTextRevealSpeed(0.005f);
        DialogueControls.Dialogue.Disable();

        _playerCamera.SetFPSCamera();
        EnableControls();
    }

    private void FixedUpdate()
    {
        GroundChecking();
        HandleMovement();
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
        float startHeight = _playerBoxCollider.height;
        float currentSpeed = _currentSpeed;

        while (currentTime < timeToCrouch)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / timeToCrouch;
            _playerBoxCollider.height = Mathf.SmoothStep(startHeight, targetHeight, t);
            _currentSpeed = Mathf.SmoothStep(currentSpeed, targetSpeed, t);
            yield return null;
        }

        _playerBoxCollider.height = targetHeight;
        _currentSpeed = targetSpeed;
        _isCrouching = isCrouching;

        if (_isCrouching)
            _feedbacks.PlayCrouchFeedbackOn();
        else
            _feedbacks.PlayCrouchFeedbackOff();
    }

    private bool IsBlockedAbove()
    {
        Vector3 rayStart = transform.position + Vector3.up * (_playerBoxCollider.height / 2);
        bool hitSomething = Physics.Raycast(rayStart, Vector3.up, out _, standHeight - crouchHeight);
        return hitSomething;
    }

    private bool _isOnLadder;

    public void SetIsOnLadder(bool b)
    {
        _isOnLadder = b;
        _playerRigidBody.useGravity = !b;
    }
    public bool GetIsOnLadder() { return _isOnLadder; }

    private void HandleMovement()
    {
        if (_isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        Vector3 moveDirection;

        if (_isOnLadder)
        {
            moveDirection = Vector3.up * _moveInput.y;
            _playerRigidBody.MovePosition(_playerRigidBody.position + moveDirection * (_currentSpeed * Time.deltaTime));
        }
        else
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            moveDirection = forward * _moveInput.y + right * _moveInput.x;
            _playerRigidBody.MovePosition(_playerRigidBody.position + moveDirection * (_currentSpeed * Time.deltaTime));

            if (_moveInput != Vector2.zero)
            {
                _playerCamera.HeadBob(!_isCrouching ? walkingHeadBobSpeed : crouchingHeadBobSpeed);
                PlayFootStep();
            }
            else
                _playerCamera.HeadBob(0.005f);
        }
    }

    private void GroundChecking()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;
        float rayLenght = _playerBoxCollider.height / 2 + 0.1f;
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

    public void DisableControls()
    {
        GetComponent<PlayerCamera>().EnablePovCamera(false);
        _controls.Disable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void EnableControls()
    {
        GetComponent<PlayerCamera>().EnablePovCamera(true);
        _controls.Enable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
