using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneEvent : MonoBehaviour
{
    [SerializeField] private List<ProgressionScriptableObject> progressionsToUnlock;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;
        foreach (ProgressionScriptableObject progression in progressionsToUnlock)
            progression.IsProgressionFinished = true;
    }
}
