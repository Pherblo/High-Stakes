using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	[Header("Card UI Text References")]
	[SerializeField] private Image _characterArt;
	[SerializeField] private TextMeshProUGUI _cardName;
	[SerializeField] private TextMeshProUGUI _cardTitle;
	[SerializeField] private Typewriter _cardDescription;
	[SerializeField] private Typewriter _dialogueTextA;
    [SerializeField] private Typewriter _dialogueTextB;

    private CardEvent currentCardEvent;

	[Header("Stats To Be Changed")]
	public Stats suspicion;
	public Stats faith;
	public Stats popularity;

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

	public void ClearDialogues()
	{
		// Clearing the text box
        _dialogueTextA.RunDialogue("");
        // Turning off dialogue background when not in use
        _dialogueTextA.gameObject.GetComponentInParent<Image>().enabled = false;
    }

    // Subscribed to Deck's OnCardPicked event.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
	{
		// Updating UI texts and art.
		_cardName.text = cardToDisplay.AssociatedCharacter.Name;
		_cardTitle.text = cardToDisplay.AssociatedCharacter.Title;
        _characterArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;

        // Update typewriters.
        _cardDescription.RunDialogue(cardToDisplay.Description);
		_dialogueTextA.RunDialogue(cardToDisplay.DialogueA.DialogueText);
        _dialogueTextB.RunDialogue(cardToDisplay.DialogueB.DialogueText);

		// Updating Stat bottles from last card
		suspicion.changeValue(cardToDisplay.suspicionValue);
		faith.changeValue(cardToDisplay.faithValue);
		popularity.changeValue(cardToDisplay.popularityValue);

		// Caching the cardToDisplay
		currentCardEvent = cardToDisplay;
	}
}
