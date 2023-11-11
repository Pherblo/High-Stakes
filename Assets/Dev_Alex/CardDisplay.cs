using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("Card UI References")]
    [SerializeField] private Image _characterArt;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardTitle;
    [SerializeField] private Typewriter _cardDescription;
    [SerializeField] private Typewriter _choiceAText;
    [SerializeField] private Typewriter _choiceBText;

    [Header("stats  to be changed")]
    public Stats suspicion;
    public Stats faith;
    public Stats popularity;


    // Called by events.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
    {
        // Display texts.
        _cardName.text = cardToDisplay.AssociatedCharacter.Name;
        _cardTitle.text = cardToDisplay.AssociatedCharacter.Title;
        //_cardDescription.text = cardToDisplay.Description;
        //_choiceAText.text = cardToDisplay.DialogueA.DialogueText;
        //_choiceBText.text = cardToDisplay.DialogueB.DialogueText;
        _cardDescription.RunDialogue(cardToDisplay.Description);
        _choiceAText.RunDialogue(cardToDisplay.DialogueA.DialogueText);
        _choiceBText.RunDialogue(cardToDisplay.DialogueB.DialogueText);
        _characterArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;
        suspicion.changeValue(cardToDisplay.suspicionValue);
        faith.changeValue(cardToDisplay.faithValue);
        popularity.changeValue(cardToDisplay.popularityValue);

    }
}
