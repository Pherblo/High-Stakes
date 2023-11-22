using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References: ")]
    public GameObject Deck;
    public GameObject TutorialDatabase;
    public GameObject CharacterDatabase;

    [Header("Variables for game state: ")]
    private int numOfDeaths = 0;
    private bool runningTutorial = true;

    void Awake()
    {
        if (runningTutorial)
        {
            Deck.GetComponent<Deck>()._database = TutorialDatabase;
        }
        else
        {
            Deck.GetComponent<Deck>()._database = CharacterDatabase;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runningTutorial)
        {
            Deck.GetComponent<Deck>()._database = TutorialDatabase;

        }
        else
        {
            Deck.GetComponent<Deck>()._database = CharacterDatabase;
        }


    }


    //to be called by OnDeathEvent in Stats
    public void addDeath()
    {
        numOfDeaths++;
    }

    public void ToggleTutorial(bool status)
    {
        runningTutorial = status;
    }

    public bool getTutorialStatus()
    {
        return runningTutorial;
    }

}
