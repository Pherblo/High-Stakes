using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // This script handles game logic. This was previously done via events but it caused some headaches. Therefore, everything will be aggregated to this script for convenience's sake.
    [Header("Scene References")]
    [SerializeField] private Deck _deck;
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] CardAnimator[] _cardAnimators;

    private CardAnimator _currentCardAnimator = null;
    private int _currentCardAnimatorIndex = 0;
    private CardEvent _currentCardEvent;

    private void Awake()
    {
        _currentCardAnimatorIndex = 0;
        _currentCardAnimator = _cardAnimators[0];

        foreach (CardAnimator card in _cardAnimators)
        {
            card.transform.position = Vector3.right * 100f;
            card.OnCardDrawFinished += StartDisplayingCard;
        }
    }

    public void StartPickCard()
    {
        _currentCardEvent = _deck.PickCard();

        // Update cached card.
        _currentCardAnimatorIndex = _currentCardAnimatorIndex % _cardAnimators.Length;
        _currentCardAnimator = _cardAnimators[_currentCardAnimatorIndex];

        // Assign new references to CardDisplay.
        _cardDisplay.UpdateReferences(_currentCardAnimator.CharacterName, _currentCardAnimator.CharacterTitle, _currentCardAnimator.CardArt);

        // Start animations. Wait for animation to end (checked via events).
        _currentCardAnimator.AnimateCardDraw();
    }

    public void StartDisplayingCard()
    {
        _currentCardAnimator.RevealCard();

        _cardDisplay.UpdateCardDisplay(_currentCardEvent);
    }
}
