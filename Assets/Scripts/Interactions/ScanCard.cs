using UnityEngine;

public class ScanCard : GrabbableObject
{
    [SerializeField] private Event eventToUnlockOnScan;
    protected override void OnTriggerEnter(Collider other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        Debug.Log("CurrentTriggerZone is: " + currentTriggerZone + " and triggerZone is: " + triggerZone);
        if (currentTriggerZone == null || currentTriggerZone != triggerZone) 
            return;
        eventToUnlockOnScan.SetProgressionStatus(true);
        Debug.Log("Object is in trigger zone");
    }
}
