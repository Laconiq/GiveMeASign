using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text sliderValue;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void ChangeSensitivity()
    {
        float sensitivity = slider.value;
        _playerController.sensitivity = sensitivity;
        sliderValue.text = sensitivity.ToString().Substring(0, 2);
    }
}
