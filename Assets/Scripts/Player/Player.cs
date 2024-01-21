using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Title("Controls")]
    public float basicSpeed;
    public float jumpForce;
    
    [Title("Data")]
    [SerializeField] private PlayerController playerController;
    public Rigidbody playerRigidbody;
    
    public void Initialize()
    {
        playerController.Initialize();
    }
}
