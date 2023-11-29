using UnityEngine;

// Ajusting and setting card statuses
public class CardSettings : MonoBehaviour
{
	[Header("List of All Card Settings")]
	[SerializeField] private OnClickSwitch[] cards;

	[Header("Chosen card's cached references")]
	private OnClickSwitch chosenCard;
	private Animator chosenCardAnimator;

	public void StopClickingOtherCards(OnClickSwitch thisCard)
	{
		// Caching this cards components
		chosenCard = thisCard;
		chosenCardAnimator = chosenCard.GetComponent<Animator>();

		// Turning off the clickablity of each card
		foreach (var card in cards)
		{
			card.enabled = false;
		}

		// Turn on CardIsChosen after the animation is played and turn clickability back on
		Invoke("DelayCardIsChosen", 1f);
	}

	public void StartClickingOtherCards()
	{
		// Turning the bool of this cards animator to false, is not choosing it anymore
		chosenCardAnimator.SetBool("CardIsChosen", false);

		// Turning on the clickablity of each other cards
		foreach (var card in cards)
		{
			if(card != chosenCard)
			{
				card.enabled = true;
			}
		}
	}

	// Turn CardIsChosen to true after a second so the animation can run before stopping
	private void DelayCardIsChosen()
	{
		chosenCardAnimator.SetBool("CardIsChosen", true);
		chosenCard.enabled = true;
	}
}
