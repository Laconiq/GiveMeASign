using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PauseCanvas pauseCanvas;
    [SerializeField] private HandleCursor handleCursor;
    [SerializeField] private SliderSensitivity sliderSensitivity;

    public void Initialize()
    {
        pauseCanvas.Initialize();
        handleCursor.Initialize();
        sliderSensitivity.Initialize();
    }
}
