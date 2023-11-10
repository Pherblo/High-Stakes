using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
	//public UnityEvent<CardEvent> OnCardPicked;      // When a card has been picked from the deck.

	[Header("References")]
	[SerializeField] private CharacterDatabase _database;

	// private List<CardEvent> _availableCards = new();
	// private List<CardEvent> _lockedCards = new();
	private List<CharacterData> _characters = new();
	private List<CharacterData> _aliveCharacters = new();
	private List<CharacterData> _deadCharacters = new();

	private List<CardDialogue> _selectedDialogues = new();

	public List<CardDialogue> SelectedDialogues => _selectedDialogues;

	// SerializedField for now just for debugging
	[SerializeField] private CardEvent currentCardDisplayed;

	// Accessor
	public CardEvent CurrentCardDisplayed => currentCardDisplayed;

	public void Start()
	{
		// Load all the cards. Instantiate their cards.
		foreach (CharacterData character in _database.CharacterInstances)
		{
			character.InitializeCharacter();
			_characters.Add(character);
		}
		// Sort characters
		foreach (CharacterData character in _characters)
		{
			if (character.IsAlive)
			{
				_aliveCharacters.Add(character);
			}
			else
			{
				_deadCharacters.Add(character);
			}
		}

		// Gets new card on start
		PickCard();
	}

	public void PickCard()
	{
		/*
		int randomIndex = Random.Range(0, _aliveCharacters.Count);
		CardEvent selectedCard = _aliveCharacters[randomIndex].GetCard();
		selectedCard.OnDialogueSelected += ProcessCard;*/
		// Shuffle deck to iterate through it and get the first available card.
		// We're shuffling instead of picking a character at random because characters may not return valid cards whose conditions are met.
		ShuffleDeck();
		CardEvent selectedCard;
		foreach (CharacterData character in _aliveCharacters)
		{
			selectedCard = character.GetCard();
			if (selectedCard)
			{
				// Change the current card displayed to the newly chosen card
				currentCardDisplayed = selectedCard; 
				
				// TODO: Make deck listen to card's event here.
				selectedCard.OnDialogueSelected += ProcessCard;
				//OnCardPicked?.Invoke(selectedCard); // Not too clear what the unity event does here.
				break;
			}
		}

		// Do something if no valid card is returned (ran out of cards).
	}

	private void ProcessCard(CardEvent card)
	{
		// Modify characters' alive/dead states if needed.
		// Store chosen dialogue.
		card.OnDialogueSelected = null;

		if (card.PickedChoice == SelectedChoice.ChoiceA)
		{
			_selectedDialogues.Add(card.DialogueA);
		}
		else if (card.PickedChoice == SelectedChoice.ChoiceB)
		{
			_selectedDialogues.Add(card.DialogueB);
		}

		/*
		// Add cards from _lockedCards into _availableCards if requirements are met.
		foreach(CardEvent lockedCard in _lockedCards)
		{
			if (lockedCard.CheckRequirements())
			{
				_lockedCards.Remove(lockedCard);
				_availableCards.Add(lockedCard);
			}
		}

		_availableCards.Remove(card);
		*/
		//_completedCards.Add(card);
	}

	[ContextMenu("Shuffle Test")]
	private void ShuffleDeck()
	{
		System.Random rng = new System.Random();
		_aliveCharacters = _aliveCharacters.OrderBy((x) => rng.Next()).ToList();
	}
}
