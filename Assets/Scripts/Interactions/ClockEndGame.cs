using UnityEngine;
using UnityEngine.SceneManagement;

public class ClockEndGame : Clock
{
    [SerializeField] private bool goBackToMainMenu;
    protected override void OnClockFinished()
    {
        if (goBackToMainMenu)
            SceneManager.LoadScene(0);
        else
            Application.Quit();
    }
}