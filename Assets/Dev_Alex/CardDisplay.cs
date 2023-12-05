using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
	public UnityEvent OnDescriptionDisplayStart;
    public UnityEvent OnDescriptionDisplayEnd;

    [Header("Card UI Text References")]
	[SerializeField] private Image _cardArt;
	[SerializeField] private TextMeshProUGUI _cardName;
	[SerializeField] private TextMeshProUGUI _cardTitle;
	[SerializeField] private Typewriter _cardDescription;
	[SerializeField] private Typewriter _dialogueTextA;
    [SerializeField] private Typewriter _dialogueTextB;

    private CardEvent _currentCardEvent;

	[Header("Stats To Be Changed")]
	public Stats suspicion;
	public Stats faith;
	public Stats popularity;
    List<String> statsBeingChanged = new List<string>();

    [Header("Reference to animator")]
    public CardAnimator animator1;
    public CardAnimator animator2;

    private void Awake()
    {
        _cardDescription.OnTextGenerationFinished += SetChoices;
    }
    private void Update()
    {
        //ChangeStatsDisplays(_currentCardEvent);
    }
    // Changes dialogue depending on the swipe
    // Can be changed later to have a fade effect
    public void ToggleDialogues(CardSwipeDrag thisCardSwipe)
	{
		/*
		// Turning dialogue background back on
        _dialogueTextA.gameObject.GetComponentInParent<Image>().enabled = true;

		// Check swipe condition and change to the corresponding dialogue
		if(!thisCardSwipe.SwipeIsRight)
		{
			_dialogueTextA.RunDialogue(currentCardEvent.DialogueA.DialogueText);
		}
		else if (thisCardSwipe.SwipeIsRight)
		{
            _dialogueTextA.RunDialogue(currentCardEvent.DialogueB.DialogueText);
        }
		*/
    }

	/*public void ClearDialogues()
	{
		// Clearing the text box
        _dialogueTextA.RunDialogue("");
        // Turning off dialogue background when not in use
        _dialogueTextA.gameObject.GetComponentInParent<Image>().enabled = false;
    }*/

	public void UpdateReferences(CardEvent newCardEvent, TextMeshProUGUI newCardName, TextMeshProUGUI newCardTitle, Image newCardArt)
	{
		_currentCardEvent = newCardEvent;
		_cardName = newCardName;
		_cardTitle = newCardTitle;
		_cardArt = newCardArt;
	}

    // Subscribed to Deck's OnCardPicked event.
    public void UpdateCardDisplay(CardEvent cardToDisplay)
	{
        // Cache the card.
        _currentCardEvent = cardToDisplay;
        print(cardToDisplay);
        // Updating UI texts and art on the card.
        _cardName.text = cardToDisplay.AssociatedCharacter.Name;
		_cardTitle.text = cardToDisplay.AssociatedCharacter.Title;
        _cardArt.sprite = cardToDisplay.AssociatedCharacter.CharacterArt;

		// Update typewriters.
		//_dialogueTextA.ClearText();
        //_dialogueTextB.ClearText();
        _dialogueTextA.SetFillerText(cardToDisplay.DialogueA.DialogueText);
        _dialogueTextB.SetFillerText(cardToDisplay.DialogueB.DialogueText);
        SetDescriptionText(cardToDisplay.Description);

		OnDescriptionDisplayStart?.Invoke();

        //_cardDescription.RunDialogue(cardToDisplay.Description);
        //_dialogueTextA.RunDialogue(cardToDisplay.DialogueA.DialogueText);
        //_dialogueTextB.RunDialogue(cardToDisplay.DialogueB.DialogueText);
       
	}

    public void ExitDisplay()
    {
        // Update typewriters materials.
        _cardDescription.SetAllTexts(_currentCardEvent.Description);
        _dialogueTextA.SetAllTexts(_currentCardEvent.DialogueA.DialogueText);
        _dialogueTextB.SetAllTexts(_currentCardEvent.DialogueB.DialogueText);

        _cardDescription.CloseDialogue();
        _dialogueTextA.CloseDialogue();
        _dialogueTextB.CloseDialogue();
    }

	public void ClearDisplay()
	{
        _cardDescription.ClearText();
        _dialogueTextA.ClearText();
        _dialogueTextB.ClearText();
    }

    private void SetDescriptionText(string text)
	{
		_cardDescription.SetFillerText(text);
        _cardDescription.RunDialogue(text);
    }

	// Called by card description event after it finishes generating its text.
	private void SetChoices()
	{
		OnDescriptionDisplayEnd?.Invoke();
        _dialogueTextA.RunDialogue(_currentCardEvent.DialogueA.DialogueText);
        _dialogueTextB.RunDialogue(_currentCardEvent.DialogueB.DialogueText);
    }

    private void ChangeStatsDisplays( CardEvent cardToDisplay)
    {
        cardToDisplay.FindStatsChanged();
        List<String> emptyList = new List<String>();
        if(animator1.GetLerpValue() == -1 || animator2.GetLerpValue() == -1) //check which way the card is leaning to highlight those stats
        {
            statsBeingChanged = cardToDisplay.GetStatsChanged('A');
        } else if (animator2.GetLerpValue() == 1 || animator1.GetLerpValue() == 1) {
            statsBeingChanged = cardToDisplay.GetStatsChanged('B');
        } else
        {
            statsBeingChanged = emptyList; //if its not leaning left or right highlight nothing
        }
        // Updating Stat bottles from last card
        suspicion.changeValue(cardToDisplay.GetStatsValue(suspicion));
        faith.changeValue(cardToDisplay.GetStatsValue(faith));
        popularity.changeValue(cardToDisplay.GetStatsValue(popularity));
        string resultA = "";
        string resultB = "";
        for (int i=0; i< cardToDisplay.GetStatsChanged('A').Count; i++)
        {
            resultA = resultA + cardToDisplay.GetStatsChanged('A')[i] + ", ";
        }

        for (int i = 0; i < cardToDisplay.GetStatsChanged('B').Count; i++)
        {
            resultB= resultA + cardToDisplay.GetStatsChanged('B')[i] + ", ";
        }
       // print ("statsChanged B: " + resultB);
       // print("statsChanged A: " + resultA);
        for (int i =0; i<statsBeingChanged.Count; i++)
        {
            if(statsBeingChanged[i].ToLower().Equals( "suspicion"))
            {
                suspicion.Glow();
            }
            if (statsBeingChanged[i].ToLower().Equals("faith"))
            {
                faith.Glow();
            }
            if (statsBeingChanged[i].ToLower().Equals("popularity"))
                {
                popularity.Glow();
            }
        }
    }
}
