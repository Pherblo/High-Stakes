using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public UnityEvent<CardEvent> OnCardSelected;

    // Load all cards

    private List<CardEvent> _availableCards = new();
    private List<CardEvent> _lockedCards = new();

    public List<CardDialogue> selectedDialogues = new();

    public void InitializeDeck()
    {
        // Load all the cards.
    }

    public void PickCard()
    {
        int randomIndex = Random.Range(0, _availableCards.Count);
        CardEvent selectedCard = _availableCards[randomIndex];
        selectedCard.OnDialogueSelected += ProcessCard;
    }

    private void ProcessCard(CardEvent card)
    {
        card.OnDialogueSelected = null;

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
        //_completedCards.Add(card);
        OnCardSelected?.Invoke(card);
    }
}
