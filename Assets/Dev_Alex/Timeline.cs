using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MoonCycle {
    ThirdQuarter,
    WaningGibbous,
    FullMoon,
    WaxingGibbous,
    FirstQuarter,
    WaxingCrescent,
    NewMoon,
    WaningCrescent
}

public enum TimeOfDay
{
    Morning,
    Afternoon,
    Night
}

public class Timeline : MonoBehaviour
{
    [Header("Card related settings: ")]
    private int cardsPassedToday = 0;
    private int totalCardsPassed;
    private int maxCards;
    private int cardsTillNextDay = 4;
    private int cardsTillNextTime = 2;

    [Header("Day variables: ")]
    private int daysPassed = 0;
    private TimeOfDay currentTime;

    private MoonCycle currentMoonCycle;

    // Start is called before the first frame update
    void Start()
    {
        //set initial values
        currentMoonCycle = MoonCycle.WaxingGibbous; //start on mooncycle after full moon
        currentTime = TimeOfDay.Morning; //start in morning

    }

    // Update is called once per frame
    void Update()
    {
        if(cardsPassedToday > cardsTillNextDay)
        {
            changeDay();
        }

        if(cardsPassedToday> cardsTillNextTime)
        {
            changeTime();
        }
    }

    private void changeDay()
    {
            daysPassed++;
            int moonCycle = (int)currentMoonCycle; //cast current mooncyle to int
            currentMoonCycle = (MoonCycle)  moonCycle + 1; //make new mooncycle current + 1
            resetCards();
    }

    private void changeTime()
    {
        int time = (int)currentTime;
        currentTime = (TimeOfDay)time + 1;
    }

    private void resetCards()
    {
        cardsPassedToday = 0;
    }

    private void updateDisplay()
    {
        //stub to hook up to animation
    }


}
