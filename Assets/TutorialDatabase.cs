using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDatabase : MonoBehaviour
{

    [Header("Characters can be manually loaded using ContextMenu or auto loaded on Awake.")]
    [SerializeField] public string _resourcePath;
    [SerializeField] private bool _loadCharactersOnAwake = true;

    public CharacterData[] _characterPrefabs = new CharacterData[1];



    private void Awake()
    {
        if (_loadCharactersOnAwake) LoadCharacters();
    }

    [ContextMenu("Load Characters")]
    private void LoadCharacters()
    {

        _characterPrefabs = Resources.LoadAll<CharacterData>(_resourcePath);
        for (int i=0; i<_characterPrefabs.Length; i++)
        {
            CharacterData characterInstance = Instantiate(_characterPrefabs[i]);
        }
            
     
    }
}
