using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Enum list of options for selected dialogues
public enum SelectedChoice
{
	None = 0,
    ChoiceA = 1,	// Left.
    ChoiceB = 2		// Right.
}

// Is on a card prefab
public class CardEvent : CardBase
{
	public Action<CardEvent> OnDialogueSelected;

	[Header("Card Settings")]
	[SerializeField] private CharacterData associatedCharacter;
	[SerializeField, TextArea] private string _description;
	[SerializeField] private bool _guaranteedCard = false;
	// [SerializeField] private bool _guaranteedCard = false; // If true, this card will be played next once requirements are met.

	[Header("stat change values")]

	private List<String> statsChanged = new List<string>();
	private float suspicionValue =0;
    private float faithValue = 0;
    private float popularityValue = 0;  //this is a quick fix can be changed later

    public float suspicionValueA = 0;
    public float faithValueA = 0;
    public float popularityValueA = 0;  //this is a quick fix can be changed later

    public float suspicionValueB = 0;
    public float faithValueB = 0;
    public float popularityValueB = 0;  //this is a quick fix can be changed later

    [Header("Dialogues")]
	[SerializeField] private CardDialogue _dialogueA;
	[SerializeField] private CardDialogue _dialogueB;
	[Header("Conditions")]
	// [SerializeField] private List<CardDialogue> _dialogueRequirements = new();
	[SerializeField] private List<CardCondition> _conditions = new();

	private SelectedChoice _pickedChoice = SelectedChoice.None;

	public CharacterData AssociatedCharacter => associatedCharacter;
	public string Description => _description;
	public bool GuaranteedCard => _guaranteedCard;
	public CardDialogue DialogueA => _dialogueA;
	public CardDialogue DialogueB => _dialogueB;
	// public bool GuaranteedCard => _guaranteedCard;
	public SelectedChoice PickedChoice => _pickedChoice;

	private Deck _deckInstance;

    public override CardEvent GetCard()
    {
		return this;
    }

    private void Update()
    {
		FindStatsChanged(); //find the list of stats changed
    }

    // TODO: Improve this. Likely separate initial associatedCharacter variable from instanced associatedCharacter variable.
    public void AssignCharacter(CharacterData associatedCharacterInstance)
	{
        associatedCharacter = associatedCharacterInstance;
	}

	public void AssignDeck(Deck deck)
	{
		_deckInstance = deck;

    }

    // Called via events.
    public void ChooseDialogue(int optionInt)
	{
		// Maps int to a SelectedDialogue
		//SelectedChoice option = (SelectedChoice)optionInt;
		/*
		// Add corresponding selected dialogue to deck selected dialogue list
		if(option == SelectedChoice.ChoiceA)
		{
			_deck.SelectedDialogues.Add(_dialogueA);
        }
		else if(option == SelectedChoice.ChoiceB)
		{
			_deck.SelectedDialogues.Add(_dialogueB);
		}*/
		_pickedChoice = (SelectedChoice)optionInt;
        OnDialogueSelected?.Invoke(this);

        if (_pickedChoice == SelectedChoice.ChoiceA)
		{
			suspicionValue = suspicionValueA;
			faithValue = faithValueA;
			popularityValue= popularityValueA;
			
		} else if (_pickedChoice == SelectedChoice.ChoiceB)
		{
			suspicionValue = suspicionValueB;
			faithValue = faithValueB;
			popularityValue= popularityValueB;	
		}
    }

	public override bool CheckRequirements()
	{
		// If dialogue has already been picked, this card isn't available anymore.
		if (_pickedChoice != SelectedChoice.None)
		{
			return false;
		}

		foreach (CardCondition condition in _conditions)
		{
			if (condition.CheckCondition(_deckInstance.SelectedDialogues))
			{
				print("condition true");
				return true;
			}
			else return false;
		}

		// Returns true by default if there are no conditions.
		return true;

		// Following comments probably don't work and are disabled for now. Meaning conditions won't work.
		// For each of this cards dialogues requirements check if it is in selected dialogues
		/*
		for(int i = 0; i < _dialogueRequirements.Count; i++)
		{
            // Checks if each required dialogue is in the selected dialogues
            if (_deck.selectedDialogues.Contains(_dialogueRequirements[i]))
			{
				// Just continue checking
            }
			else
			{
				// Returns bool right away if any are not included in selected dialogues
				return false;
			}
        }
		*/
	}

	// Just a debug to use in a button to see if this works
    public void test()
    {
		Debug.Log("Requirements met for this card: " + CheckRequirements());
    }

	private void FindStatsChanged()
	{
        if (suspicionValue != suspicionValueA || suspicionValue != suspicionValueB)
        {
            statsChanged.Add("suspicion");
        }

        if (faithValue != faithValueA || faithValue != faithValueB)
        {
            statsChanged.Add("faith");
        }

        if (popularityValue != popularityValueA || popularityValue != popularityValueB)
        {
            statsChanged.Add("popularity");
        }
    }

	public List<String> GetStatsChanged()
	{
		return statsChanged;
	}

	public float GetStatsValue(Stats stat)
	{
		float statValue = 0;
		if(stat.name == "popularity")
		{
			statValue = popularityValue;
		} else if(stat.name == "faith")
		{
			statValue = faithValue;
		} else if( stat.name == "suspicion")
		{
			statValue = suspicionValue;
		}
		return statValue;
	}

}
