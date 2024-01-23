using Sirenix.OdinInspector;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Controls _controls;
    private Player _player;
    private float _turnInput;
    private Rigidbody _rb;
    private CameraManager _cameraManager;
    private bool _isGrounded;
    private PlayerFeedbacks _playerFeedbacks;

    [Title("Controls")]
    [SerializeField] private float jumpForce;
    
    [Title("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float speedBoost;
    private float _currentSpeed;
    [SerializeField] private float turnSpeed;
    
    [Title("FOV")]
    [SerializeField] private float fieldOfView;
    [SerializeField] private float speedUpFieldOfView;
    [SerializeField] private float timeToChangeFOV;

    public void Initialize()
    {
        _player = GetComponent<Player>();
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
        _rb = _player.playerRigidBody;
        _cameraManager = FindObjectOfType<CameraManager>();
        _currentSpeed = speed;
        
        _controls = new Controls();
        _controls.Player.TurnWheel.performed += ctx => 
        {
            Vector2 turnVector = ctx.ReadValue<Vector2>();
            _turnInput = turnVector.x;
        };
        _controls.Player.TurnWheel.canceled += _ => _turnInput = 0f;
        _controls.Player.SpeedUp.performed += _ => SpeedUp();
        _controls.Player.SpeedUp.canceled += _ => SpeedDown();
        _controls.Player.Jump.performed += _ => Jump();
        
        _controls.Enable();
    }
    
    private void TurnWheel()
    {
        _rb.MovePosition(transform.position + transform.forward * (_currentSpeed * Time.fixedDeltaTime));
        Quaternion turnRotation = Quaternion.Euler(0f, _turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);
    }
    
    private void SpeedUp()
    {
        _currentSpeed = speedBoost;
        _cameraManager.SetCameraFOV(speedUpFieldOfView, timeToChangeFOV);
        _playerFeedbacks.PlaySpeedUpFeedback();
    }
    
    private void SpeedDown()
    {
        _currentSpeed = speed;
        _cameraManager.SetCameraFOV(fieldOfView, timeToChangeFOV);
        _playerFeedbacks.PlaySpeedDownFeedback();
    }

    private void UpdateBoost()
    {
        //Mettre en place la recharge du boost
    }

    private void Jump()
    {
        if (!_isGrounded)
            return;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _isGrounded = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }
    
    private void FixedUpdate()
    {
        TurnWheel();
        UpdateBoost();
    }
}