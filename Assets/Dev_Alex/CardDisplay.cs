using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	public UnityEvent OnDescriptionDisplayStart;
    public UnityEvent OnDescriptionDisplayEnd;

    [Header("Card UI Text References")]
	[SerializeField] private Image _cardArt;
	[SerializeField] private TextMeshProUGUI _cardName;
	[SerializeField] private TextMeshProUGUI _cardTitle;
	[SerializeField] private Typewriter _cardDescription;
	[SerializeField] private Typewriter _dialogueTextA;
    [SerializeField] private Typewriter _dialogueTextB;

    private CardEvent _currentCardEvent;

	[Header("Stats To Be Changed")]
	public Stats suspicion;
	public Stats faith;
	public Stats popularity;

    private void Awake()
    {
        _cardDescription.OnTextGenerationFinished += SetChoices;
    }

    // Changes dialogue depending on the swipe
    // Can be changed later to have a fade effect
    public void ToggleDialogues(CardSwipeDrag thisCardSwipe)
	{
		/*
		// Turning dialogue background back on
        _dialogueTextA.gameObject.GetComponentInParent<Image>().enabled = true;

		// Check swipe condition and change to the corresponding dialogue
		if(!thisCardSwipe.SwipeIsRight)
		{
			_dialogueTextA.RunDialogue(currentCardEvent.DialogueA.DialogueText);
		}
		else if (thisCardSwipe.SwipeIsRight)
		{
            _dialogueTextA.RunDialogue(currentCardEvent.DialogueB.DialogueText);
        }
		*/
    }

	/*public void ClearDialogues()
	{
		// Clearing the text box
        _dialogueTextA.RunDialogue("");
        // Turning off dialogue background when not in use
        _dialogueTextA.gameObject.GetComponentInParent<Image>().enabled = false;
    }*/

	public void UpdateReferences(CardEvent newCardEvent, TextMeshProUGUI newCardName, TextMeshProUGUI newCardTitle, Image newCardArt)
	{
		_currentCardEvent = newCardEvent;
		_cardName = newCardName;
		_cardTitle = newCardTitle;
		_cardArt = newCardArt;
	}

    // Subscribed to Deck's OnCardPicked event.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
	{
        // Cache the card.
        _currentCardEvent = cardToDisplay;

        // Updating UI texts and art on the card.
        _cardName.text = cardToDisplay.AssociatedCharacter.Name;
		_cardTitle.text = cardToDisplay.AssociatedCharacter.Title;
        _cardArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;

		// Update typewriters.
		//_dialogueTextA.ClearText();
        //_dialogueTextB.ClearText();
        _dialogueTextA.SetFillerText(cardToDisplay.DialogueA.DialogueText);
        _dialogueTextB.SetFillerText(cardToDisplay.DialogueB.DialogueText);
        SetDescriptionText(cardToDisplay.Description);

		OnDescriptionDisplayStart?.Invoke();

        //_cardDescription.RunDialogue(cardToDisplay.Description);
		//_dialogueTextA.RunDialogue(cardToDisplay.DialogueA.DialogueText);
        //_dialogueTextB.RunDialogue(cardToDisplay.DialogueB.DialogueText);

		// Updating Stat bottles from last card
		suspicion.changeValue(cardToDisplay.suspicionValue);
		faith.changeValue(cardToDisplay.faithValue);
		popularity.changeValue(cardToDisplay.popularityValue);
	}

    public void ExitDisplay()
    {
        // Update typewriters materials.
        _cardDescription.SetAllTexts(_currentCardEvent.Description);
        _dialogueTextA.SetAllTexts(_currentCardEvent.DialogueA.DialogueText);
        _dialogueTextB.SetAllTexts(_currentCardEvent.DialogueB.DialogueText);

        _cardDescription.CloseDialogue();
        _dialogueTextA.CloseDialogue();
        _dialogueTextB.CloseDialogue();
    }

	public void ClearDisplay()
	{
        _cardDescription.ClearText();
        _dialogueTextA.ClearText();
        _dialogueTextB.ClearText();
    }

    private void SetDescriptionText(string text)
	{
		_cardDescription.SetFillerText(text);
        _cardDescription.RunDialogue(text);
    }

	// Called by card description event after it finishes generating its text.
	private void SetChoices()
	{
		OnDescriptionDisplayEnd?.Invoke();
        _dialogueTextA.RunDialogue(_currentCardEvent.DialogueA.DialogueText);
        _dialogueTextB.RunDialogue(_currentCardEvent.DialogueB.DialogueText);
    }
}
