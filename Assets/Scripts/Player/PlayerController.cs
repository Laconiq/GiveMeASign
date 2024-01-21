using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Controls _controls;
    private Rigidbody _rb;
    private Player _player;
    private float _currentSpeed;
    
    public void Initialize()
    {
        _player = GetComponent<Player>();
        _rb = _player.playerRigidbody;
        _currentSpeed = _player.basicSpeed;
        _controls = new Controls();
        _controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        _controls.Player.Move.canceled += _ => Move(Vector2.zero);
        EnableControls();
    }

    private void EnableControls() { _controls.Enable(); }
    private void DisableControls() { _controls.Disable(); }
    
    private void Move(Vector2 direction)
    {
        _rb.velocity = new Vector3(direction.x, 0, direction.y) * _currentSpeed;
    }
}
