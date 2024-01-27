using UnityEngine;
using UnityEngine.UI;

public class HandleCursor : MonoBehaviour
{
    [SerializeField] private GameObject grabCursor;
    [SerializeField] private GameObject useCursor;
    [SerializeField] private GameObject crosshair;

    public void Initialize()
    {
        grabCursor.SetActive(false);
    }

    public void SetGrabCursorVisibility(bool visibility)
    {
        grabCursor.SetActive(visibility);
    }
    
    public void SetGrabCursorClor(Color color)
    {
        grabCursor.GetComponent<Image>().color = color;
    }
    
    public void SetUseCursorVisibility(bool visibility)
    {
        useCursor.SetActive(visibility);
    }
    
    public void SetCrosshairVisibility(bool visibility)
    {
        crosshair.SetActive(visibility);
    }
}
