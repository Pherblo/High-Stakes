using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour//, IDragHandler
{
	private CardPositions cardState;
	public CardPositions CardState => cardState;
	private float keyboardInput;

	[Header("** Console Debug Logs **")]
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

		if(Debug_CardState) { Debug.Log("Cards State: " + cardState); }
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
        if (cardState == CardPositions.Left)
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
        if (cardState == CardPositions.Right)
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
