using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeries : CardBase
{
    [Header("Cards will be selected in order. This is similar to having a series of cards whose requirements are that the card before them needs to be completed.")]
    [SerializeField] private List<CardEvent> _cardEvents = new();

    public override CardEvent GetCard()
    {
        if (_cardEvents.Count > 1) return _cardEvents[0];
        else return null;
    }

    public override bool CheckRequirements()
    {
        foreach (CardEvent card in _cardEvents)
        {
            if (card.CheckRequirements()) return true;
            else return false;
        }

        // Card Series is empty.
        return false;
    }
}
