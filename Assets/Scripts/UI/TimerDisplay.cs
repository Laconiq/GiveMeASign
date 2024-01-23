using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Timer timer;

    private void Update()
    {
        timerText.text = timer.GetTimeElapsed().ToString("F2");
    }
}
