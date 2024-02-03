using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class Clock : MonoBehaviour
{
    [SerializeField] private float activationDelay;
    [SerializeField] private bool canUnlockProgression;
    [FormerlySerializedAs("progressionToUnlock")] [SerializeField, ShowIf("canUnlockProgression")] private Event eventToUnlock;
    [SerializeField] private bool canStartDialogue;
    [SerializeField, ShowIf("canStartDialogue")] private Npc npcToStartDialogue;
    
    public void StartClock()
    {
        Invoke(nameof(OnClockFinished), activationDelay);
    }
    
    protected virtual void OnClockFinished()
    {
        Debug.Log("Clock finished");
        if (canUnlockProgression)
            eventToUnlock.SetProgressionStatus(true);
        if (canStartDialogue)
            npcToStartDialogue.OnPlayerInteract();
    }
}
