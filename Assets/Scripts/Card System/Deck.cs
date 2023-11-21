using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
	public UnityEvent<CardEvent> OnCardPicked;      // When a card has been picked from the deck.

	[Header("References")]
	//[SerializeField] private string _cardEventsResourcesPath;
   // [SerializeField] private string _characterDataResourcesPath;
    public GameObject _database;

	private List<CharacterData> _characters = new();
	private List<CardEvent> _availableCards = new();
	private List<CardEvent> _lockedCards = new();

	private List<CardDialogue> _selectedDialogues = new();

	public List<CardDialogue> SelectedDialogues => _selectedDialogues;

	// SerializedField for now just for debugging
	[SerializeField] private CardEvent currentCardDisplayed;
	private bool runningTutorial;

    // Accessor
    public CardEvent CurrentCardDisplayed => currentCardDisplayed;

	private int cardNum = 0 ; //for keeping track of cards when drawing in order

  
    public void Start()
	{

        
        // Load and instantiate all cards and put them all into _lockedCards to sort further.
        CardEvent[] cardPrefabs = Resources.LoadAll<CardEvent>(_database.GetComponent<CharacterDatabase>()._eventsResourcePath);
		foreach (CardEvent cardPrefab in cardPrefabs)
		{
			CardEvent cardInstance = Instantiate(cardPrefab, transform);
			_lockedCards.Add(cardInstance);
		}

		// Load and instantiate all characters.
		CharacterData[] _loadedCharacters = _database.GetComponent<CharacterDatabase>()._characterPrefabs;
        foreach (CharacterData character in _loadedCharacters)
        {
            CharacterData characterInstance = Instantiate(character, transform);
            _characters.Add(characterInstance);
			// Assign character instance to cards with matching associated character.
			List<CardEvent> matchingCards = _lockedCards.FindAll((x) => x.AssociatedCharacter == character);
			foreach (CardEvent card in matchingCards)
			{
				card.AssignCharacter(characterInstance);
			}
        }

		// Sort all cards and get the first available ones.
		CardEvent[] cardsToSort = _lockedCards.ToArray();
        foreach (CardEvent card in cardsToSort)
        {
            if (card.CheckRequirements())
            {
                _lockedCards.Remove(card);
                _availableCards.Add(card);
            }
        }

		// Pick a card at the start.
		PickCard();

        /*
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
		*/
        //PickCard();
    }

	public void PickCard()
	{
        // Shuffle deck to iterate through it and get the first available card.
        // Pick a random character, then pick a random card associated with them.
        // We're shuffling instead of picking a character at random because characters may not return valid cards whose conditions are met.
        runningTutorial = GetComponentInParent<GameManager>().runningTutorial;
        CardEvent newCard;
		print(runningTutorial);

		if (runningTutorial)
		{
			foreach (CharacterData character in _characters)
			{
				List<CardEvent> associatedCards = _availableCards.FindAll((x) => x.AssociatedCharacter == character);
				foreach (CardEvent card in associatedCards)
				{

					newCard = card;

					if (cardNum < associatedCards.Count)
					{
						newCard = nextCard(card, associatedCards); //draw the next card in order
						if(cardNum == associatedCards.Count )
						{
                            GetComponentInParent<GameManager>().runningTutorial = false;
                        }
					}
			


					if (card.CheckRequirements())
					{
						card.OnDialogueSelected += ProcessCard;
						OnCardPicked?.Invoke(newCard);

						return;
					}
				}
			}
		}
		else
		{
			print("new");
			ShuffleDeck();
            foreach (CharacterData character in _characters)
            {
                List<CardEvent> associatedCards = _availableCards.FindAll((x) => x.AssociatedCharacter == character);
                foreach (CardEvent card in associatedCards)
                {

                    newCard = card;

                    if (card.CheckRequirements())
                    {
                        card.OnDialogueSelected += ProcessCard;
                        OnCardPicked?.Invoke(newCard);

                        return;
                    }
                }
            }
        }
		/*
		CardEvent selectedCard;
		foreach (CharacterData character in _aliveCharacters)
		{
			selectedCard = character.GetCard();
			if (selectedCard)
			{
				// Change the current card displayed to the newly chosen card
				currentCardDisplayed = selectedCard; 
				
				selectedCard.OnDialogueSelected += ProcessCard;
				OnCardPicked?.Invoke(selectedCard);
				break;
			}
		}
		*/
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
        _characters = _characters.OrderBy((x) => rng.Next()).ToList();
        _availableCards = _availableCards.OrderBy((x) => rng.Next()).ToList();
		print("Shuffled");
    }

	private CardEvent nextCard(CardEvent card, List<CardEvent> associatedCards)
	{
		//int cardPos = associatedCards.FindIndex(0, associatedCards.Count, (x) => card);
		
		CardEvent newCard = associatedCards[cardNum];
		print(associatedCards.FindIndex(0, associatedCards.Count, (x) => newCard));
		print("num: " + cardNum);
		cardNum++;
		return newCard;
    }
}
