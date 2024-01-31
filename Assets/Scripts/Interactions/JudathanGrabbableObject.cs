using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudathanGrabbableObject : GrabbableObject
{
    private bool _hasBeenDropped;
    [SerializeField] private Event _eventToUnlockOnDrop;
    
    public override void ObjectIsGrabbed(bool b)
    {
        base.ObjectIsGrabbed(b);
        if (_hasBeenDropped || b)
            return;
        _hasBeenDropped = true;
        _eventToUnlockOnDrop.SetProgressionStatus(true);
    }
}
