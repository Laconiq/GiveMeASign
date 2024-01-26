using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableObject : Interactable
{
    private Animator _animator;
    [SerializeField] private string animatorTriggerName;
    [SerializeField] private float delayBeforeTriggeringAnimation;
    [SerializeField] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression")] private Progression progressionToUnlock;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        Invoke(nameof(SetAnimatorTrigger), delayBeforeTriggeringAnimation);
        if (canUnlockProgression)
            progressionToUnlock.SetProgressionStatus(true);
    }
    
    private void SetAnimatorTrigger()
    {
        _animator.SetTrigger(animatorTriggerName);
    }
}
