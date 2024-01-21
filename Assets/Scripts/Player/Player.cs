using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Title("Speed")]
    public float basicSpeed = 10f;
    
    [Title("Data")]
    [SerializeField] private PlayerController playerController;
    public Rigidbody playerRigidbody;
    
    public void Initialize()
    {
        playerController.Initialize();
    }
}
