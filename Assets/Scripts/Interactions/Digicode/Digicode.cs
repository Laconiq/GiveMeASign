using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Digicode : MonoBehaviour
{
    [FormerlySerializedAs("progressionToUnlock")] [SerializeField] private Event eventToUnlock;
    private readonly List<DigicodeButton> _buttons = new List<DigicodeButton>();
    [SerializeField] private TMP_Text passwordText;
    [SerializeField] private AK.Wwise.Event wrongPasswordSound;
    [SerializeField] private AK.Wwise.Event successSound;
    
    public void PressButton(DigicodeButton button)
    {
        if (_buttons.Count == 6)
            return;
        _buttons.Add(button);
        passwordText.text += "*";
    }
    
    public void CheckPassword()
    {
        if (_buttons.Count != 4)
            OnFail();
        else
            OnSuccess();
    }
    
    private void OnSuccess()
    {
        Debug.Log("Success");
        eventToUnlock.SetProgressionStatus(true);
        passwordText.text = "";
        _buttons.Clear();
        successSound.Post(gameObject);
    }
    
    private void OnFail()
    {
        Debug.Log("Wrong password");
        passwordText.text = "";
        _buttons.Clear();
        wrongPasswordSound.Post(gameObject);
    }
}
