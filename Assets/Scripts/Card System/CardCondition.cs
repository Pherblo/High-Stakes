using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeCondition
{
    None = 0,
    Alive = 1,
    Dead = 2
}

public enum ChoiceCondition
{
    None = 0,
    ChoiceA = 1,
    ChoiceB = 2
}

[Serializable]
public class CardCondition
{
    [Header("Selected Dialogue Condition")]
    [SerializeField] private CardEvent _cardReference;
    [SerializeField] private ChoiceCondition _choiceCondition = ChoiceCondition.None;
    [Header("Character Life Condition")]
    [SerializeField] private CharacterData _characterReference;
    [SerializeField] private LifeCondition _lifeCondition = LifeCondition.None;

    public bool CheckCondition(List<CardDialogue> selectedChoices)
    {
        // Get the required choice.
        // TODO: I hate this implementation btw but I just need it to work. -Lorenzo
        string dialogueId = "";
        CardDialogue choiceCondition = null;
        if (_choiceCondition != ChoiceCondition.None)
        {
            if (_choiceCondition == ChoiceCondition.ChoiceA)
            {
                choiceCondition = _cardReference.DialogueA;
                dialogueId += _cardReference.DialogueA.DialogueText;
            }
            else if (_choiceCondition == ChoiceCondition.ChoiceB)
            {
                choiceCondition = _cardReference.DialogueB;
                dialogueId += _cardReference.DialogueB.DialogueText;
            }

            // Check if the required choice was previously chosen. If not, return false.
            if (selectedChoices.Exists((x) => x == choiceCondition))
            {
                return true;
            }
            else
            {
                Debug.Log($"Required choice was not chosen. condition text: {choiceCondition.DialogueText}, selected choices count: {selectedChoices.Count}");
            }

            if (selectedChoices.Exists((x) => x.DialogueText == dialogueId))
            {
                return true;
            }
        }

        // Evaluate character life prerequisites. TODO: Reference a list that tracks characters' life states.
        /*
        if (_lifeCondition != LifeCondition.None)
        {
            if (_choiceCondition == ChoiceCondition.ChoiceA)
            {
                choiceCondition = _cardReference.DialogueA;
            }
            else if (_choiceCondition == ChoiceCondition.ChoiceB)
            {
                choiceCondition = _cardReference.DialogueB;
            }

            // Check if the required choice was previously chosen. If not, return false.
            if (selectedChoices.Exists((x) => x == choiceCondition))
            {
                return false;
            }
        }
        */

        return false;
    }
}
