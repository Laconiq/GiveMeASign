using System;
using UnityEngine;

public class HandleCursor : MonoBehaviour
{
    [SerializeField] private GameObject handleCursor;

    private void Awake()
    {
        handleCursor.SetActive(false);
    }

    public void SetCursorVisibility(bool visibility)
    {
        handleCursor.SetActive(visibility);
    }
}
