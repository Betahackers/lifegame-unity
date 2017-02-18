﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {


	[Header("Debug options")]
	[SerializeField]private bool _SOLO_TEST;
	[SerializeField]private bool _LINEAR_MODE;

	[Header("User stats")]
	public int familyLevel = 50;
	public int loveLevel = 50;
	public int moneyLevel = 50;
	public int healthLevel = 50;

	[SerializeField]public int _yearsPassed;
	public readonly int _initialAge = 21;


	[Tooltip("All game's cards. We'll build the pool each turn from here")]
	public List<CardData.Settings> gameDeck = new List<CardData.Settings>();

	[Tooltip("Actual turn's cards")]
	public List<CardData.Settings> turnPool = new List<CardData.Settings>();

	[SerializeField]private CardData.Settings _activeCard;


	public static CardManager instance;

	private int _linearCardIndex;
	private ManagerGUI _ui;


	void Start () {
		instance = this;
		//------------------
		if (_SOLO_TEST) {
			_ui = GetComponent<ManagerGUI> ();
			StartDeck ();
		}
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

	private void BuildPool () {
		// Erase the actual pool
		turnPool = new List<CardData.Settings>();

		if (_LINEAR_MODE) {
			// In Linear Mode, we'll just pick the next card in the list
			turnPool.Add (gameDeck [_linearCardIndex]);
			_linearCardIndex++;
		} else {
			// Check through all cards which ones could make into the pool
			for (int i = 0; i < gameDeck.Count; i++) {
				// If we used this card too  much, it's automatically discarded
				if (!CheckUsage (gameDeck [i]))
					return;
				// First of all: the card must be inside the age range
				if (gameDeck [i].chances.ageConstraint == 0)
					turnPool.Add (gameDeck [i]);
				else {
					// Younger cards
					if (gameDeck [i].chances.ageConstraint < 0 && gameDeck [i].chances.ageConstraint <= _initialAge + _yearsPassed) {
						if (CheckChance (gameDeck [i]))
							turnPool.Add (gameDeck [i]);
					} else if (gameDeck [i].chances.ageConstraint > 0 && gameDeck [i].chances.ageConstraint >= _initialAge + _yearsPassed) {
						// Older cards
						if (CheckChance (gameDeck [i]))
							turnPool.Add (gameDeck [i]);
					}
				}
			}
		}
		// In case we run out of cards, throw a reference error
		if (turnPool.Count == 0)
			Debug.LogError ("THERE'S NO CARDS IN THE TURN POOL! THIS COULD NOT HAPPEN!!!!!! POLISH THE DATABASE!!!!!!!");
		//------------------
		if (_SOLO_TEST)
			PickCard ();
	}

	private bool CheckChance (CardData.Settings cdata) {
		bool answer = false;
		// If there's no chance to manage, it's approved
		if (cdata.chances.ageChanceIncrement == 0f)
			answer = true;
		else {
			float chance = cdata.chances.ageChanceBase + (cdata.chances.ageChanceIncrement * (float)_yearsPassed);
			if (Random.Range (0f, 100f) <= chance)
				answer = true;
		}
		// return the check
		return answer;
	}

	private bool CheckUsage (CardData.Settings cdata) {
		return cdata.chances.maxUse <= 0 || (cdata.chances.maxUse > 0 && cdata.chances.currentUsage < cdata.chances.maxUse);
	}

	public CardData.Settings PickCard () {
		// Pick a card at random from the pool
		_activeCard = turnPool[Random.Range(0, turnPool.Count)];
		// Count our uses. If we reach the limit, remove this card from the game
		_activeCard.chances.currentUsage++;
		// If set, update the temp GUI
		if (_SOLO_TEST)
			DrawCard ();
		return _activeCard;
	}

	private void DrawCard () {
		_ui.familiLabel.text = "Family Level: " + familyLevel;
		_ui.loveLabel.text = "Love Level: " + loveLevel;
		_ui.moneyLabel.text = "Money Level: " + moneyLevel;
		_ui.healthLabel.text = "Health Level: " + healthLevel;

		_ui.probabilityLabel.text = "Probability: " + _activeCard.chances.probability;
		_ui.ageConstraintLabel.text = "Age Constraint: " + _activeCard.chances.ageConstraint;
		_ui.ageChanceBase.text = "Age Chance Base: " + _activeCard.chances.ageChanceBase;
		_ui.ageChanceIncrementLabel.text = "Age chance Increment: " + _activeCard.chances.ageChanceIncrement;
		_ui.maxUsageLabel.text = "Usages: " + _activeCard.chances.maxUse + "/" + _activeCard.chances.currentUsage;
	}

	//===============================================================================================================================================================================
	//===============================================================================================================================================================================
	//=====  ACTION PLAY FUNCTIONS
	public void Swipe (bool rightSwipe) {
		// Pick the right outcome to work with
		CardData.Outcome outcome = rightSwipe == true ? _activeCard.rightOutcome : _activeCard.leftOutcome;
		// Apply the outcome
		familyLevel += outcome.family;
		loveLevel += outcome.love;
		moneyLevel += outcome.money;
		healthLevel += outcome.health;
		// Pass the time
		_yearsPassed += Random.Range(1, 5);
		BuildPool ();

		if (_SOLO_TEST)
			DrawCard ();
	}
}
