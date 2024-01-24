using UnityEngine;

public class Interactable : MonoBehaviour
{
    private enum InteractionType
    {
        Object,
        Npc
    }
    
    [SerializeField] private InteractionType interactionType;

    public virtual void OnPlayerInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
