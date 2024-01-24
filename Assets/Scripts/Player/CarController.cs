using Sirenix.OdinInspector;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Controls _controls;
    private Player _player;
    private CameraManager _cameraManager;
    private PlayerFeedbacks _playerFeedbacks;
    private GameCanvas _gameCanvas;
    private Rigidbody _rb;

    [Title("Controls")]
    [SerializeField] private float jumpForce;
    private bool _isGrounded;
    
    [Title("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float speedBoost;
    [SerializeField] private float turnSpeed;
    private float _turnInput;
    private float _currentSpeed;
    
    [Title("Boost")]
    [InfoBox("Tous les paramètres sont en secondes")]
    [SerializeField] private float maxBoost = 100f;
    [SerializeField] private float boostConsumption = 10f;
    [SerializeField] private float boostRechargeRate = 5f;
    [SerializeField] private float minimumBoostToActivate = 20f;
    private float _currentBoost;
    private bool _isBoosting;

    [Title("FOV")]
    [SerializeField] private float fieldOfView;
    [SerializeField] private float speedUpFieldOfView;
    [SerializeField] private float timeToChangeFOV;

    public void Initialize()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
        _gameCanvas = FindObjectOfType<GameCanvas>();
        _player = GetComponent<Player>();
        _playerFeedbacks = GetComponent<PlayerFeedbacks>();
        _rb = _player.playerRigidBody;
        _currentSpeed = speed;
        _currentBoost = maxBoost;
        
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
    
    [Title("Wheel Configuration")]
    [SerializeField] private Transform leftFrontWheel;
    [SerializeField] private Transform rightFrontWheel;
    [SerializeField] private float maxWheelTurnAngle = 30f; 

    private void TurnWheel()
    {
        float wheelTurnAngle = _turnInput * maxWheelTurnAngle;
        leftFrontWheel.localRotation = Quaternion.Euler(0f, wheelTurnAngle, 0f);
        rightFrontWheel.localRotation = Quaternion.Euler(0f, wheelTurnAngle, 0f);

        Vector3 forwardMovement = transform.forward * (_currentSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + forwardMovement);

        if (_turnInput != 0) 
        {
            Quaternion turnRotation = Quaternion.Euler(0f, wheelTurnAngle * Time.fixedDeltaTime, 0f);
            _rb.MoveRotation(_rb.rotation * turnRotation);
        }

        // Reset Z rotation
        Vector3 currentRotation = _rb.rotation.eulerAngles;
        _rb.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    
    private void SpeedUp()
    {
        if (!(_currentBoost >= minimumBoostToActivate)) 
            return;
        _currentSpeed = speedBoost;
        _cameraManager.SetCameraFOV(speedUpFieldOfView, timeToChangeFOV);
        _playerFeedbacks.PlaySpeedUpFeedback();
        _isBoosting = true;
    }
    
    private void SpeedDown()
    {
        if (!_isBoosting)
            return;
        _currentSpeed = speed;
        _cameraManager.SetCameraFOV(fieldOfView, timeToChangeFOV);
        _playerFeedbacks.PlaySpeedDownFeedback();
        _isBoosting = false;
    }

    private void UpdateBoost()
    {
        if (_isBoosting)
        {
            _currentBoost -= boostConsumption * Time.fixedDeltaTime;
            if (_currentBoost <= 0f)
                SpeedDown();
        }
        else if (!(_currentBoost >= maxBoost) && !_isBoosting)
        {
            _currentBoost += boostRechargeRate * Time.fixedDeltaTime;
            _currentBoost = Mathf.Min(_currentBoost, maxBoost);
        }
        _gameCanvas.UpdateBoostBar(_currentBoost / maxBoost);
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
