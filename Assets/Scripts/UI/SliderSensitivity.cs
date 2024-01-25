using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderSensitivity : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text sliderValue;
    private PlayerController _playerController;

    public void Initialize()
    {
        _playerController = GameManager.Instance.playerController;
    }

    public void ChangeSensitivity()
    {
        float sensitivity = slider.value;
        _playerController.sensitivity = sensitivity;
        sliderValue.text = sensitivity.ToString(CultureInfo.InvariantCulture)[..2];
    }
}
