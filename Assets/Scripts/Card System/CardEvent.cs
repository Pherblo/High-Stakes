using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enum list of options for selected dialogues
public enum SelectedChoice
{
    ChoiceA = 0,
    ChoiceB = 1
}

// Is on a card prefab
public class CardEvent : MonoBehaviour
{
	public Action<CardEvent> OnDialogueSelected;

	[Header("Card Settings")]
	[SerializeField] private CharacterData _associatedCharacter;
	[SerializeField] private string _description;
	// [SerializeField] private bool _guaranteedCard = false; // If true, this card will be played next once requirements are met.

	[Header("Dialogues")]
	[SerializeField] private CardDialogue _dialogueA;
	[SerializeField] private CardDialogue _dialogueB;
	[Header("Conditions")]
	// [SerializeField] private List<CardDialogue> _dialogueRequirements = new();
	[SerializeField] private List<CardCondition> _conditions = new();
	[Header("Deck")]
	[SerializeField] private Deck _deck;

	private SelectedChoice _pickedChoice;

	public string Description => _description;
	public CardDialogue DialogueA => _dialogueA;
	public CardDialogue DialogueB => _dialogueB;
	// public bool GuaranteedCard => _guaranteedCard;
	public SelectedChoice PickedChoice => _pickedChoice;

	// This Event Cards Requirements

    // Called by player input via GUI.
    public void ChooseDialogue(int optionInt)
	{
		// Maps int to a SelectedDialogue
        SelectedChoice option = (SelectedChoice)optionInt;

		// Add corresponding selected dialogue to deck selected dialogue list
		if(option == SelectedChoice.ChoiceA)
		{
			_deck.selectedDialogues.Add(_dialogueA);
        }
		else if(option == SelectedChoice.ChoiceB)
		{
			_deck.selectedDialogues.Add(_dialogueB);
		}
	}

	public bool CheckRequirements()
	{
		// For each of this cards dialogues requirements check if it is in selected dialogues
		/*
		for(int i = 0; i < _dialogueRequirements.Count; i++)
		{
            // Checks if each required dialogue is in the selected dialogues
            if (_deck.selectedDialogues.Contains(_dialogueRequirements[i]))
			{
				// Just continue checking
            }
			else
			{
				// Returns bool right away if any are not included in selected dialogues
				return false;
			}
        }
		*/
		// Only returns true if all requirements are in selected dialogues
		return true;
	}

	// Just a debug to use in a button to see if this works
    public void test()
    {
		Debug.Log("Requirements met for this card: " + CheckRequirements());
    }
}
