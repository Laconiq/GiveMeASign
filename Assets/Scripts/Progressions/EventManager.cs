using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

public class EventManager : MonoBehaviour
{
    private List<Event> _progressionItems;
    public void Initialize()
    {
        _progressionItems = new List<Event>();
        foreach (Transform child in transform)
        {
            Event @event = child.GetComponent<Event>();
            if (@event != null)
                _progressionItems.Add(@event);
        }
        
        foreach (var progressionItem in _progressionItems)
            progressionItem.ResetProgression();
    }

#if UNITY_EDITOR
    [Button("Add Event")]
    private void AddProgressionEditor()
    {
        GameObject progressionPrefab = Resources.Load<GameObject>("Progression");
        if (progressionPrefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(progressionPrefab, transform);
            instance.name = "New Event";
            Undo.RegisterCreatedObjectUndo(instance, "Create event instance");
        }
        else
            Debug.LogWarning("Event prefab not found in Resources.");
    }
#endif
}