using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Digicode : MonoBehaviour
{
    [FormerlySerializedAs("progressionToUnlock")] [SerializeField] private Event eventToUnlock;
    private readonly List<DigicodeButton> _buttons = new List<DigicodeButton>();
    public void PressButton(DigicodeButton button)
    {
        if (_buttons.Contains(button))
            return;
        _buttons.Add(button);
    }
    
    public void CheckPassword()
    {
        if (_buttons.Count != 4)
        {
            Debug.Log("Wrong password");
            _buttons.Clear();
        }
        else
            UnlockDoor();
    }
    
    private void UnlockDoor()
    {
        eventToUnlock.SetProgressionStatus(true);
    }
}
