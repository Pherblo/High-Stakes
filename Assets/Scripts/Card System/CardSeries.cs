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
        if (_seriesIndex > _instancedCardEvents.Count) return null;

        if (_instancedCardEvents.Count > 0)
        {
            _seriesIndex++;
            return _instancedCardEvents[_seriesIndex - 1];
        }
        else return null;
    }

    public override bool CheckRequirements()
    {
        print($"checking series card reqs. index:{_seriesIndex}, count: {_instancedCardEvents.Count}");
        // Check if all cards completed.
        if (_seriesIndex < _instancedCardEvents.Count)
        {
            print("start checking series");
            // If the current card meets requirements.
            if (_instancedCardEvents[_seriesIndex].CheckRequirements())
            {
                print($"series card doesnt meet requirements. index: {_seriesIndex}, count: {_instancedCardEvents.Count}");
                return true;
            }
        }
        print("series has no card with reqs true");
        return false;
    }

    public void AddCardToSeries(CardEvent newCard)
    {
        _instancedCardEvents.Add(newCard);
    }
}
