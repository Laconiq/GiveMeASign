using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableObject : Interactable
{
    [SerializeField] protected Animator animator;
    [SerializeField] private string animatorTriggerName;
    [SerializeField] private float delayBeforeTriggeringAnimation;
    [SerializeField] private bool canUnlockProgression;
    [FormerlySerializedAs("progressionToUnlock")] [SerializeField, ShowIf("canUnlockProgression")] private Event eventToUnlock;
    [SerializeField] private bool canInteract = true;
    [FormerlySerializedAs("progressionToCheck")] [SerializeField, HideIf("canInteract")] private Event eventToCheck;
    [SerializeField] private AK.Wwise.Event interactSound;

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        interactSound?.Post(gameObject);
        if (!canInteract && eventToCheck.GetProgressionStatus() == false)
            return;
        if (animatorTriggerName != "" && animator is not null)
            Invoke(nameof(SetAnimatorTrigger), delayBeforeTriggeringAnimation);  
        if (canUnlockProgression)
            eventToUnlock.SetProgressionStatus(true);
    }
    
    protected void SetAnimatorTrigger()
    {
        animator.SetTrigger(animatorTriggerName);
    }
}
