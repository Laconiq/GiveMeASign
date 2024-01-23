using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Title("Data")]
    [SerializeField] private CarController carController;
    public Rigidbody playerRigidBody;
    
    public void Initialize()
    {
        carController.Initialize();
    }
}
