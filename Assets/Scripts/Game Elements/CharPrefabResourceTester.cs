using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharPrefabResourceTester : MonoBehaviour
{
    public string ResourcePath;

    public CharPrefabTest[] Chars;

    private void Start()
    {
        Chars = Resources.LoadAll<CharPrefabTest>(ResourcePath);
        foreach(var character in Chars)
        {
            print(character.Name);
        }
    }
}
