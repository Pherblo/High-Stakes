using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [Header("Card UI")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text characterDialogue;
    [SerializeField] private TMP_Text dialogueAText;
    [SerializeField] private TMP_Text dialogueBText;
    [SerializeField] private Sprite characterArt;

    private CardDialogue cardDialogueA;
    private CardDialogue cardDialogueB;

    private CharacterData characterData;
    [SerializeField]private CardEvent cardEvent;

    public SpriteRenderer characterArtSR; //the Sprite renderer in CharacterArt

    [Header("Deck")]
    [SerializeField] Deck deck;

    [SerializeField] private CardEvent decksPickedCard; 

    // Start is called before the first frame update
    void Start()
    {
        assignData();
    }

    // Update is called once per frame
    void Update()
    {
        assignData();
    }

    private void assignData()
    {
        // Grab the current card event
        cardEvent = deck.CurrentCardDisplayed;

        // Grab the current cards associated characterdata
        //characterData = cardEvent.AssociatedCharacter;
        // Grab the current cards dialogue A
        //cardDialogueA = cardEvent.DialogueA;
        // Grab the current cards dialogue B
        //cardDialogueB = cardEvent.DialogueB;

        // Setting the UI texts

        //art
        //characterArt = characterData.art;
       // characterArtSR.sprite = characterArt;
       

        // Setting Text Meshes
        cardName.text = "<Incr> " + cardEvent.AssociatedCharacter.Name + "</Incr>";
        characterDialogue.text = cardEvent.Description;
    }
}
