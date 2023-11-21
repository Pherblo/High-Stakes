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
    public bool runningTutorial;

    public void Awake()
    {
        runningTutorial = true;
    }
    void Start()
    {
        Deck.GetComponent<Deck>()._database = TutorialDatabase;
    }

    // Update is called once per frame
    void Update()
    {
        if(numOfDeaths > 0)
        {
            Deck.GetComponent<Deck>()._database = CharacterDatabase;
           // runningTutorial = false;
        } else
        {
            runningTutorial = true;
        }
    }


    //to be called by OnDeathEvent in Stats
    public void addDeath()
    {
        numOfDeaths++;
    }

}
