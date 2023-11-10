using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [Header("Card UI")]
    [SerializeField] private TMP_Text _cardName;
    [SerializeField] private TMP_Text _characterDialogue;
    [SerializeField] private TMP_Text _dialogueAText;
    [SerializeField] private TMP_Text _dialogueBText;
    [SerializeField] private Sprite _characterArt;

    public SpriteRenderer characterArtSR; //the Sprite renderer in CharacterArt

    [Header("Deck")]
    [SerializeField] Deck deck;

    // Update is called once per frame
    void Update()
    {
        UpdateCardDisplay();
    }

    private void UpdateCardDisplay()
    {
        // Grab the current card event
        CardEvent cardEvent = deck.CurrentCardDisplayed;

        // Grab the current cards dialogue A
        //cardDialogueA = cardEvent.DialogueA;
        // Grab the current cards dialogue B
        //cardDialogueB = cardEvent.DialogueB;

        // Setting the UI texts

        //art
        //characterArt = characterData.art;
       // characterArtSR.sprite = characterArt;
       

        // Setting Text Meshes
        _cardName.text = cardEvent.AssociatedCharacter.Name;
        _characterDialogue.text = cardEvent.Description;
    }
}
