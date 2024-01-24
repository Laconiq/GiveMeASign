using Sirenix.OdinInspector;
using UnityEngine;

public class GrabbableObject : Interactable
{
    [Title("Global Settings")]
    public bool isGrabbable = true;
    public ProgressionScriptableObject progressionToUnlock;
    
    [Title("Trigger Zone Settings")]
    [SerializeField] private bool isTriggerZone;
    [SerializeField, ShowIf("isTriggerZone")] private TriggerZoneEvent triggerZoneEvent;
    [SerializeField, ShowIf("isTriggerZone")] private ProgressionScriptableObject progressionOnTriggerZone;
    
    public void ObjectIsGrabbed(bool b)
    {
        if (progressionToUnlock != null)
            progressionToUnlock.IsProgressionFinished = b;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZoneEvent)
            progressionOnTriggerZone.IsProgressionFinished = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (!isTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZoneEvent)
            progressionOnTriggerZone.IsProgressionFinished = false;
    }
}
