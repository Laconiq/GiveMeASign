using Sirenix.OdinInspector;
using UnityEngine;

public class GrabbableObject : Interactable
{
    [Title("Global Settings")]
    public bool isGrabbable = true;
    [ShowIf("isGrabbable"), Tooltip("La progression validée quand l'objest est en main")] public ProgressionScriptableObject progressionToUnlockOnGrab;
    
    [Title("Trigger Zone Settings")]
    [SerializeField, Tooltip("Est-ce que l'objet peut activer une progression quand il est dans une zone ?")] private bool canToggleEventInTriggerZone;
    [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La zone où doit être l'objet")] private TriggerZoneEvent triggerZone;
    [SerializeField, ShowIf("canToggleEventInTriggerZone"), Tooltip("La progression validée quand l'objet est dans la zone")] private ProgressionScriptableObject progressionToUnlockInTriggerZone;
    
    public void ObjectIsGrabbed(bool b)
    {
        if (progressionToUnlockOnGrab != null)
            progressionToUnlockOnGrab.IsProgressionFinished = b;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone)
            progressionToUnlockInTriggerZone.IsProgressionFinished = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (!canToggleEventInTriggerZone)
            return;
        var currentTriggerZone = other.gameObject.GetComponent<TriggerZoneEvent>();
        if (currentTriggerZone != null && currentTriggerZone == triggerZone)
            progressionToUnlockInTriggerZone.IsProgressionFinished = false;
    }
}
