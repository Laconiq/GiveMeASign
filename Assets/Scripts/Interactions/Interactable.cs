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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Interactable"))
            GameManager.Instance.playerController.AddNearbyInteractable(this);
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Interactable"))
            GameManager.Instance.playerController.RemoveNearbyInteractable(this);
    }
}
