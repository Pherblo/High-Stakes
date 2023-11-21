using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    public Action OnCardDrawFinished;

    [Header("Component References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private UIAnimator _uiAnimator;

    private void Awake()
    {
        OnCardDrawFinished += RevealCard;
    }

    public void AnimateCardDraw()
    {
        _animator.SetTrigger("DrawCard");
    }

    public void AnimateCardDiscard()
    {
        //_animator.SetTrigger("DiscardCard");
    }

    // Called via animation events.
    public void SendDrawFinishedEvent()
    {
        OnCardDrawFinished?.Invoke();
    }

    private void RevealCard()
    {
        _uiAnimator.StartEnterFade();
    }
}
