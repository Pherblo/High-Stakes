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

    private List<CharacterData> _instancedCharacterTargets = new();     // Prefab instances of characters to kill.

    public CharacterData[] CharactersToBeDead => _charactersToBeDead;
    public List<CharacterData> InstancedCharacterTargets => _instancedCharacterTargets;
    public string DialogueText => _dialogueText;

    public void AssignCharacterInstance(CharacterData characterInstance)
    {
        _instancedCharacterTargets.Add(characterInstance);
    }
}
