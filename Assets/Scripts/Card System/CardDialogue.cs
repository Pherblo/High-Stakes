using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardDialogue : MonoBehaviour
{
    [SerializeField] private bool _dialogueIsActive = true;
    [SerializeField] private string _dialogueOption;

    public string Description => _dialogueOption;
}
