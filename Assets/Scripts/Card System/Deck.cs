using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public UnityEvent<CardEvent> OnCardPicked;      // When a card has been picked from the deck.

    //[Header("References")]

    //public GameObject _database;
    [Header("Resource Paths")]
    [SerializeField] private string _charactersPath;
    [SerializeField] private string _cardsPath;
    [SerializeField] private string _tutorialSeriesPath;
    [SerializeField] private string _endCardsPath;

    [Header("Scene References")]
    [SerializeField] private StatsManager _stats;
    /*[SerializeField] private Stats _suspicion;
    [SerializeField] private Stats _souls;
    [SerializeField] private Stats _popularity;*/

    [Header("Tutorial Settings")]
    [SerializeField] private CardSeries _tutorialCardSeries;

    [Header("Default End Card")]
    [SerializeField] private CardEvent _defaultEndCard;     // Gets picked when there are no available cards left.

    [Header("Debug settings")]
    [SerializeField] private bool _skipTutorial = false;

    private List<CharacterData> _characters = new();
    private List<CardBase> _availableCards = new();
    private List<CardBase> _lockedCards = new();
    private List<CardBase> _finishedCards = new();      // Storing finished cards here just for organization purposes.
    private List<CardDialogue> _selectedDialogues = new();
    private List<CardEvent> _guaranteedCards = new();
    private List<CardEvent> _endCards = new();

    public StatsManager Stats => _stats;
    private CardSeries _tutorialSeriesInstance;
    private CardEvent _defaultEndCardInstance;

    public List<CardDialogue> SelectedDialogues => _selectedDialogues;
    /*public Stats Suspicion => _suspicion;
    public Stats Souls => _souls;
    public Stats Popularity => _popularity;*/

    [Header("Variables to keep track of game")]
    private bool runningTutorial;
    private int cardNum = 0; //for keeping track of cards when drawing in order


    public void Awake()
    {
        // Cache cards and card series for use later.
        List<CardEvent> allCards = new();
        List<CardSeries> allSeries = new();

        // Load and instantiate all cards and put them all into _lockedCards to sort further.
        CardBase[] cardPrefabs = Resources.LoadAll<CardBase>(_cardsPath);
        foreach (CardBase cardPrefab in cardPrefabs)
        {
            CardBase cardInstance = null;
            cardInstance = Instantiate(cardPrefab, transform);
            if (cardInstance is CardEvent cardEventInstance) cardEventInstance.AssignDeck(this);
            _lockedCards.Add(cardInstance);

            // If it's a card series, also instantiate all its cards.
            if (cardPrefab is CardSeries cardSeriesInstance)
            {
                foreach (CardEvent seriesCard in cardSeriesInstance.CardEvents)
                {
                    CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
                    seriesCardInstance.AssignDeck(this);
                    cardSeriesInstance.AddCardToSeries(seriesCardInstance);
                }
            }

            if (cardInstance is CardEvent cardEvent) allCards.Add(cardEvent);
            else if (cardInstance is CardSeries cardSeries) allSeries.Add(cardSeries);
        }

        // Load and instantiate tutorial series and its cards.
        _tutorialSeriesInstance = Instantiate(_tutorialCardSeries, transform);
        foreach (CardEvent seriesCard in _tutorialSeriesInstance.CardEvents)
        {
            CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
            seriesCardInstance.AssignDeck(this);
            _tutorialSeriesInstance.AddCardToSeries(seriesCardInstance);
        }
        
        // Add all cards from all CardSeries present.
        foreach (CardSeries series in allSeries) allCards.AddRange(series.CardEvents);

        // Load and instantiate all characters, then assign their respective characters
        CharacterData[] loadedCharacters = Resources.LoadAll<CharacterData>(_charactersPath);
        foreach (CharacterData character in loadedCharacters)
        {
            CharacterData characterInstance = Instantiate(character, transform);
            _characters.Add(characterInstance);

            // Assign character instance to card events with matching associated character.
            List<CardEvent> matchingCards = allCards.FindAll((x) => x.AssociatedCharacter == character);
            foreach (CardEvent card in matchingCards)
            {
                card.AssignCharacter(characterInstance);
            }
        }

        // Sort all cards and get the first available ones.
        CardBase[] cardsToSort = _lockedCards.ToArray();
        foreach (CardBase card in cardsToSort)
        {
            if (card.CheckRequirements())
            {
                _lockedCards.Remove(card);
                _availableCards.Add(card);
            }
        }

        // Create end cards.
        CardEvent[] loadedEndCard = Resources.LoadAll<CardEvent>(_endCardsPath);
        foreach (CardEvent card in loadedEndCard)
        {
            CardEvent endCardInstance = Instantiate(card, transform);
            endCardInstance.AssignDeck(this);
            _endCards.Add(endCardInstance);
        }

        // Create default end card.
        _defaultEndCardInstance = Instantiate(_defaultEndCard, transform);
        _defaultEndCard.AssignDeck(this);
    }

    public CardEvent PickCard()
    {
        // Pick end cards, if any.
        //Debug.LogWarning($"{_endCards[0].gameObject.name}, {_endCards[0].CheckRequirements()}, dialogue selected: {_selectedDialogues.Count}");
        foreach (CardEvent endCard in _endCards)
        {
            if (endCard.CheckRequirements())
            {
                return endCard;
            }
        }
        /*for (int i = 0; i < _endCards.Count; i++)
        {
            if (_endCards[i].CheckRequirements())
            {
                return _endCards[i];
            }
            else
            {
                Debug.LogWarning($"not valid condition: {_endCards[i]}");
                Debug.LogWarning($"dialogue count: {SelectedDialogues.Count}");
            }
        }*/

        // Return first guaranteed card, if any.
        if (_guaranteedCards.Count > 0)
        {
            CardEvent guaranteedCard = _guaranteedCards[0];
            _guaranteedCards.Remove(guaranteedCard);
            return guaranteedCard;
        }

        // Return tutorial series, if not finished yet.
        if (!_skipTutorial &&_tutorialSeriesInstance.CheckRequirements())
        {
            CardEvent tutorialCard = _tutorialSeriesInstance.GetCard();
            return tutorialCard;
            //return _tutorialSeriesInstance.GetCard();
        }

        ShuffleDeck();
        // Pick out cards based on characters.
        foreach (CharacterData character in _characters)
        {
            if (!character.IsAlive) continue;

            CardEvent newCard;
            List<CardBase> associatedCards = _availableCards.FindAll((x) => x.GetCard().AssociatedCharacter == character);
            
            foreach (CardEvent card in associatedCards)
            {
                newCard = card;
                if (card.CheckRequirements())
                {
                    //card.OnDialogueSelected += ProcessCard;
                    //OnCardPicked?.Invoke(newCard);
                    //print(newCard);
                    return card;
                }
            }
        }
        // Do something if no valid card is returned (ran out of cards).
        return _defaultEndCardInstance;
    }

    /*public CardEvent PickCardOld()
    {
        //AssignData(); //assign data to card
        // Shuffle deck to iterate through it and get the first available card.
        // Pick a random character, then pick a random card associated with them.
        // We're shuffling instead of picking a character at random because characters may not return valid cards whose conditions are met.
        runningTutorial = GetComponentInParent<GameManager>().getTutorialStatus(); //set running tutorial variable to the one in game manager class
        print("status: " + runningTutorial);
        CardEvent newCard;

        if (runningTutorial) //when tutorial is running
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
                        if (cardNum == associatedCards.Count)
                        {
                            GetComponentInParent<GameManager>().ToggleTutorial(false);
                            cardNum = 0;
                        }
                    }

                    if (card.CheckRequirements())
                    {
                        card.OnDialogueSelected += ProcessCard;
                        OnCardPicked?.Invoke(newCard);
  
                        return newCard;
                    }
                }
            }
        }
        else //if not in tutorial
        {
            if (cardNum == 0) //if first card after tutorial
            {
                //AssignData(); //assign new data
            }

            ShuffleDeck(); //shuffle deck

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
                        print(newCard);
                        return card;
                    }
                }
            }
        }
        return null;
    }*/

    public void ProcessCard(CardEvent card)
    {
        // Modify characters' alive/dead states if needed.
        // Store chosen dialogue.
        card.OnDialogueSelected = null;

        /*if (card.PickedChoice == SelectedChoice.ChoiceA)
        {
            Suspicion.changeValue(card.suspicionValueA);
            Popularity.changeValue(card.popularityValueA);
            _selectedDialogues.Add(card.DialogueA);
        }
        else if (card.PickedChoice == SelectedChoice.ChoiceB)
        {
            Suspicion.changeValue(card.suspicionValueB);
            Popularity.changeValue(card.popularityValueB);
            _selectedDialogues.Add(card.DialogueB);
        }*/

        _availableCards.Remove(card);
        _guaranteedCards.Remove(card);
        _finishedCards.Add(card);

        // Sort all locked cards.
        CardBase[] cardsToSort = _lockedCards.ToArray();
        foreach (CardBase lockedCard in cardsToSort)
        {
            if (lockedCard.CheckRequirements())
            {
                _lockedCards.Remove(lockedCard);
                if (lockedCard is CardEvent cardEvent && cardEvent.GuaranteedCard)
                {
                    _guaranteedCards.Add(cardEvent);
                }
                else _availableCards.Add(lockedCard);
            }
        }
    }

    [ContextMenu("Shuffle Test")]
    private void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        _characters = _characters.OrderBy((x) => rng.Next()).ToList();
        _availableCards = _availableCards.OrderBy((x) => rng.Next()).ToList();
        cardNum++;  //add a new card to cardNum
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

    /*private void AssignData()
    {
        //moved this from start so it can be called in different places

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
    }*/
}
