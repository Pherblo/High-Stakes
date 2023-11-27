using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
	private CardPositions cardState = CardPositions.Middle;
	public CardPositions CardState => cardState;
	private float keyboardInput;
    private bool isKeyboard;
    public bool IsKeyboard => isKeyboard;
    
    [SerializeField] private UnityEvent changeCardPosition;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        isKeyboard = false;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

        // Update is called once per frame
        void Update()
	{
        // Getting keyboard input
        keyboardInput = Input.GetAxisRaw("Horizontal");

        // Only first frame of key press
        if (Input.anyKeyDown)
        {
            isKeyboard = true;

            // Set cardState with keyboardInput
            CheckCardStates();
        }

        // Changes card position when cardState is not in the middle, keyboard is being used, and event is not null
        if (cardState != CardPositions.Middle && changeCardPosition != null && isKeyboard)
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
