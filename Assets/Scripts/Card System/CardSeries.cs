using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeries : CardBase
{
    [Header("Cards will be selected in order. This is similar to having a series of cards whose requirements are that the card before them needs to be completed.")]
    [SerializeField] private List<CardEvent> _cardEvents = new();

    private int _seriesIndex = 0;

    public List<CardEvent> CardEvents => _cardEvents;

    public override CardEvent GetCard()
    {
        // Guard.
        if (_seriesIndex > _cardEvents.Count) return null;

        if (_cardEvents.Count > 1)
        {
            _seriesIndex++;
            return _cardEvents[_seriesIndex - 1];
        }
        else return null;
    }

    public override bool CheckRequirements()
    {
        // All cards completed.
        if (_seriesIndex > _cardEvents.Count) return false;

        foreach (CardEvent card in _cardEvents)
        {
            if (card.CheckRequirements()) return true;
            else return false;
        }

        // Card Series is empty.
        return false;
    }
}
