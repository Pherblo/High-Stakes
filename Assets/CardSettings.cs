using UnityEngine;

// Ajusting and setting card statuses
public class CardSettings : MonoBehaviour
{
	[Header("List of All Card Settings")]
	[SerializeField] private OnClickSwitch[] cards;

	[Header("Chosen card's cached references")]
	private OnClickSwitch chosenCard;
	private Animator chosenCardAnimator;

    [Header("** Console Debug Logs **")]
    [SerializeField] private bool debug_WhereWhenCalled;

    private void Start()
    {
        if (cards == null)
		{
			Debug.LogWarning(gameObject.name + "'s 'cards' are null. Please enter in setting cards into list.");
		}
    }

    public void StopClickingOtherCards(OnClickSwitch thisCard)
	{
		// Null reference checking
		if(cards != null)
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

        // ** Debugs **
        if (debug_WhereWhenCalled) { Debug.Log("StopClickingOtherCards() was called from" + chosenCard.gameObject.name); }
	}

    public void StartClickingOtherCards()
	{
        // Null reference checking
        if (cards != null && chosenCard != null)
		{
            // Turning the bool of this cards animator to false, is not choosing it anymore
            chosenCardAnimator.SetBool("CardIsChosen", false);

            // Turning on the clickablity of each card
            foreach (var card in cards)
            {
                card.enabled = true;
            }
        }

        // ** Debugs **
        if (debug_WhereWhenCalled) { Debug.Log("StartClickingOtherCards() was called from" + chosenCard.gameObject.name); }
    }

	// Turn CardIsChosen to true after a second so the animation can run before stopping
	private void DelayCardIsChosen()
	{
		chosenCardAnimator.SetBool("CardIsChosen", true);

        // Only turn clicking back on auto after 1 second if it is not the settings option
        if (!chosenCard.GetComponent<StartMenuAnimations>().IsSettings)
        {
            chosenCard.enabled = true;
        }
    }
}
