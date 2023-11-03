using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public UnityEvent<CardEvent> OnCardSelected;

    [Header("References")]
    [SerializeField] private CharacterDatabase _database;

    // private List<CardEvent> _availableCards = new();
    // private List<CardEvent> _lockedCards = new();
    private List<CharacterData> _characters = new();
    private List<CharacterData> _aliveCharacters = new();
    private List<CharacterData> _deadCharacters = new();

    public List<CardDialogue> selectedDialogues = new();

    public void Start()
    {
        // Load all the cards.
        _characters = _database.Characters.ToList();
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
    }

    public void PickCard()
    {
        int randomIndex = Random.Range(0, _aliveCharacters.Count);
        CardEvent selectedCard = _aliveCharacters[randomIndex].GetCard();
        selectedCard.OnDialogueSelected += ProcessCard;
    }

    private void ProcessCard(CardEvent card)
    {
        card.OnDialogueSelected = null;
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
        OnCardSelected?.Invoke(card);
    }

    [ContextMenu("Shuffle Test")]
    private void ShuffleDeck()
    {
        // Shuffle card events.
        _aliveCharacters.Sort(delegate (CharacterData x, CharacterData y)
        {
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0)
            {
                print("sort -1");
                return -1;
            }
            else
            {
                print("sort 1");
                return 1;
            }
        });
    }
}
