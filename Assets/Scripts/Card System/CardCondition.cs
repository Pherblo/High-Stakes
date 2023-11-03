using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardCondition
{
    [SerializeField] private CardEvent _cardCondition;
    [SerializeField] private SelectedChoice _choiceCondition = 0;

    public bool CheckCondition(List<CardDialogue> selectedChoices)
    {
        // Get the required choice.
        CardDialogue choiceCondition;
        if (_choiceCondition == SelectedChoice.ChoiceA)
        {
            choiceCondition = _cardCondition.DialogueA;
        }
        else
        {
            choiceCondition = _cardCondition.DialogueB;
        }

        // Check if the required choice was previously chosen.
        bool hasSelectedChoice = selectedChoices.Exists((x) => x == choiceCondition);
        if (hasSelectedChoice) return true;
        return false;
    }
}
