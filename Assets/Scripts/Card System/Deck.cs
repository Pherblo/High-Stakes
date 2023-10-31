using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public UnityEvent<CardEvent> OnCardSelected;

    private List<CardEvent> _availableCards = new();
    private List<CardEvent> _lockedCards = new();
    private List<CardEvent> _completedCards = new();

    public void PickCard()
    {
        int randomIndex = Random.Range(0, _availableCards.Count);
        CardEvent selectedCard = _availableCards[randomIndex];
        selectedCard.OnDialogueSelected.AddListener(ProcessCard);
    }

    private void ProcessCard(CardEvent card)
    {
        card.OnDialogueSelected = null;
        OnCardSelected?.Invoke(card);
    }
}
