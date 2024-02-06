using UnityEngine;

public class ScanCard : GrabbableObject
{
    [SerializeField] private Event eventToUnlockOnScan;
    protected override void OnTriggerStay(Collider other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone == null || currentTriggerZone != triggerZone) 
            return;
        eventToUnlockOnScan.SetProgressionStatus(true);
    }
}
