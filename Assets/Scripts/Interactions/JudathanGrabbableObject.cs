using UnityEngine;
using UnityEngine.Serialization;

public class JudathanGrabbableObject : GrabbableObject
{
    private bool _hasBeenDropped;
    [FormerlySerializedAs("_eventToUnlockOnDrop")] [SerializeField] private Event eventToUnlockOnDrop;
    
    public override void ObjectIsGrabbed(bool b)
    {
        base.ObjectIsGrabbed(b);
        if (_hasBeenDropped || b)
            return;
        _hasBeenDropped = true;
        eventToUnlockOnDrop.SetProgressionStatus(true);
    }
}
