using UnityEngine;

public class WebGLText : MonoBehaviour
{
    private void Awake()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            gameObject.SetActive(true);
        #else
            gameObject.SetActive(false);
        #endif
    }
}
