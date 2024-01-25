using UnityEngine;
using UnityEngine.Rendering;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    private PlayerController _playerController;
    [SerializeField] private Volume blurVolume;

    public void Initialize()
    {
        pauseCanvas.SetActive(false);
        blurVolume.weight = 0;
        _playerController = GameManager.Instance.playerController;
    }

    public void SwitchPauseCanvas()
    {
        bool isPaused = pauseCanvas.activeSelf;

        if (isPaused)
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _playerController.EnableControls();
            blurVolume.weight = 0;
        }
        else
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _playerController.DisableControls();
            blurVolume.weight = 1;
        }
    }
}
