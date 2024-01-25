using System.Collections.Generic;
using UnityEngine;

public class Progression : MonoBehaviour
{
    [HideInInspector] public bool isProgressionFinished;
    [SerializeField] private List<GameObject> objectsToActivate;
    [SerializeField] private List<GameObject> objectsToDeactivate;
    
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
