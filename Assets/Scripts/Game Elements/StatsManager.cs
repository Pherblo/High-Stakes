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
        //
    }

    public void HighlightStats(CardEvent card, float input)
    {
        if (input < 0)
        {
            //
        }
        else if (input > 0)
        {
            //
        }
    }

    // Called via drag events in current card animator.
    public void RemoveStatsHighlights()
    {
        //
    }
}
