using UnityEngine;

public class SoundManager : MonoBehaviour
{ 
    [SerializeField] private AK.Wwise.Event startMusicEvent;
    [SerializeField] private AK.Wwise.Event stopMusicEvent;

    private void Start()
    {
        StartMusic();
    }

    private void StartMusic()
    {
        startMusicEvent.Post(gameObject);
    }
    
    public void StopMusic()
    {
        stopMusicEvent.Post(gameObject);
    }
}
