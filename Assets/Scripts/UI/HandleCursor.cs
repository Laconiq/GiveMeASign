using UnityEngine;

public class HandleCursor : MonoBehaviour
{
    [SerializeField] private GameObject handleCursor;

    public void Initialize()
    {
        handleCursor.SetActive(false);
    }

    public void SetCursorVisibility(bool visibility)
    {
        handleCursor.SetActive(visibility);
    }
}
