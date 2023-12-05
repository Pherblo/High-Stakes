using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private Stats _suspicion;
    [SerializeField] private Stats _souls;
    [SerializeField] private Stats _popularity;

    public void ModifySuspicion(float value)
    {
        _suspicion.changeValue(value);
    }

    public void ModifySouls(float value)
    {
        _souls.changeValue(value);
    }

    public void ModifyPopularity(float value)
    {
        _popularity.changeValue(value);
    }

    public void ModifyStats(CardEvent card)
    {
        if (card.PickedChoice == SelectedChoice.ChoiceA)
        {
            _suspicion.changeValue(card.suspicionValueA);
            _souls.changeValue(card.faithValueA);
            _popularity.changeValue(card.popularityValueA);
        }
        else if (card.PickedChoice == SelectedChoice.ChoiceB)
        {
            _suspicion.changeValue(card.suspicionValueB);
            _souls.changeValue(card.faithValueB);
            _popularity.changeValue(card.popularityValueB);
        }
    }

    // Called via events.
    public void HighlightStats(CardEvent card, float input)
    {
        if (input < 0)
        {
            if (card.suspicionValueA != 0) _suspicion.Glow();
            if (card.faithValueA != 0) _souls.Glow();
            if (card.popularityValueA != 0) _popularity.Glow();
        }
        else if (input > 0)
        {
            if (card.suspicionValueB != 0) _suspicion.Glow();
            if (card.faithValueB != 0) _souls.Glow();
            if (card.popularityValueB != 0) _popularity.Glow();
        }
    }

    // Called via drag events in current card animator.
    public void RemoveStatsHighlights()
    {
        _suspicion.ClearGlow();
        _souls.ClearGlow();
        _popularity.ClearGlow();
    }
}
