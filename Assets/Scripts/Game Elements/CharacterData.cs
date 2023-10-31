using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Event", menuName = "Cards/Card Event")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string _name;
    // Probably art assets here too.

    public string Name => _name;
}
