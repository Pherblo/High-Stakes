using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // Start is called before the first frame update
    private int currentValue;
    private int maxValue;
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
        return currentValue;
    }

    //takes a change value and adds it to current value, values should be entered as negative or positive.
    public void changeValue(int change)
    {
        currentValue += change; 
    }

    public bool triggerDeath()
    {
        if(currentValue >= maxValue)
        {
            return true;
        } else if (currentValue <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void updateDisplay()
    {

    }
}
