using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCameraDialogue;
    private PlayerController _playerController;
    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _noise = cineMachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void SetDialogueCamera()
    {
        cineMachineVirtualCamera.Priority = 9;
        cineMachineVirtualCameraDialogue.Priority = 11;
    }
    
    public void SetFPSCamera()
    {
        cineMachineVirtualCamera.Priority = 11;
        cineMachineVirtualCameraDialogue.Priority = 9;
        cineMachineVirtualCameraDialogue.LookAt = null;
    }
    
    public void LookAtTarget(Transform target)
    {
        cineMachineVirtualCameraDialogue.LookAt = target;
    }
    
    public void EnablePovCamera(bool b)
    {
        var povComponent = cineMachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (b)
        {
            povComponent.m_HorizontalAxis.m_MaxSpeed = _playerController.sensitivity;
            povComponent.m_VerticalAxis.m_MaxSpeed = _playerController.sensitivity;
        }
        else
        {
            cineMachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0f;
            cineMachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0f;
        }
    }

    public void HeadBob(float f)
    {
        _noise.m_FrequencyGain = f;
    }
}
