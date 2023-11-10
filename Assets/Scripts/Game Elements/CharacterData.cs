using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterData : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private string _name;
    [SerializeField] private bool _isAlive = true;
    [Header("Character Cards")]
    [SerializeField] private List<CardEvent> _cardEvents = new();

    // Probably art asset fields here too.

    private List<CardEvent> _cardEventInstances = new();        // Instantiated cards.

    public string Name => _name;
    public bool IsAlive => _isAlive;

    // Called by Deck to create the prefabs. Returns self prefab.
    public void InitializeCharacter()
    {
        foreach (CardEvent card in _cardEvents)
        {
            CardEvent cardEventInstance = Instantiate(card, transform);
            _cardEvents.Add(cardEventInstance);
        }
    }

    public CardEvent GetCard()
    {
        int randomIndex = _cardEvents.Count;
        var newArray = _cardEvents;

        // Shuffle card events.
        System.Random rng = new System.Random();
        _cardEvents = _cardEvents.OrderBy((x) => rng.Next()).ToList();

        // Return the first available card, else return nothing.
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
