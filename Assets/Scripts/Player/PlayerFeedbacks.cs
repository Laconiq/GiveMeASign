using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    [SerializeField] private MMF_Player speedUpFeedback;
    [SerializeField] private MMF_Player speedDownFeedback;
    
    public void PlaySpeedUpFeedback()
    {
        speedDownFeedback.StopFeedbacks();
        speedUpFeedback.PlayFeedbacks();
    }
    
    public void PlaySpeedDownFeedback()
    {
        speedUpFeedback.StopFeedbacks();
        speedDownFeedback.PlayFeedbacks();
    }
}
