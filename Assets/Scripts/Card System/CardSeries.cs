using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeries : CardBase
{
    [Header("Cards will be selected in order. This is similar to having a series of cards whose requirements are that the card before them needs to be completed.")]
    [SerializeField] private List<CardEvent> _cardEvents = new();

    private int _seriesIndex = 0;
    private List<CardEvent> _instancedCardEvents = new();

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
        // Check if all cards completed.
        if (_seriesIndex < _instancedCardEvents.Count)
        {
            // If the current card meets requirements.
            if (_cardEvents[_seriesIndex].CheckRequirements())
            {
                return true;
            }
        }
        return false;
    }

    public void AddCardToSeries(CardEvent newCard)
    {
        _instancedCardEvents.Add(newCard);
    }
}
