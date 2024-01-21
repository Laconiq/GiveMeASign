using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Controls _controls;
    private Rigidbody _rb;
    private Player _player;
    private float _jumpForce;
    private float _currentSpeed;
    private Vector2 _moveInput;
    
    public void Initialize()
    {
        _player = GetComponent<Player>();
        _rb = _player.playerRigidbody;
        _currentSpeed = _player.basicSpeed;
        _jumpForce = _player.jumpForce * _rb.mass;
        
        _controls = new Controls();
        _controls.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += _ => _moveInput = Vector2.zero;
        _controls.Player.Jump.performed += _ => Jump();
        
        EnableControls();
    }
    
    private void EnableControls() { _controls.Enable(); }
    private void DisableControls() { _controls.Disable(); }

    private void OnDisable()
    {
        DisableControls();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.velocity = new Vector3(_moveInput.x * _currentSpeed, _rb.velocity.y, _moveInput.y * _currentSpeed);
    }

    
    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
}
