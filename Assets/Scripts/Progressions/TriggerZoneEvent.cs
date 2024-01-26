using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerZoneEvent : MonoBehaviour
{
    [SerializeField] private bool isLockedByProgression;
    [SerializeField, ShowIf("isLockedByProgression")] private Progression progressionToUnlock;
    
    [SerializeField, Tooltip("Est-ce que la zone doit activer des progressions ?")] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression"), Tooltip("Les progressions validées quand le joueur est dans la zone")] private List<Progression> progressionsToUnlock;
    [SerializeField, Tooltip("Est-ce que la zone doit téléporter le joueur ?")] private bool canTeleportPlayer;
    [SerializeField, ShowIf("canTeleportPlayer"), Tooltip("Position où le joueur sera téléporté en rentrant dans la zone")] private Transform teleportPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (isLockedByProgression && !progressionToUnlock.GetProgressionStatus())
            return;
        
        //Progression
        if (other.CompareTag("Player") && canUnlockProgression)
            foreach (Progression progression in progressionsToUnlock)
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
