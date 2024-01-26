using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [HideInInspector] public bool isProgressionFinished;
    [SerializeField] private List<GameObject> objectsToActivate;
    [SerializeField] private List<GameObject> objectsToDeactivate;
    [SerializeField] private bool canStartClock;
    [SerializeField, ShowIf("canStartClock")] private Clock clockToStart;
    
    [SerializeField] private bool canMoveObject;
    [SerializeField, ShowIf("canMoveObject")] private GameObject objectToMove;
    [SerializeField, ShowIf("canMoveObject")] private Transform destination;
    
    [SerializeField] private bool canRemoveDialogue;
    [SerializeField, ShowIf("canRemoveDialogue")] private Dialogue dialogueToRemove;
    
    public bool GetProgressionStatus()
    {
        return isProgressionFinished;
    }
    
    public void SetProgressionStatus(bool status)
    {
        isProgressionFinished = status;
        if (!isProgressionFinished) 
            return;
        foreach (GameObject objectToActivate in objectsToActivate)
            objectToActivate.SetActive(true);
        
        foreach (GameObject objectToDeactivate in objectsToDeactivate)
            objectToDeactivate.SetActive(false);
        
        if (canStartClock)
            clockToStart.StartClock();
        
        if (canMoveObject)
        {
            objectToMove.transform.position = destination.position;
            objectToMove.transform.rotation = destination.rotation;
        }

        if (canRemoveDialogue)
            dialogueToRemove.isDialogueFinished = true;
    }
    
    public void ResetProgression()
    {
        isProgressionFinished = false;
        foreach (GameObject objectToActivate in objectsToActivate)
            objectToActivate.SetActive(false);
        foreach (GameObject objectToDeactivate in objectsToDeactivate)
            objectToDeactivate.SetActive(true);
    }
}
