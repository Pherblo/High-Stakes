using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDatabase : MonoBehaviour
{

    [Header("Characters can be manually loaded using ContextMenu or auto loaded on Awake.")]
    [SerializeField] private string _resourcePath;
    [SerializeField] private bool _loadCharactersOnAwake = true;
    [SerializeField] private CharacterData[] _characterPrefabs = new CharacterData[1];

    private List<CharacterData> _characterInstances = new();

    public List<CharacterData> CharacterInstances => _characterInstances;

    private void Awake()
    {
        if (_loadCharactersOnAwake) LoadCharacters();
    }

    [ContextMenu("Load Characters")]
    private void LoadCharacters()
    {
        _characterPrefabs = Resources.LoadAll<CharacterData>(_resourcePath);

        foreach (CharacterData character in _characterPrefabs)
        {
            CharacterData characterInstance = Instantiate(character);
            _characterInstances.Add(characterInstance);
        }
    }
}
