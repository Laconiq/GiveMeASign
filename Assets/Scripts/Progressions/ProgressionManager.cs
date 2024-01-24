using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private List<ProgressionScriptableObject> progressionItems;
    public void Initialize()
    {
        foreach (var progressionItem in progressionItems)
            progressionItem.IsProgressionFinished = false;
    }
}
