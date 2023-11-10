using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [Header("Card UI References")]
    [SerializeField] private SpriteRenderer _characterArtRenderer;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardDescription;
    [SerializeField] private TextMeshProUGUI _choiceAText;
    [SerializeField] private TextMeshProUGUI _choiceBText;

    // Called by events.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
    {
        // Grab the current cards dialogue A
        //cardDialogueA = cardEvent.DialogueA;
        // Grab the current cards dialogue B
        //cardDialogueB = cardEvent.DialogueB;

        // Setting the UI texts

        //art
        //characterArt = characterData.art;
       // characterArtSR.sprite = characterArt;
       

        // Set displayed card's texts.
        _cardName.text = cardToDisplay.AssociatedCharacter.Name;
        _cardDescription.text = cardToDisplay.Description;
        _choiceAText.text = cardToDisplay.DialogueA.DialogueText;
        _choiceBText.text = cardToDisplay.DialogueB.DialogueText;
        _characterArtRenderer.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;
    }
}
