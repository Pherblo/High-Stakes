using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enum list of options for selected dialogues
public enum SelectedDialogue
{
    dialogueA, // 0
    dialogueB  // 1
}

// Is on a card prefab
public class CardEvent : MonoBehaviour
{
	public Action<CardEvent> OnDialogueSelected;

	[Header("Card Settings")]
	[SerializeField] private CharacterData associatedCharacter;
	[SerializeField] private string description;
	[SerializeField] private bool guaranteedCard = false; // If true, this card will be played next once requirements are met.

	[Header("Dialogues")]
	[SerializeField] private CardDialogue dialogueA;
	[SerializeField] private CardDialogue dialogueB;
	[Header("Deck")]
	[SerializeField] private Deck _deck;
	public bool GuaranteedCard => guaranteedCard;
	public string Description => description;

	public CardDialogue DialogueA => dialogueA;
	public CardDialogue DialogueB => dialogueB;

	// This Event Cards Requirements
	[SerializeField] private List<CardDialogue> dialogueRequirements = new();

    // Called by player input via GUI.
    public void ChooseDialogue(int optionInt)
	{
		// Maps int to a SelectedDialogue
        SelectedDialogue option = (SelectedDialogue)optionInt;

		// Add corresponding selected dialogue to deck selected dialogue list
		if(option == SelectedDialogue.dialogueA)
		{
			_deck.selectedDialogues.Add(dialogueA);
        }
		else if(option == SelectedDialogue.dialogueB)
		{
			_deck.selectedDialogues.Add(dialogueB);
		}
	}

	public bool CheckRequirements()
	{
		// For each of this cards dialogues requirements check if it is in selected dialogues
		for(int i = 0; i < dialogueRequirements.Count; i++)
		{
            // Checks if each required dialogue is in the selected dialogues
            if (_deck.selectedDialogues.Contains(dialogueRequirements[i]))
			{
				// Just continue checking
            }
			else
			{
				// Returns bool right away if any are not included in selected dialogues
				return false;
			}
        }

		// Only returns true if all requirements are in selected dialogues
		return true;
	}

	// Just a debug to use in a button to see if this works
    public void test()
    {
		Debug.Log("Requirements met for this card: " + CheckRequirements());
    }
}
