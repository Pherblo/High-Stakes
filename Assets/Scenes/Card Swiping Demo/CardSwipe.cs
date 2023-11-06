using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Unity.VisualScripting;

public class CardSwipe : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text characterDialogue;
    [SerializeField] private TMP_Text dialogueAText;
    [SerializeField] private TMP_Text dialogueBText;

    private CardDialogue cardDialogueA;
    private CardDialogue cardDialogueB;

    private CharacterData characterData;
    private CardEvent cardEvent;

    [Header("Highlighting Option A")]
    [SerializeField] private UnityEvent highlightA;

    [Header("Highlighting Option B")]
    [SerializeField] private UnityEvent highlightB;

    [Header("Changing Cards")]
    [SerializeField] private UnityEvent cardIsChosen;

    private bool isDragging;

    public bool IsDragging => isDragging;

    // Start is called before the first frame update
    void Start()
    {
        // FOR TESTING

        // Referencing
        cardEvent = gameObject.GetComponent<CardEvent>();
        characterData = gameObject.GetComponent<CharacterData>();
        cardDialogueA = cardEvent.DialogueA;
        cardDialogueB = cardEvent.DialogueB;

        // Setting Text Meshes
        cardName.text = "<Incr> " + characterData.Name + "</Incr>";
        characterDialogue.text = cardEvent.Description;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && (MousePosition))
        //{

        //}
    }

    private void OnMouseDown()
    {

    }
}
