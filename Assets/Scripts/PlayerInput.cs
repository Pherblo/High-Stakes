using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour, IDragHandler
{
	private CardPositions cardState;
	public CardPositions CardState => cardState;
	private float currentMousePosition;
	private float keyboardInput;
	private float cardSwipeThreshold = 200;

	[Header("** Console Debug Logs **")]
	[SerializeField] private bool Debug_CurrentMousePosition;
    [SerializeField] private bool Debug_CardState;


    public enum CardPositions
	{
		LeftSwiped,
		Left,
		Middle,
		Right,
		RightSwiped
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		// Getting keyboard input
		keyboardInput = Input.GetAxisRaw("Horizontal");

        CheckCardStates();

        // Updating card states from currentMousePositions
        if (currentMousePosition == 0)
		{
			cardState = CardPositions.Middle;
		}
        else if (currentMousePosition < 0 && currentMousePosition < -cardSwipeThreshold)
        {
            cardState = CardPositions.LeftSwiped;
        }
        else if (currentMousePosition > 0 && currentMousePosition > cardSwipeThreshold)
        {
            cardState = CardPositions.RightSwiped;
        }
        else if (currentMousePosition < 0)
		{
			cardState = CardPositions.Left;
		}
        else if (currentMousePosition > 0)
        {
            cardState = CardPositions.Right;
        }

        // When mouse is reeleased reset the mouses positon
        if (Input.GetMouseButtonUp(0))
		{
			ResetMousePosition();
		}

		if(Debug_CardState) { Debug.Log("Cards State: " + cardState); }
        if (Debug_CurrentMousePosition) { Debug.Log("Current Mouse Position: " + currentMousePosition); }
    }

	// When the players mouse click is moving
	public void OnDrag(PointerEventData eventData)
	{
		// Update current mouse position to reflect the amount dragged
		currentMousePosition += eventData.delta.x;

		if (Debug_CurrentMousePosition) { Debug.Log("Current Mouse Position: " + currentMousePosition); }
	}

	// Check card states with keyboard inputs
	private void CheckCardStates()
	{
        // When card is in middle check if buttons are pressed and change to next position
        if (cardState == CardPositions.Middle)
        {
            if (keyboardInput == -1)
            {
                cardState = CardPositions.Left;

                currentMousePosition -= 1; // temp
            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.Right;

                currentMousePosition += 1; // temp
            }
        }

        // When card is in left position check if buttons are pressed and change to next position
        if (cardState == CardPositions.Left)
        {
            if (keyboardInput == -1)
            {
                cardState = CardPositions.LeftSwiped;

                currentMousePosition = -cardSwipeThreshold; // temp
            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.Middle;

                currentMousePosition = 0; // temp
            }
        }

        // When card is in right position check if buttons are pressed and change to next position
        if (cardState == CardPositions.Right)
        {
            if (keyboardInput == -1)
            {
                cardState = CardPositions.Middle;

                currentMousePosition = 0; // temp
            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.RightSwiped;

                currentMousePosition = cardSwipeThreshold; // temp
            }
        }
    }

	public void ResetMousePosition()
	{
		currentMousePosition = 0;
	}
}
