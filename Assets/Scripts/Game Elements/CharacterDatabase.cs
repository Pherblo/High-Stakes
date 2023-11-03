using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Action<CharacterData> OnCharacterStateChanged;

    [SerializeField] private string _resourcePath;

    private CharacterData[] _characterDatabase = new CharacterData[1];

    private void Awake()
    {
        _characterDatabase = Resources.LoadAll<CharacterData>(_resourcePath);
    }

    public void ChangeCharacterState(CharacterData character, bool isAlive)
    {
        character.SetAliveState(isAlive);
        OnCharacterStateChanged?.Invoke(character);
    }
}
