using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    private int _currentValue;
    private int _maxValue;
    public GameObject statDisplay;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //returns the current value of stat
    public int getValue()
    {
        return _currentValue;
    }

    //takes a change value and adds it to current value, values should be entered as negative or positive.
    public void changeValue(int change)
    {
        _currentValue += change; 
    }

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

    private void updateDisplay()
    {
        if(statDisplay.GetComponent<SpriteRenderer>() != null)
        {
            
        }
    }
}
