using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardRequirement : MonoBehaviour
{
    [SerializeField] private CardEvent _cardCondition;
    [SerializeField] private SelectedChoice _choiceCondition = 0;

    public bool CheckCondition(List<CardEvent> cards)
    {
        CardEvent cardRequirement = cards.Find((x) => x == _cardCondition);
        if (cardRequirement != null && cardRequirement.PickedChoice == _choiceCondition) return true;
        return false;
    }
}
