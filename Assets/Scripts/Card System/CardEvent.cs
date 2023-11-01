using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Is on a card prefab
public class CardEvent : MonoBehaviour
{
	public Action<CardEvent> OnDialogueSelected;


	[Header("Card Settings")]
	[SerializeField] private CharacterData _associatedCharacter;
	[SerializeField] private string _description;
	[SerializeField] private bool _guaranteedCard = false;      // If true, this card will be played next once requirements are met.
	// Card requirements go here.
	[Header("Dialogues")]
	[SerializeField] private CardDialogue _dialogueA;
	[SerializeField] private CardDialogue _dialogueB;
	[Header("Deck")]
	[SerializeField] private Deck _deck;
	public bool GuaranteedCard => _guaranteedCard;

	// Called by player input via GUI.
	public void ChooseDialogue(SelectedDialogue option)
	{
		if(option == SelectedDialogue.dialogueA)
		{
			_deck.selectedDialogues.Add(_dialogueA);
        }
		else if(option == SelectedDialogue.dialogueB)
		{
			_deck.selectedDialogues.Add(_dialogueB);
		}
	}

	public bool CheckRequirements()
	{
		// TODO: IMPLEMENT FUNCTIONALITY.
		return true;
	}
}

public enum SelectedDialogue
{
    dialogueA,
	dialogueB
}
