using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Action<CharacterData> OnCharacterStateChanged;

    [Header("Characters can be manually loaded using ContextMenu or auto loaded on Awake.")]
    [SerializeField] private string _resourcePath;
    [SerializeField] private bool _loadCharactersOnAwake = true;
    [SerializeField] private CharacterData[] _characters = new CharacterData[1];

    public CharacterData[] Characters => _characters;

    private void Awake()
    {
        if (_loadCharactersOnAwake) LoadCharacters();
    }

    [ContextMenu("Load Characters")]
    private void LoadCharacters()
    {
        _characters = Resources.LoadAll<CharacterData>(_resourcePath);
    }
}
