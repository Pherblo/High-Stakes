using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardDialogue : MonoBehaviour
{
    [SerializeField] private bool dialogueIsActive = true;
    [SerializeField] private string dialogueOption;

    public string Dialogue => dialogueOption;
}
