using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class CardDialogue
{
    [Header("Dialogue Settings")]
    [SerializeField] private bool _dialogueIsActive = true;
    [SerializeField, TextArea] private string _dialogueText;
    // Card Effects pertain to the effects of the cards once this corresponding dialogue is selected.
    [Header("Card Results Settings")]
    [SerializeField] private CharacterData[] _charactersToBeDead = new CharacterData[0];

    public CharacterData[] CharactersToBeDead => _charactersToBeDead;
    public string DialogueText => _dialogueText;
}
