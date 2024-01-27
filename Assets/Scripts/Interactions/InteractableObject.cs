using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableObject : Interactable
{
    [SerializeField] protected Animator animator;
    [SerializeField] private string animatorTriggerName;
    [SerializeField] private float delayBeforeTriggeringAnimation;
    [SerializeField] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression")] private Progression progressionToUnlock;
    [SerializeField] private bool canInteract = true;
    [SerializeField, HideIf("canInteract")] private Progression progressionToCheck;

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        if (!canInteract && progressionToCheck.GetProgressionStatus() == false)
            return;
        Invoke(nameof(SetAnimatorTrigger), delayBeforeTriggeringAnimation);
        if (canUnlockProgression)
            progressionToUnlock.SetProgressionStatus(true);
    }
    
    protected void SetAnimatorTrigger()
    {
        animator.SetTrigger(animatorTriggerName);
    }
}
