using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerZoneEvent : MonoBehaviour
{
    [SerializeField] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression")] private List<ProgressionScriptableObject> progressionsToUnlock;
    [SerializeField] private bool canTeleportPlayer;
    [SerializeField, ShowIf("canTeleportPlayer")] private Transform teleportPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        //Progression
        if (other.CompareTag("Player") && canUnlockProgression)
            foreach (ProgressionScriptableObject progression in progressionsToUnlock)
                progression.IsProgressionFinished = true;

        //Teleport
        if (other.CompareTag("Player") && canTeleportPlayer)
            other.transform.position = teleportPosition.position;
    }
}
