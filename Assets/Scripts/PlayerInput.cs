using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour//, IDragHandler
{
	private CardPositions cardState = CardPositions.Middle;
	public CardPositions CardState => cardState;
	private float keyboardInput;

    private UnityEvent changeCardPosition;

	[Header("** Console Debug Logs **")]
	[SerializeField] private bool debug_CardState;
    [SerializeField] private bool debug_KeyboardInput;

    public enum CardPositions
	{
		LeftSwiped,
		Left,
		Middle,
		Right,
		RightSwiped
	}

	// Update is called once per frame
	void Update()
	{
        // Getting keyboard input
        keyboardInput = Input.GetAxisRaw("Horizontal");

        // Only first frame of key press
        if (Input.anyKeyDown)
        {
            // Set cardState with keyboardInput
            CheckCardStates();
        }

        if (cardState != CardPositions.Middle && changeCardPosition != null)
        {
            changeCardPosition.Invoke();
        }

        // ** Debugs **
		if(debug_CardState) { Debug.Log("Cards State: " + cardState); }
        if (debug_KeyboardInput) { Debug.Log("Keyboard Input: " + keyboardInput); }
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

            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.Right;
            }
        }

        // When card is in left position check if buttons are pressed and change to next position
        else if (cardState == CardPositions.Left)
        {
            if (keyboardInput == -1)
            {
                cardState = CardPositions.LeftSwiped;
            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.Middle;
            }
        }

        // When card is in right position check if buttons are pressed and change to next position
        else if (cardState == CardPositions.Right)
        {
            if (keyboardInput == -1)
            {
                cardState = CardPositions.Middle;
            }
            else if (keyboardInput == 1)
            {
                cardState = CardPositions.RightSwiped;
            }
        }
    }
}
