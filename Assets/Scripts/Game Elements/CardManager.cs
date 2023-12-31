using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    // This script handles game logic. This was previously done via events but it caused some headaches. Therefore, everything will be aggregated to this script for convenience's sake.
    // This is practically a game manager.

    public UnityEvent OnCardPickStart;
    public UnityEvent OnCardDisplayStart;

    [Header("Scene References")]
    [SerializeField] private Deck _deck;
    [SerializeField] private CardDisplay _cardDisplay;
    [SerializeField] private StatsManager _statsManager;
    [SerializeField] CardAnimator[] _cardAnimators;
    [Header("Physical Card Settings")]
    [SerializeField] private Transform _startingTransformValues;

    private CardAnimator _currentCardAnimator = null;
    private int _currentCardAnimatorIndex = 0;
    private CardEvent _currentCardEvent;

    private void Awake()
    {
        _currentCardAnimatorIndex = 0;
        _currentCardAnimator = _cardAnimators[0];

        // Subscribe to input events.
        foreach (CardAnimator cardAnim in _cardAnimators)
        {
            cardAnim.OnChoiceChosen += ChooseChoice;
            cardAnim.OnDragDirectionChanged += HighlightStats;
            cardAnim.OnCardSnapback += _statsManager.RemoveStatsHighlights;
            cardAnim.OnCardSwiped.AddListener(_statsManager.RemoveStatsHighlights);
        }
    }

    private void Start()
    {
        foreach (CardAnimator card in _cardAnimators)
        {
            card.transform.position = _startingTransformValues.position;
            card.transform.rotation = _startingTransformValues.rotation;
            card.OnCardDrawFinished += StartDisplayingCard;
            card.OnCardDiscardStart += StartDiscardingCard;
        }
        StartPickCard();
        _cardDisplay.ClearDisplay();
    }

    public void StartPickCard()
    {
        _currentCardEvent = _deck.PickCard();

        // Update cached card.
        _currentCardAnimatorIndex = (_currentCardAnimatorIndex + 1) % _cardAnimators.Length;
        _currentCardAnimator = _cardAnimators[_currentCardAnimatorIndex];

        // Assign new references to CardDisplay.
        _cardDisplay.UpdateReferences(_currentCardEvent, _currentCardAnimator.CharacterName, _currentCardAnimator.CharacterTitle, _currentCardAnimator.CardArt);

        // Start animations. Wait for animation to end (checked via events).
        _currentCardAnimator.AnimateCardDraw();
        OnCardPickStart?.Invoke();
    }

    // Called after card has been drawn and card gameobject is in position.
    public void StartDisplayingCard()
    {
        _currentCardAnimator.RevealCard();
        _cardDisplay.UpdateCardDisplay(_currentCardEvent);

        OnCardDisplayStart?.Invoke();
    }

    public void StartDiscardingCard()
    {
        _cardDisplay.ExitDisplay();
        StartPickCard();
    }

    private void ChooseChoice(int direction)
    {
        if (direction == -1)
        {
            _currentCardEvent.ChooseDialogue(1);
            _deck.ProcessCard(_currentCardEvent);
        }
        else
        {
            _currentCardEvent.ChooseDialogue(2);
            _deck.ProcessCard(_currentCardEvent);
        }

        _statsManager.ModifyStats(_currentCardEvent);
    }

    private void HighlightStats(int dragInput)
    {
        _statsManager.HighlightStats(_currentCardEvent, dragInput);
    }
}
