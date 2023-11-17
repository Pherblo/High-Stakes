using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class CardSwipeDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Vector3 cardOriginPosition = new Vector3(0, 35, 0);
	private float swipeDistance;
	private float cardSwipeThreshold = 200;
	[SerializeField] private bool swipeIsRight = false;
	private bool cachedSwipe = false;
	private Image thisCardArt;

	[Header("Settings")]
	[SerializeField, Range(0.001f, 1)] private float rotateAmount;
    [SerializeField, Range(1, 10)] private float swipeAmount;

    [Header("When card is swiped run")]
    [SerializeField] private UnityEvent discardCard; // Play discard animation while card is transparent

	[SerializeField] private UnityEvent dragIsFinished; // Clear highlighted objects when not dragging

	[Header("When card is being dragged")]
	[SerializeField] private UnityEvent updateDialogueOptionsUI;
	public bool SwipeIsRight => swipeIsRight;

	[Header("** Console Debug Logs **")]
	[SerializeField] private bool debug_CurrentSwipeDistnace = false;
    [SerializeField] private bool debug_Dragging = false;
	[SerializeField] private bool debug_SwipeDirection = false;

    private void Start()
    {
		thisCardArt = GetComponent<Image>();
		cardOriginPosition = thisCardArt.transform.localPosition;
    }

    // When you drag the card. Move its local x position, add the amount moved on x axis.
    public void OnDrag(PointerEventData eventData)
	{
		transform.localPosition = new Vector2(transform.localPosition.x + (eventData.delta.x * swipeAmount), transform.localPosition.y);

		// Rotate by a rotateAmount when dragging 
		transform.Rotate(0, eventData.delta.x * rotateAmount, 0);

		// If the x of moved card is more than the origin of the card then you are swiping right.
        if (transform.localPosition.x > cardOriginPosition.x)
        {
            swipeIsRight = true;
        }
		else
		{
			swipeIsRight = false;
		}

		// Change the dialogue options UI from card display while dragging
		if(cachedSwipe != swipeIsRight)
		{
            updateDialogueOptionsUI.Invoke();
			cachedSwipe = swipeIsRight;
        }

        // ** Debugs **
        if (debug_CurrentSwipeDistnace) { Debug.Log("Current Swipe Distance" + Mathf.Abs(transform.localPosition.x - cardOriginPosition.x)); }
		if (debug_Dragging) { Debug.Log("Is dragging"); }
		if (debug_SwipeDirection) { Debug.Log("Swiping right is: " + swipeIsRight); }
    }

    // When you first begin to drag the card, store its inital origin position;
    public void OnBeginDrag(PointerEventData eventData)
	{
		// Take the inital origin position and cache it into cardOriginPosition
		// cardOriginPosition = transform.localPosition;

        // ** Debugs **
        if (debug_Dragging) { Debug.Log("Started to drag"); }
    }

    // When you release the drag.
    public void OnEndDrag(PointerEventData eventData)
	{
		// Take the new position of card and take out the origin position. To find the swipe distance
		// Make it an absolute number to always be a positive magnitude
		swipeDistance = Mathf.Abs(transform.localPosition.x - cardOriginPosition.x);

		// Only revert the cards position when within the threshold
		if (swipeDistance < cardSwipeThreshold)
		{
            transform.localPosition = cardOriginPosition;
			transform.localRotation = Quaternion.identity; // Reset rotation
        }
		else
		{
            // Dont show card anymore
            // Discard animation
            // Discard card and change card information
            // When doing so, pick a new card from deck class
            discardCard.Invoke();
            // Draw new card
            thisCardArt.color = Color.clear;
			//Invoke("DrawNewCard", 5);
			DrawNewCard();
        }

        dragIsFinished.Invoke();

        // ** Debugs **
        if (debug_Dragging) { Debug.Log("Finished drag"); }
    }

	public void DrawNewCard()
	{
		// From card displayer.
		// From animation

		// For now demo logic
		transform.localPosition = cardOriginPosition;
        transform.localRotation = Quaternion.identity; // Reset rotation
        thisCardArt.color = Color.white;
	}
}
