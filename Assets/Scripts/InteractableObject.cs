using System;
using UnityEngine;

public class InteractableObject : Interactable
{
    private Animator _animator;
    [SerializeField] private string animatorTriggerName;
    [SerializeField] private float delayBeforeTriggeringAnimation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        Invoke(nameof(SetAnimatorTrigger), delayBeforeTriggeringAnimation);
    }
    
    private void SetAnimatorTrigger()
    {
        _animator.SetTrigger(animatorTriggerName);
    }
}
