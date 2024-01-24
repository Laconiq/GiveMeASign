using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerFeedbacks : MonoBehaviour
{
    public MMF_Player crouchFeedbackOn;
    public MMF_Player crouchFeedbackOff;

    public void PlayCrouchFeedbackOn()
    {
        crouchFeedbackOff.StopFeedbacks();
        crouchFeedbackOn.PlayFeedbacks();
    }

    public void PlayCrouchFeedbackOff()
    {
        crouchFeedbackOn.StopFeedbacks();
        crouchFeedbackOff.PlayFeedbacks();
    }
}
