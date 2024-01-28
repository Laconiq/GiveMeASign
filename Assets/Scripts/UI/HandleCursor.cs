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

    public void DisplayGrabCursor()
    {
        grabCursor.SetActive(true);
        useCursor.SetActive(false);
        crosshair.SetActive(false);
    }
    
    public void SetGrabCursorColor(Color color)
    {
        grabCursor.GetComponent<Image>().color = color;
    }
    
    public void DisplayUseCursor()
    {
        grabCursor.SetActive(false);
        useCursor.SetActive(true);
        crosshair.SetActive(false);
    }
    
    public void DisplayCrosshair()
    {
        grabCursor.SetActive(false);
        useCursor.SetActive(false);
        crosshair.SetActive(true);
    }
}
