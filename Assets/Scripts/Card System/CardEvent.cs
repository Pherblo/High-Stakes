using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Event", menuName = "Cards/Card Event")]
public class CardEvent : ScriptableObject
{
    public Action OnCardDialogueSelected;

    [Header("Card Settings")]
    [SerializeField] private string _description;
    [Header("Dialogues")]
    [SerializeField] private CardDialogue[] _dialogues = new CardDialogue[2];

    private int _chosenDialogue = 0;

    public bool CheckRequirements()
    {
        // TODO: IMPLEMENT FUNCTIONALITY.
        return true;
    }
}
