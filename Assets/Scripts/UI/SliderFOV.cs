using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderFOV : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCameraDialogue;
    [SerializeField] private TMP_Text sliderValue;
    public void ChangeFOV()
    {
        float fov = slider.value;
        cineMachineVirtualCamera.m_Lens.FieldOfView = fov;
        cineMachineVirtualCameraDialogue.m_Lens.FieldOfView = fov;
        sliderValue.text = fov.ToString().Substring(0, 2);
    }
}
