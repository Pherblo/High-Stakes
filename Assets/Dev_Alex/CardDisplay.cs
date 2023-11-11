using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    [Header("Card UI References")]
    [SerializeField] private SpriteRenderer _characterArtRenderer;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private Typewriter _cardDescription;
    [SerializeField] private Typewriter _choiceAText;
    [SerializeField] private Typewriter _choiceBText;

    // Called by events.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
    {
        // Display texts.
        _cardName.text = cardToDisplay.AssociatedCharacter.Name;
        //_cardDescription.text = cardToDisplay.Description;
        //_choiceAText.text = cardToDisplay.DialogueA.DialogueText;
        //_choiceBText.text = cardToDisplay.DialogueB.DialogueText;
        _cardDescription.RunDialogue(cardToDisplay.Description);
        _choiceAText.RunDialogue(cardToDisplay.DialogueA.DialogueText);
        _choiceBText.RunDialogue(cardToDisplay.DialogueB.DialogueText);
        _characterArtRenderer.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;
    }
}
