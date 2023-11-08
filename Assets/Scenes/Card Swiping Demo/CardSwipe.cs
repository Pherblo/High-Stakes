using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

	[SerializeField] private Animator animator;

	[Header("Hover Areas")]
	[SerializeField] private Button dialogueAHover;
	[SerializeField] private Button dialogueBHover;

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
		// Updating the animators parameters to current hover states
		animator.SetBool("hoveringA", dialogueAHover.GetComponent<HoverArea>().AIsHovering);
        animator.SetBool("hoveringB", dialogueBHover.GetComponent<HoverArea>().BIsHovering);

    }


}
