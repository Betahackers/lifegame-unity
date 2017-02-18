using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {


	[Header("Debug options")]
	[SerializeField]private bool _SOLO_TEST;

	[Header("User stats")]
	public int familyLevel = 50;
	public int loveLevel = 50;
	public int moneyLevel = 50;
	public int healthLevel = 50;

	[SerializeField]private int _yearsPassed;
	private readonly int _initialAge = 21;


	[Tooltip("All game's cards. We'll build the pool each turn from here")]
	public List<CardData.Settings> gameDeck = new List<CardData.Settings>();

	[Tooltip("Actual turn's cards")]
	public List<CardData.Settings> turnPool = new List<CardData.Settings>();

	[SerializeField]private CardData.Settings _activeCard;


	public static CardManager instance;


	void Start () {
		instance = this;
		//------------------
		if (_SOLO_TEST)
			StartDeck ();
	}

	//===============================================================================================================================================================================
	//===============================================================================================================================================================================
	//=====  DECK MANAGEMENT FUNCTIONS
	public void StartDeck () {
		// This function just resets things to start over
		familyLevel = 50;
		loveLevel = 50;
		moneyLevel = 50;
		healthLevel = 50;
		// Build the first pool
		BuildPool();
	}

	public void BuildPool () {
		// Erase the actual pool
		turnPool = new List<CardData.Settings>();
		// Check through all cards which ones could make into the pool
		for (int i = 0; i < gameDeck.Count; i++) {

		}
		//------------------
		if (_SOLO_TEST)
			PickCard ();
	}

	public void PickCard () {

	}

	public void RecycleCard () {

	}

	//===============================================================================================================================================================================
	//===============================================================================================================================================================================
	//=====  ACTION PLAY FUNCTIONS

}
