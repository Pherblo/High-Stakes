using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Action<CharacterData> OnCharacterStateChanged;

    [Header("Characters can be manually loaded using ContextMenu or auto loaded on Awake.")]
    [SerializeField] public string _characterResourcePath;
    [SerializeField] public string _eventsResourcePath;
    [SerializeField] public bool _loadCharactersOnAwake = false;
    [SerializeField] public CharacterData[] _characterPrefabs = new CharacterData[1];

    private List<CharacterData> _characterInstances = new();

    public List<CharacterData> CharacterInstances => _characterInstances;

    private void Awake()
    {
        if (_loadCharactersOnAwake) LoadCharacters();
    }

    [ContextMenu("Load Characters")]
    private void LoadCharacters()
    {
        _characterPrefabs = Resources.LoadAll<CharacterData>(_characterResourcePath);

        foreach (CharacterData character in _characterPrefabs)
        {
            CharacterData characterInstance = Instantiate(character);
            _characterInstances.Add(characterInstance);
        }
    }
}
