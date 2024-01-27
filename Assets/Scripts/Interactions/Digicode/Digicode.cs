using System.Collections.Generic;
using UnityEngine;

public class Digicode : MonoBehaviour
{
    [SerializeField] private Progression progressionToUnlock;
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
        progressionToUnlock.SetProgressionStatus(true);
    }
}
