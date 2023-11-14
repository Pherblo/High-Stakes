using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	[Header("Card UI References")]
	[SerializeField] private Image characterArt;
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI cardTitle;
	[SerializeField] private Typewriter cardDescription;
	[SerializeField] private Typewriter choiceAText;
	[SerializeField] private Typewriter choiceBText;

	[Header("Stats To Be Changed")]
	public Stats suspicion;
	public Stats faith;
	public Stats popularity;

	public void ToggleDialogues(CardSwipeDrag thisCardSwipe)
	{
		if(!thisCardSwipe.SwipeIsRight)
		{
			choiceAText.gameObject.SetActive(true);
            choiceBText.gameObject.SetActive(false);
            Debug.Log("Swipe is not right");
		}
		else if (thisCardSwipe.SwipeIsRight)
		{
            choiceBText.gameObject.SetActive(true);
            choiceAText.gameObject.SetActive(false);
            Debug.Log("Swipe is Right");
        }
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
		//choiceAText.RunDialogue(cardToDisplay.DialogueA.DialogueText);
		//choiceBText.RunDialogue(cardToDisplay.DialogueB.DialogueText);

        // Updating Card Canvas UI image to card events sprites
        characterArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;

		// Updating Stat bottles from last card
		suspicion.changeValue(cardToDisplay.suspicionValue);
		faith.changeValue(cardToDisplay.faithValue);
		popularity.changeValue(cardToDisplay.popularityValue);

	}
}
