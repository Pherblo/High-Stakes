using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstance
{
    private CharacterData _data;
    private bool _isAlive;

    public CharacterData Data => _data;
    public bool IsAlive => _isAlive;

    public bool GetAliveState()
    {
        return _isAlive;
    }

    public CharacterInstance(CharacterData newData)
    {
        _data = newData;
    }

    public bool ChangeState(bool isAlive) => _isAlive = isAlive;
}
