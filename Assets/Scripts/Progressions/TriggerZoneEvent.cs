using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerZoneEvent : MonoBehaviour
{
    [SerializeField, Tooltip("Est-ce que la zone doit activer des progressions ?")] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression"), Tooltip("Les progressions validées quand le joueur est dans la zone")] private List<ProgressionScriptableObject> progressionsToUnlock;
    [SerializeField, Tooltip("Est-ce que la zone doit téléporter le joueur ?")] private bool canTeleportPlayer;
    [SerializeField, ShowIf("canTeleportPlayer"), Tooltip("Position où le joueur sera téléporté en rentrant dans la zone")] private Transform teleportPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        //Progression
        if (other.CompareTag("Player") && canUnlockProgression)
            foreach (ProgressionScriptableObject progression in progressionsToUnlock)
                progression.IsProgressionFinished = true;

        //Teleport
        if (other.CompareTag("Player") && canTeleportPlayer)
            other.transform.position = teleportPosition.position;
    }
}
