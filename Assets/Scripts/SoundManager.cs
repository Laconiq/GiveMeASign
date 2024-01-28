using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{ 
    [SerializeField] private AK.Wwise.Event startMusicEvent;
    [SerializeField] private AK.Wwise.Event stopMusicEvent;

    private void Start()
    {
        StartMusic();
    }

    public void StartMusic()
    {
        startMusicEvent.Post(gameObject);
    }
    
    public void StopMusic()
    {
        stopMusicEvent.Post(gameObject);
    }
}
