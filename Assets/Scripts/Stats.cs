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
    public float minY = 0.8f;
    public float maxY = 2.8f;
    public float dangerThreshold = 75;

    [Header("References")]
    public GameObject potionMask;
    public ParticleSystem bubblePassive;
    public ParticleSystem bubbleBoil;
    public ParticleSystem bubbleBurst;
    public ParticleSystem overflow;
    public ParticleSystem shatter;

    [Space]
    public UnityEvent OnDeath;

    private void Start()
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

        Mathf.Clamp(_currentValue, 0, _maxValue);

        updateDisplay(); //set the display to show current value
        bubbleBurst.Play();
    }

    //check if player has died, and play corresponding particle animations. if dead, call OnDeath() for other scripts to listen to
    private void checkForDeath()
    {
        if(_currentValue >= _maxValue)
        {
            overflow.Play();
        } else if (_currentValue <= 0)
        {
            shatter.Play();
        } else
        {
            return;
        }

        OnDeath.Invoke();
    }

    //updates image fill amount to match current value.
    private void updateDisplay()
    {
        if(_currentValue >= dangerThreshold)
        {
            bubbleBoil.Play();
        }
        else
            bubbleBoil.Stop();

        Vector3 pos = potionMask.transform.localPosition;
        pos.y = (_currentValue / _maxValue) * (maxY - minY) + minY;

        potionMask.transform.localPosition = pos;

        checkForDeath();
    }
}
