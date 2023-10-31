using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class CardDialogue
{
    [SerializeField] private string _dialogueOption;

    public string Description => _dialogueOption;
}
