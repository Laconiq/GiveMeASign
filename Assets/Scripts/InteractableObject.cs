using System;
using UnityEngine;

public class InteractableObject : Interactable
{
    private Animator _animator;
    [SerializeField] private string animatorTriggerName;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _animator.SetTrigger(animatorTriggerName);        
    }
}
