using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private string _name;
    [SerializeField] private bool _isAlive = true;
    // Probably art assets here too.

    public string Name => _name;
    public bool IsAlive => _isAlive;

    public void SetAliveState(bool isAlive)
    {
        _isAlive = isAlive;
    }
}
