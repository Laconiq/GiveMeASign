using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private List<ProgressionScriptableObject> progressionItems;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (var progressionItem in progressionItems)
            progressionItem.IsProgressionFinished = false;
    }
}
