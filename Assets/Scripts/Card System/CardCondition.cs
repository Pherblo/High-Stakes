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
    [Header("Stat Conditions")]
    [SerializeField] private float _suspicionRequirement = 0;
    [SerializeField] private float _soulsRequirement = 0;
    [SerializeField] private float _popularityRequirement = 0;
    [Header("Character Life Condition")]
    [SerializeField] private CharacterData _characterReference;
    [SerializeField] private LifeCondition _lifeCondition = LifeCondition.None;

    public bool CheckCondition(List<CardDialogue> selectedChoices, Deck deck)
    {
        // Get the required choice.

        // TODO: I hate this id implementation btw but I just need it to work. -Lorenzo
        string dialogueId = "";

        // Parts of this code is commented cuz i'll work on them later to replace dialogueId.
        //CardDialogue choiceCondition = null;
        if (_choiceCondition != ChoiceCondition.None && _cardReference != null)
        {
            if (_choiceCondition == ChoiceCondition.ChoiceA)
            {
                //choiceCondition = _cardReference.DialogueA;
                dialogueId += _cardReference.DialogueA.DialogueText;
            }
            else if (_choiceCondition == ChoiceCondition.ChoiceB)
            {
                //choiceCondition = _cardReference.DialogueB;
                dialogueId += _cardReference.DialogueB.DialogueText;
            }

            // Check if the required choice was previously chosen. If not, return false.
            /*if (selectedChoices.Exists((x) => x == choiceCondition))
            {
                return true;
            }
            else
            {
                Debug.LogWarning($"Required choice was not chosen. condition text: {choiceCondition.DialogueText}, selected choices count: {selectedChoices.Count}");
            }*/

            // Check if selected dialogue exists.
            if (!selectedChoices.Exists((x) => x.DialogueText == dialogueId))
            {
                return false;
            }
        }

        if (deck.Stats.Suspicion.getValue() < _suspicionRequirement
                || deck.Stats.Souls.getValue() < _soulsRequirement
                || deck.Stats.Popularity.getValue() < _popularityRequirement)
        {
            return false;
        }

        return true;
    }
}
