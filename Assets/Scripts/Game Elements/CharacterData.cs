using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private string _name;
    [SerializeField] private bool _isAlive = true;
    [Header("Character Cards")]
    [SerializeField] private List<CardEvent> _cardEvents = new();
    // Probably art assets here too.

    public string Name => _name;
    public bool IsAlive => _isAlive;

    public CardEvent GetCard()
    {
        int randomIndex = _cardEvents.Count;
        var newArray = _cardEvents;

        // Shuffle card events.
        _cardEvents.Sort(delegate (CardEvent x, CardEvent y)
        {
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0) return -1;
            else return 1;
        });

        foreach (CardEvent card in newArray)
        {
            if (card.CheckRequirements()) return card;
        }
        return null;
    }

    public void SetAliveState(bool isAlive)
    {
        _isAlive = isAlive;
    }
}
