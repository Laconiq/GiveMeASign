using UnityEngine;

public class Interactable : MonoBehaviour
{
    private enum InteractionType
    {
        GrabbableObject,
        Npc,
        Object
    }
    
    [SerializeField] private InteractionType interactionType;

    public virtual void OnPlayerInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
