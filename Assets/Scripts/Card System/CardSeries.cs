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
    public int SeriesIndex => _seriesIndex;

    public override CardEvent GetCard()
    {
        Debug.LogWarning($"GETTING SERIES CARD OF: {gameObject.transform.name}");
        // Guard.
        if (_seriesIndex > _instancedCardEvents.Count) return null;

        if (_instancedCardEvents.Count > 0)
        {
            Debug.LogWarning("RETURNING SERIES CARD!!!");
            _seriesIndex++;
            return _instancedCardEvents[_seriesIndex - 1];
        }
        else
        {
            Debug.LogWarning("SERIES CARD NOT AVAILABLE");
            return null;
        }
    }

    public override bool CheckRequirements()
    {
        // Check if all cards completed.
        if (_seriesIndex < _instancedCardEvents.Count)
        {
            // If the current card meets requirements.
            if (_instancedCardEvents[_seriesIndex].CheckRequirements())
            {
                return true;
            }
        }
        return false;
    }

    public CardEvent GetCurrentSeriesCard()
    {
        if (_seriesIndex < _instancedCardEvents.Count) return _instancedCardEvents[_seriesIndex];
        else return null;
    }

    public void AddCardToSeries(CardEvent newCard)
    {
        _instancedCardEvents.Add(newCard);
    }

    // temporary fix
    public void DecrementSeriesIndex() => _seriesIndex--;
}
