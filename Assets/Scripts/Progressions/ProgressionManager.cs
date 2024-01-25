using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

public class ProgressionManager : MonoBehaviour
{
    private List<Progression> _progressionItems;
    public void Initialize()
    {
        _progressionItems = new List<Progression>();
        foreach (Transform child in transform)
        {
            Progression progression = child.GetComponent<Progression>();
            if (progression != null)
                _progressionItems.Add(progression);
        }
        
        foreach (var progressionItem in _progressionItems)
            progressionItem.ResetProgression();
    }

#if UNITY_EDITOR
    [Button("Add Progression")]
    private void AddProgressionEditor()
    {
        GameObject progressionPrefab = Resources.Load<GameObject>("Progression");
        if (progressionPrefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(progressionPrefab, transform);
            instance.name = "Progression";
            Undo.RegisterCreatedObjectUndo(instance, "Create progression instance");
        }
        else
            Debug.LogWarning("Progression prefab not found in Resources.");
    }
#endif
}