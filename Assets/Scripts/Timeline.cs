using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
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
    private int cardsPassedTime = 0;
    private int totalCardsPassed;
    private int maxCards;
    private int cardsTillNextDay = 3;
    private int cardsTillNextTime = 1;

    [Header("Day variables: ")]
    private int daysPassed = 0;
    private TimeOfDay currentTime;

    private MoonCycle currentMoonCycle;
 
    const int numOfCycles = 8;
    const int numOfTimes = 2;

    int timePassed = 0;

    public GameObject moonPhasesArt;
    public GameObject arrowArt;

    public float change;


    // Start is called before the first frame update
    void Start()
    {
        //set initial values
        currentMoonCycle = MoonCycle.WaxingGibbous; //start on mooncycle after full moon
        currentTime = TimeOfDay.Morning; //start in morning
        change = 45;
    }

    // Update is called once per frame
    void Update()
    {
        if (cardsPassedTime >= cardsTillNextTime)
        {
            changeTime();
        }
        if (cardsPassedToday >= cardsTillNextDay)
        {
            changeDay();
            change += 45;
        }

        updateDisplay();
        print(totalCardsPassed);
        print("time: " + currentTime.ToString());
        print("moon cycle: " + currentMoonCycle.ToString());
    }

    private void changeDay()
    {
            daysPassed++;
            int moonCycle = (int)currentMoonCycle; //cast current mooncyle to int
        if(moonCycle < numOfCycles-1)
        {
            currentMoonCycle = (MoonCycle)moonCycle + 1; //make new mooncycle current + 1
        }
        else
        {
            currentMoonCycle = (MoonCycle) 0;
        }
        Debug.Log("change day");
        cardsPassedToday = 0;
    }

    private void changeTime()
    {
        Debug.Log("change time");
   

        int time = (int)currentTime; //cast current mooncyle to int
        if (time < numOfTimes )
        {
            currentTime = (TimeOfDay)currentTime + 1; //make new mooncycle current + 1
        }
        else
        {
            currentTime = (TimeOfDay)0;
        }
        cardsPassedTime = 0;
    }


    public void updateDisplay()
    {

        //stub to hook up to animation
         Debug.Log("time: " + currentTime.ToString());
        // Debug.Log(cardsPassedToday.ToString());
         Debug.Log("moon cycle: " + currentMoonCycle.ToString());

        //will change the display
        // moonPhasesArt.transform.Rotate(0, 0, 45, Space.World);

        float speed = 2f;
        Quaternion targetRotation = Quaternion.Euler(0, 0 , moonPhasesArt.transform.rotation.z + change);
        Quaternion startRotation = moonPhasesArt.transform.rotation;
        moonPhasesArt.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, speed * Time.deltaTime);
        
    }

 
    //to be called by onCardPicked event in deck
    public void updateCardsPassed()
    {
        cardsPassedToday++;
        cardsPassedTime++;
        totalCardsPassed++;
        print("cardPicked");
    }

    public void TriggerGodEvent()
    {
        if(currentMoonCycle == MoonCycle.FullMoon)
        {

        }
    }
   
}
