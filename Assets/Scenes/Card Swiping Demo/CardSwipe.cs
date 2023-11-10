using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSwipe : MonoBehaviour
{

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

	}

	// Update is called once per frame
	void Update()
	{
		// Updating the animators parameters to current hover states
		animator.SetBool("hoveringA", dialogueAHover.GetComponent<HoverArea>().AIsHovering);
        animator.SetBool("hoveringB", dialogueBHover.GetComponent<HoverArea>().BIsHovering);

    }


}
