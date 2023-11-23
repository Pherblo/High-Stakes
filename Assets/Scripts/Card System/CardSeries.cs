using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSeries : CardBase
{
    [SerializeField] private List<CardEvent> _cardEvents = new();

    public override CardEvent GetCard()
    {
        if (_cardEvents.Count > 1) return _cardEvents[0];
        else return null;
    }
}
