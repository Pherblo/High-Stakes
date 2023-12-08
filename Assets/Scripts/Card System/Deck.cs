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
    [SerializeField] private string _fullMoonWinPath;
    [SerializeField] private string _fullMoonLosePath;

    [Header("Scene References")]
    [SerializeField] private StatsManager _stats;
    /*[SerializeField] private Stats _suspicion;
    [SerializeField] private Stats _souls;
    [SerializeField] private Stats _popularity;*/

    [Header("Tutorial Settings")]
    [SerializeField] private CardSeries _tutorialCardSeries;

    [Header("Full Moon Settings")]
    [SerializeField] private CardSeries _fullMoonWinSeries;
    [SerializeField] private CardSeries _fullMoonLoseSeries;

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
    public Timeline _timeline;
    private CardSeries _tutorialSeriesInstance;
    private CardSeries _fullMoonWinSeriesInstance;
    private CardSeries _fullMoonLoseSeriesInstance;
    private CardEvent _defaultEndCardInstance;
    public Stats _souls;

    public List<CardDialogue> SelectedDialogues => _selectedDialogues;
    /*public Stats Suspicion => _suspicion;
    public Stats Souls => _souls;
    public Stats Popularity => _popularity;*/

    [Header("Variables to keep track of game")]
    private bool runningTutorial;
    private int cardNum = 0; //for keeping track of cards when drawing in order
    private List<CardEvent> allCards2 = new();

    private void Update()
    {
        print(allCards2[0].transform.name);
    }

    public void Awake()
    {
        // Cache cards and card series for use later.
        List<CardEvent> allCards = new();
        List<CardSeries> allSeries = new();

        // Load and instantiate all cards and put them all into _lockedCards to sort further.
        CardBase[] cardPrefabs = Resources.LoadAll<CardBase>(_cardsPath);
        foreach (CardBase cardPrefab in cardPrefabs)
        {
            CardBase cardInstance = Instantiate(cardPrefab, transform);
            if (cardInstance is CardEvent cardEventInstance) cardEventInstance.AssignDeck(this);
            _lockedCards.Add(cardInstance);
            if (cardInstance is CardEvent cardEvent) allCards.Add(cardEvent);
           // else if (cardInstance is CardSeries cardSeries) allSeries.Add(cardSeries);
            // If it's a card series, also instantiate all its cards.
            else if (cardInstance is CardSeries cardSeriesInstance)
            {
                allSeries.Add(cardSeriesInstance);
                Debug.LogWarning("adding card to series");
                foreach (CardEvent seriesCard in cardSeriesInstance.CardEvents)
                {
                    Debug.LogWarning("adding card INSTANCE to series");
                    CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
                    seriesCardInstance.AssignDeck(this);
                    cardSeriesInstance.AddCardToSeries(seriesCardInstance);
                    allCards.Add(seriesCardInstance);
                }
            }
        }

        // Load and instantiate tutorial series and its cards.
        _tutorialSeriesInstance = Instantiate(_tutorialCardSeries, transform);
        foreach (CardEvent seriesCard in _tutorialSeriesInstance.CardEvents)
        {
            CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
            seriesCardInstance.AssignDeck(this);
            _tutorialSeriesInstance.AddCardToSeries(seriesCardInstance);
        }

        // Load and instantiate full moon event cards and series
        
        _fullMoonWinSeriesInstance = Instantiate(_fullMoonWinSeries, transform);
        foreach (CardEvent seriesCard in _fullMoonWinSeriesInstance.CardEvents)
        {
            CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
            seriesCardInstance.AssignDeck(this);
            _fullMoonWinSeriesInstance.AddCardToSeries(seriesCardInstance);
        }
        
        _fullMoonLoseSeriesInstance = Instantiate(_fullMoonLoseSeries, transform);
        foreach (CardEvent seriesCard in _fullMoonLoseSeriesInstance.CardEvents)
        {
            CardEvent seriesCardInstance = Instantiate(seriesCard, transform);
            seriesCardInstance.AssignDeck(this);
            _fullMoonLoseSeriesInstance.AddCardToSeries(seriesCardInstance);
        }

        // Add all cards from all CardSeries present.
        //foreach (CardSeries series in allSeries) allCards.AddRange(series.CardEvents);

        // Create end cards.
        CardEvent[] loadedEndCard = Resources.LoadAll<CardEvent>(_endCardsPath);
        foreach (CardEvent card in loadedEndCard)
        {
            CardEvent endCardInstance = Instantiate(card, transform);
            endCardInstance.AssignDeck(this);
            _endCards.Add(endCardInstance);
        }

        // I UNIRONICALLY THINK THAT THIS IS A GOOD USE CASE FOR SCRIPTABLE OBJECTS!!!
        // Load and instantiate all characters, then assign their respective characters
        CharacterData[] loadedCharacters = Resources.LoadAll<CharacterData>(_charactersPath);
        foreach (CharacterData character in loadedCharacters)
        {
            // Instantiate character here, then assign all needed references.
            CharacterData characterInstance = Instantiate(character, transform);
            _characters.Add(characterInstance);

            // Assign character instances to matching associated character, conditions, and dialogues.
            foreach (CardEvent card in allCards)
            {
                if (card.AssociatedCharacter == character)
                {
                    Debug.LogWarning($"assigned char prefab to card: {card.transform.name}");
                    card.AssignCharacter(characterInstance);
                }
                    foreach (CardCondition condition in card.Conditions)
                {
                    if (condition.CharacterReference == character)
                    {
                        condition.AssignCharacterReference(characterInstance);
                    }
                }
                if (card.DialogueA.CharactersToBeDead.Contains(character))
                {
                    card.DialogueA.AssignCharacterInstance(characterInstance);
                }
                if (card.DialogueB.CharactersToBeDead.Contains(character))
                    card.DialogueB.AssignCharacterInstance(characterInstance);
            }
            foreach (CardEvent card in _endCards)
            {
                if (card.AssociatedCharacter == character)
                {
                    Debug.LogWarning($"assigned char prefab to card: {card.transform.name}");
                    card.AssignCharacter(characterInstance);
                }

                foreach (CardCondition condition in card.Conditions)
                {
                    if (condition.CharacterReference == character)
                    {
                        condition.AssignCharacterReference(characterInstance);
                    }
                }
                if (card.DialogueA.CharactersToBeDead.Contains(character))
                {
                    card.DialogueA.AssignCharacterInstance(characterInstance);
                }
                if (card.DialogueB.CharactersToBeDead.Contains(character))
                    card.DialogueB.AssignCharacterInstance(characterInstance);
            }
            /*List<CardEvent> matchingCards = allCards.FindAll((x) => x.AssociatedCharacter == character);
            foreach (CardEvent card in matchingCards)
            {
                card.AssignCharacter(characterInstance);
            }*/
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

        // Create default end card.
        _defaultEndCardInstance = Instantiate(_defaultEndCard, transform);
        _defaultEndCard.AssignDeck(this);

        // Add all guaranteed cards that are available at the start into the list.
        List<CardEvent> guaranteedCards = allCards.FindAll((x) => x.GuaranteedCard);
        foreach (CardEvent card in guaranteedCards)
        {
            _guaranteedCards.Add(card);
        }
        allCards2 = allCards;
    }

    public CardEvent PickCard()
    {
        print("PICKING CARD");
        runningTutorial = false; 
        // Pick end cards, if any.
        //Debug.LogWarning($"{_endCards[0].gameObject.name}, {_endCards[0].CheckRequirements()}, dialogue selected: {_selectedDialogues.Count}");
        foreach (CardEvent endCard in _endCards)
        {
            if (endCard.CheckRequirements())
            {
                return endCard;
            }
        }
        //full moon events
        
        if (_timeline.TriggerGodEvent() && _fullMoonWinSeriesInstance.CheckRequirements() && _souls.getValue() >= _timeline.requiredSouls)
        {
            print("full moon WIN event");
            CardEvent fullMoonWinCard = _fullMoonWinSeriesInstance.GetCard();
            return fullMoonWinCard;
        }
        
        if (_timeline.TriggerGodEvent() && _fullMoonLoseSeriesInstance.CheckRequirements() && _souls.getValue() < _timeline.requiredSouls)
        {
            print("full moon LOSE event");
            CardEvent fullMoonLoseCard = _fullMoonLoseSeriesInstance.GetCard();
            return fullMoonLoseCard;
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
            runningTutorial = true;
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
            List<CardEvent> associatedCards = new();// _availableCards.FindAll((x) => x.GetCard().AssociatedCharacter == character);
            foreach (CardBase availableCard in _availableCards)
            {
                print($"RUNNING CHECK: current card: {availableCard}, available cards count: {_availableCards.Count}");
                CardEvent cardToCheck = null;// availableCard.GetCard();
                if (availableCard is CardSeries availableSeries)
                {
                    if (availableSeries.GetCurrentSeriesCard().CheckRequirements())
                    {
                        associatedCards.Add(availableCard.GetCard());
                    }
                }
                else if (availableCard.GetCard().AssociatedCharacterInstance == character)
                {
                    associatedCards.Add(cardToCheck);
                }
                else Debug.LogWarning("requirement not fulfilled");
                /*if (availableCard is CardSeries availableSeries)
                {
                    availableSeries.DecrementSeriesIndex();    // temporary fix
                    print("DECREMENTING: " + availableSeries.SeriesIndex);
                }*/
            }
            foreach (CardEvent card in associatedCards)
            {
                Debug.LogWarning($"{card.gameObject.transform.name}");
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
        print("Returning default card");
        return _defaultEndCardInstance;
    }

    public bool GetTutorialStatus()
    {
        return runningTutorial;
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

        // Kill characters, if any.
        if (card.PickedChoice == SelectedChoice.ChoiceA && card.DialogueA.InstancedCharacterTargets.Any())
        {
            foreach (CharacterData character in card.DialogueA.InstancedCharacterTargets)
            {
                character.SetAliveState(false);
            }
        }
        else if (card.PickedChoice == SelectedChoice.ChoiceB && card.DialogueB.InstancedCharacterTargets.Any())
        {
            foreach (CharacterData character in card.DialogueB.InstancedCharacterTargets)
            {
                character.SetAliveState(false);
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
