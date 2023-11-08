using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSwipeDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private Vector3 cardOriginPosition;
	[SerializeField] private float swipeDistance;
	private float cardSwipeThreshold = 200;

	[Header("** Console Debug Logs **")]
	[SerializeField] private bool debug_CurrentSwipeDistnace = false;
    [SerializeField] private bool debug_Dragging = false;


    // When you drag the card. Move its local x position, add the amount moved on x axis.
    public void OnDrag(PointerEventData eventData)
	{
		transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

        // ** Debugs **
        if (debug_CurrentSwipeDistnace) { Debug.Log("Current Swipe Distance" + Mathf.Abs(transform.localPosition.x - cardOriginPosition.x)); }
		if (debug_Dragging) { Debug.Log("Is dragging"); }
    }

    // When you first begin to drag the card, store its inital origin position;
    public void OnBeginDrag(PointerEventData eventData)
	{
		// Take the inital origin position and cache it into cardOriginPosition
		cardOriginPosition = transform.localPosition;

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
        }

		// ** Debugs **
        if (debug_Dragging) { Debug.Log("Finished drag"); }
    }
}
