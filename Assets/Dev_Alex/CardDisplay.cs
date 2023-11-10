using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text characterDialogue;
    [SerializeField] private TMP_Text dialogueAText;
    [SerializeField] private TMP_Text dialogueBText;
    [SerializeField] private Sprite characterArt;

    private CardDialogue cardDialogueA;
    private CardDialogue cardDialogueB;

    private CharacterData characterData;
    private CardEvent cardEvent;

    public SpriteRenderer characterArtSR; //the Sprite renderer in CharacterArt


    // Start is called before the first frame update
    void Start()
    {
        assignData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void assignData()
    {
        //information
        cardEvent = gameObject.GetComponent<CardEvent>();
        characterData = gameObject.GetComponent<CharacterData>();
        cardDialogueA = cardEvent.DialogueA;
        cardDialogueB = cardEvent.DialogueB;

        //art
        //characterArt = characterData.art;
       // characterArtSR.sprite = characterArt;
       

        // Setting Text Meshes
        cardName.text = "<Incr> " + characterData.Name + "</Incr>";
        characterDialogue.text = cardEvent.Description;
    }
}
