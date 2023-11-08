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

	// When you drag the card. Move its local x position, add the amount moved on x axis.
	public void OnDrag(PointerEventData eventData)
	{
		transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
	}

	// When you first begin to drag the card, store its inital origin position;
	public void OnBeginDrag(PointerEventData eventData)
	{
		// Take the inital origin position and cache it into cardOriginPosition
		cardOriginPosition = transform.localPosition;
		Debug.Log("Starting Drag");
	}

	// When you release the drag.
	public void OnEndDrag(PointerEventData eventData)
	{
		// Take the new position of card and take out the origin position. To find the swipe distance
		// Make it an absolute number to always be a positive magnitude
		swipeDistance = Mathf.Abs(transform.localPosition.x - cardOriginPosition.x);

		// If not past a certain threshold.
		transform.localPosition = cardOriginPosition;
	}
}
