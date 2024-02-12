using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GrabbableObject : Interactable
{
    [Title("Global Settings")]
    public bool isGrabbable = true;
    private bool isGrabbed;
    [FormerlySerializedAs("progressionToUnlockOnGrab")] [ShowIf("isGrabbable"), Tooltip("La progression validée quand l'objest est en main")] public Event eventToUnlockOnGrab;
    
    [Title("Trigger Zone Settings")]
    [SerializeField, Tooltip("Est-ce que l'objet peut activer une progression quand il est dans une zone ?")] protected bool canToggleEventInTriggerZone;
    [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La zone où doit être l'objet")] protected TriggerZoneEvent triggerZone;
    [FormerlySerializedAs("progressionToUnlockInTriggerZone")] [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La progression validée quand l'objet est dans la zone")] private Event eventToUnlockInTriggerZone;
    
    public virtual void ObjectIsGrabbed(bool b)
    {
        isGrabbed = b;
        if (eventToUnlockOnGrab != null)
            eventToUnlockOnGrab.SetProgressionStatus(b);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (!canToggleEventInTriggerZone || eventToUnlockInTriggerZone.GetProgressionStatus())
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone && eventToUnlockInTriggerZone is not null)
        {
            eventToUnlockInTriggerZone.SetProgressionStatus(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone && eventToUnlockInTriggerZone is not null)
            eventToUnlockInTriggerZone.SetProgressionStatus(false);
        if (eventToUnlockInTriggerZone != null && eventToUnlockInTriggerZone == eventToUnlockOnGrab && isGrabbed)
            eventToUnlockInTriggerZone.SetProgressionStatus(true);
    }
}
