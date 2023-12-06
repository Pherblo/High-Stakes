using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private Stats _suspicion;
    [SerializeField] private Stats _souls;
    [SerializeField] private Stats _popularity;

    public Stats Suspicion => _suspicion;
    public Stats Souls => _souls;
    public Stats Popularity => _popularity;

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

    public void HighlightStats(CardEvent card, int input)
    {
        // TODO: optimize this pls -Lorenzo.
        //RemoveStatsHighlights();

        if (input < 0)
        {
            if (card.suspicionValueA > 0) _suspicion.Glow(true);
            else if (card.suspicionValueA < 0) _suspicion.Glow(false);
            else _suspicion.ClearGlow();

            if (card.faithValueA > 0) _souls.Glow(true);
            else if (card.faithValueA < 0) _souls.Glow(false);
            else _souls.ClearGlow();

            if (card.popularityValueA > 0) _popularity.Glow(true);
            else if (card.popularityValueA < 0) _popularity.Glow(false);
            else _popularity.ClearGlow();
        }
        else if (input > 0)
        {
            if (card.suspicionValueB > 0) _suspicion.Glow(true);
            else if (card.suspicionValueB < 0) _suspicion.Glow(false);
            else _suspicion.ClearGlow();

            if (card.faithValueB > 0) _souls.Glow(true);
            else if (card.faithValueB < 0) _souls.Glow(false);
            else _souls.ClearGlow();

            if (card.popularityValueB > 0) _popularity.Glow(true);
            else if (card.popularityValueB < 0) _popularity.Glow(false);
            else _popularity.ClearGlow();
        }
    }

    // Called via drag events in current card animator.
    // NOTE: PARAM IS SERVES NO PURPOSE EXCEPT TO MAKE THE FUNCTINO ABLE TO SUB TO AN EVENT.
    public void RemoveStatsHighlights()
    {
        _suspicion.ClearGlow();
        _souls.ClearGlow();
        _popularity.ClearGlow();
    }
}
