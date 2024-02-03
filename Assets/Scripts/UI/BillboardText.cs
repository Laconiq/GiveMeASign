using UnityEngine;

public class BillboardText : MonoBehaviour
{
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (_player is null) 
            return;
        Vector3 directionToPlayer = _player.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = rotation;
    }
}