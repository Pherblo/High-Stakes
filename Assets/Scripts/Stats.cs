using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    private float _currentValue = 50;
    private float _maxValue = 100;
    public Image StatDisplay;

    void Start()
    {
        updateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        updateDisplay();
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
