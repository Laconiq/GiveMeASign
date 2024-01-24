using UnityEngine;

[CreateAssetMenu(fileName = "NewProgression", menuName = "Dialogue System/Progression")]
public class ProgressionScriptableObject : ScriptableObject
{
    public bool _isProgressionFinished;
    
    public bool IsProgressionFinished
    {
        get => _isProgressionFinished;
        set => _isProgressionFinished = value;
    }
}
