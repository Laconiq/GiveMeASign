using Sirenix.OdinInspector;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private float activationDelay;
    [SerializeField] private bool canUnlockProgression;
    [SerializeField, ShowIf("canUnlockProgression")] private Progression progressionToUnlock;
    [SerializeField] private bool canStartDialogue;
    [SerializeField, ShowIf("canStartDialogue")] private Npc npcToStartDialogue;
    
    public void StartClock()
    {
        Invoke(nameof(OnClockFinished), activationDelay);
    }
    
    private void OnClockFinished()
    {
        Debug.Log("Clock finished");
        if (canUnlockProgression)
            progressionToUnlock.SetProgressionStatus(true);
        if (canStartDialogue)
            npcToStartDialogue.OnPlayerInteract();
    }
}
