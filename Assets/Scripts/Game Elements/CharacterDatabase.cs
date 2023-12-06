using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Action<CharacterData> OnCharacterStateChanged;

    [Header("Resource paths: ")]
    [SerializeField] public string _characterResourcePath;
    [SerializeField] public string _eventsResourcePath;

    [SerializeField] public CharacterData[] _characterPrefabs = new CharacterData[1];

    private List<CharacterData> _characterInstances = new();

    public List<CharacterData> CharacterInstances => _characterInstances;

    private void Awake()
    {
        LoadCharacters(); //load first characters
    }

    [ContextMenu("Load Characters")]
    public void LoadCharacters()
    {
        _characterPrefabs = Resources.LoadAll<CharacterData>(_characterResourcePath);

        foreach (CharacterData character in _characterPrefabs)
        {
            CharacterData characterInstance = Instantiate(character);
            _characterInstances.Add(characterInstance);
        }
    }
}
