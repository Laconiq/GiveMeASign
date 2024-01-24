public class GrabbableObject : Interactable
{
    public bool isGrabbable = true;
    public ProgressionScriptableObject progressionToUnlock;
    
    public void ObjectIsGrabbed(bool b)
    {
        if (progressionToUnlock != null)
            progressionToUnlock.IsProgressionFinished = b;
    }
}
