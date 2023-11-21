using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int numOfDeaths = 0;
    public GameObject Deck;
    public GameObject TutorialDatabase;
    public GameObject CharacterDatabase;
    public bool runningTutorial = true;

    public void Awake()
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

}
