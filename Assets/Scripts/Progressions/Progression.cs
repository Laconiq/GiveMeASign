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
