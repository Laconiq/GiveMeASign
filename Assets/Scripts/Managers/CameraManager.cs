using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cineMachineVirtualCamera;
    private Coroutine _changeFOVCoroutine;

    public void SetCameraFOV(float newFOV, float duration)
    {
        if (_changeFOVCoroutine != null)
            StopCoroutine(_changeFOVCoroutine);
        _changeFOVCoroutine = StartCoroutine(ChangeFOVCoroutine(newFOV, duration));
    }

    private IEnumerator ChangeFOVCoroutine(float targetFOV, float duration)
    {
        float time = 0;
        float startFOV = cineMachineVirtualCamera.m_Lens.FieldOfView;
        while (time < duration)
        {
            time += Time.deltaTime;
            float newFOV = Mathf.Lerp(startFOV, targetFOV, time / duration);
            cineMachineVirtualCamera.m_Lens.FieldOfView = newFOV;
            yield return null;
        }
        cineMachineVirtualCamera.m_Lens.FieldOfView = targetFOV;
    }
}