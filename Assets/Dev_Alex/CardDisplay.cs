using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	[Header("Card UI Text References")]
	[SerializeField] private Image characterArt;
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI cardTitle;
	[SerializeField] private Typewriter cardDescription;
	[SerializeField] private Typewriter dialogueText;

    private CardEvent currentCardEvent;

	[Header("Stats To Be Changed")]
	public Stats suspicion;
	public Stats faith;
	public Stats popularity;

	// Changes dialogue depending on the swipe
	// Can be changed later to have a fade effect
	public void ToggleDialogues(CardSwipeDrag thisCardSwipe)
	{
		// Turning dialogue background back on
        dialogueText.gameObject.GetComponentInParent<Image>().enabled = true;

		// Check swipe condition and change to the corresponding dialogue
		if(!thisCardSwipe.SwipeIsRight)
		{
			dialogueText.RunDialogue(currentCardEvent.DialogueA.DialogueText);
		}
		else if (thisCardSwipe.SwipeIsRight)
		{
            dialogueText.RunDialogue(currentCardEvent.DialogueB.DialogueText);
        }
    }

	public void ClearDialogues()
	{
		// Clearing the text box
        dialogueText.RunDialogue("");
        // Turning off dialogue background when not in use
        dialogueText.gameObject.GetComponentInParent<Image>().enabled = false;
    }

    // Called from Deck OnCardPicked
    // Updating UI elements on Card Canvas & Dialogue Canvas
    public void UpdateCardDisplay(CardEvent cardToDisplay)
	{
		// Updating Card Canvas UI texts to card events information
		cardName.text = cardToDisplay.AssociatedCharacter.Name;
		cardTitle.text = cardToDisplay.AssociatedCharacter.Title;

        // Updating Dialogue canvas UI texts to card events information
        cardDescription.RunDialogue(cardToDisplay.Description);

        // Updating Card Canvas UI image to card events sprites
        characterArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;

		// Updating Stat bottles from last card
		suspicion.changeValue(cardToDisplay.suspicionValue);
		faith.changeValue(cardToDisplay.faithValue);
		popularity.changeValue(cardToDisplay.popularityValue);

		// Caching the cardToDisplay
		currentCardEvent = cardToDisplay;
	}
}
