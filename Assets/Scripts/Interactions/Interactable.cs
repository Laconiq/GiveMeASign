using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void OnPlayerInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
