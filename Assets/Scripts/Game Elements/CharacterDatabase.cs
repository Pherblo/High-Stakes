using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Action<CharacterInstance> OnCharacterStateChanged;

    [SerializeField] private string _resourcePath;

    private Dictionary<CharacterData, CharacterInstance> _characterDatabase = new();

    private void Awake()
    {
        CharacterData[] characters = Resources.LoadAll<CharacterData>(_resourcePath);
        foreach (CharacterData character in characters)
        {
            CharacterInstance newInstance = new CharacterInstance(character);
            _characterDatabase.Add(character, newInstance);
        }
    }

    public void ChangeCharacterState(CharacterData data, bool isAlive)
    {
        CharacterInstance selectedCharacter = _characterDatabase[data];
        selectedCharacter.ChangeState(isAlive);
        OnCharacterStateChanged?.Invoke(selectedCharacter);
    }
}
