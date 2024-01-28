using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerZoneEvent : MonoBehaviour
{
    [SerializeField] private bool isLockedByProgression;
    [FormerlySerializedAs("progressionToUnlock")] [SerializeField, ShowIf("isLockedByProgression")] private Event eventToUnlock;
    
    [SerializeField, Tooltip("Est-ce que la zone doit activer des progressions ?")] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression"), Tooltip("Les progressions validées quand le joueur est dans la zone")] private List<Event> progressionsToUnlock;
    [SerializeField, Tooltip("Est-ce que la zone doit téléporter le joueur ?")] private bool canTeleportPlayer;
    [SerializeField, ShowIf("canTeleportPlayer"), Tooltip("Position où le joueur sera téléporté en rentrant dans la zone")] private Transform teleportPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isLockedByProgression && !eventToUnlock.GetProgressionStatus())
            return;
        
        //Progression
        if (other.CompareTag("Player") && canUnlockProgression)
            foreach (Event progression in progressionsToUnlock)
                progression.SetProgressionStatus(true);

        //Teleport
        if (other.CompareTag("Player") && canTeleportPlayer)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = teleportPosition.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
