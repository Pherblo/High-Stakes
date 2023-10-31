using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Card Event", menuName = "Cards/Card Event")]
public class CardEvent : ScriptableObject
{
    public Action<CardEvent> OnDialogueSelected;

    [Header("Card Settings")]
    [SerializeField] private CharacterData _associatedCharacter;
    [SerializeField] private string _description;
    [SerializeField] private bool _guaranteedCard = false;      // If true, this card will be played next once requirements are met.
    // Card requirements go here.
    [Header("Dialogues")]
    [SerializeField] private CardDialogue _dialogueA;
    [SerializeField] private CardDialogue _dialogueB;

    private CardDialogue _chosenDialogue = null;

    public CardDialogue ChosenDialogue => _chosenDialogue;
    public bool GuaranteedCard => _guaranteedCard;

    // Called by player input via GUI.
    public void ChooseDialogue(int dialogueChosen)
    {
        if (dialogueChosen == 0) _chosenDialogue = _dialogueA;
        else _chosenDialogue = _dialogueB;
        OnDialogueSelected?.Invoke(this);
    }

    public bool CheckRequirements()
    {
        // TODO: IMPLEMENT FUNCTIONALITY.
        return true;
    }
}
