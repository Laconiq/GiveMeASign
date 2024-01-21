using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Controls _controls;
    private void Awake()
    {
        Initialize();        
    }

    private void Initialize()
    {
        _controls = new Controls();
        _controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }
    
    private void Move(Vector2 direction)
    {
        Debug.Log(direction);
    }
}
