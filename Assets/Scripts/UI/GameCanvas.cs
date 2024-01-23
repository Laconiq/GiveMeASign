using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Image boostBar;
    
    public void UpdateBoostBar(float boostValue)
    {
        boostBar.fillAmount = boostValue;
    }
}
