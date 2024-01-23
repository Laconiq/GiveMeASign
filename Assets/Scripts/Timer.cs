using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timeElapsed = 0f;
    private bool timerRunning = false;

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    private void Update()
    {
        if (timerRunning)
        {
            timeElapsed += Time.deltaTime;
        }
    }
}