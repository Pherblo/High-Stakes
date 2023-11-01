using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    [Header("Stat values: ")]
    private float _currentValue = 50;
    private float _maxValue = 100;

    [Header("Display settings: ")]
    public Image StatDisplay;
    public UnityEvent StatChange;

    void Start()
    {
        updateDisplay(); //set the display to show current value
    }

    void Update()
    {
        updateDisplay(); //update every frame to display current value
    }

    //returns the current value of stat
    public float getValue()
    {
        return _currentValue;
    }

    //takes a change value and adds it to current value, values should be entered as negative or positive.
    public void changeValue(int change)
    {
        _currentValue += change;
        StatChange.Invoke();
    }

    //method that returns a bool of true when _currentVaue is greater than/or equal to _maxValue or less than/or equal to 0, otherwise returns false
    public bool triggerDeath()
    {
        if(_currentValue >= _maxValue)
        {
            return true;
        } else if (_currentValue <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    //updates image fill amount to match current value.
    private void updateDisplay()
    {
        StatDisplay.fillAmount = _currentValue/100;
    }
}
