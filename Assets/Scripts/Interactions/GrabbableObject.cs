using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GrabbableObject : Interactable
{
    [Title("Global Settings")]
    public bool isGrabbable = true;
    [FormerlySerializedAs("progressionToUnlockOnGrab")] [ShowIf("isGrabbable"), Tooltip("La progression validée quand l'objest est en main")] public Event eventToUnlockOnGrab;
    
    [Title("Trigger Zone Settings")]
    [SerializeField, Tooltip("Est-ce que l'objet peut activer une progression quand il est dans une zone ?")] private bool canToggleEventInTriggerZone;
    [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La zone où doit être l'objet")] private TriggerZoneEvent triggerZone;
    [FormerlySerializedAs("progressionToUnlockInTriggerZone")] [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La progression validée quand l'objet est dans la zone")] private Event eventToUnlockInTriggerZone;
    
    public virtual void ObjectIsGrabbed(bool b)
    {
        if (eventToUnlockOnGrab != null)
            eventToUnlockOnGrab.SetProgressionStatus(b);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone)
            eventToUnlockInTriggerZone.SetProgressionStatus(true);
    }

    private void OnCollisionExit(Collision other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone)
            eventToUnlockInTriggerZone.SetProgressionStatus(false);
    }
}
