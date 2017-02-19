using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <CardMovement> cardGOs;
	public Text question, characterName, age;
	public ParameterDisplay parameterLove;
	public ParameterDisplay parameterFun;
	public ParameterDisplay parameterHealth;
	public ParameterDisplay parameterMoney;

	private CardManager cardManager;

	// Use this for initialization
	void Start () {
		GameObject cardManagerGO = GameObject.Find ("Card Manager");
		cardManager = cardManagerGO.GetComponent <CardManager> ();

		cardGOs [0].Init (OnCardSwiped, OnCardAnswerDisplayed, OnCardAnswerHidden);
		cardGOs [0].SetCardIdle ();
		cardGOs [0].SetCardData (cardManager.PickCard ());

		cardGOs [1].Init (OnCardSwiped, OnCardAnswerDisplayed, OnCardAnswerHidden);
		cardGOs [1].SetCardData (cardManager.PickCard ());

		parameterLove.Init();
		parameterFun.Init();
		parameterHealth.Init();
		parameterMoney.Init ();

		DisplayPlayerData ();
		DisplayCardData (cardGOs [0]);
	}

	void DisplayPlayerData () {
		age.text = (cardManager._initialAge + cardManager._yearsPassed).ToString ();
		parameterLove.Fill(cardManager.loveLevel / 100f);
		parameterFun.Fill(cardManager.familyLevel / 100f);
		parameterHealth.Fill(cardManager.healthLevel / 100f);
		parameterMoney.Fill(cardManager.moneyLevel / 100f);
	}

	void OnCardSwiped (CardMovement swipedCard) {
		CardMovement.SwipeDirection swipeDirection = swipedCard.GetSwipeResult ();
		cardManager.Swipe (swipeDirection == CardMovement.SwipeDirection.Right);
		swipedCard.SetCardData (cardManager.PickCard ());

		DisplayPlayerData ();
		CardMovement nextCard = (swipedCard == cardGOs [0]) ? cardGOs [1] : cardGOs [0];
		DisplayCardData (nextCard);
	}

	void OnCardAnswerDisplayed (int loveDelta, int funDelta, int healthDelta, int moneyDelta) {
		// TODO Implement with ParameterDisplays
		parameterLove.ShowHide (loveDelta != 0);
		if (loveDelta != 0) {
			parameterLove.Fill (loveDelta);
		} 

		parameterFun.ShowHide (funDelta != 0);
		if (funDelta != 0) {
			parameterFun.Fill (funDelta);
		} 

		parameterHealth.ShowHide (healthDelta != 0);
		if (healthDelta != 0) {
			parameterHealth.Fill (healthDelta);
		} 

		parameterMoney.ShowHide (moneyDelta != 0);
		if (moneyDelta != 0) {
			parameterMoney.Fill (moneyDelta);
		}
	
	}

	void OnCardAnswerHidden () {
		// TODO Implement with ParameterDisplays
	}

	void DisplayCardData (CardMovement card) {
		card.SetCardIdle ();
		CardData.Settings cardData = card.GetCardData ();
		if (cardData == null) {
			// TODO Implement a game over
			Debug.Log ("GAME OVER!");
			return;
		}
		question.text = cardData.cardText;
		characterName.text = cardData.characterName.ToUpper ();
	}
}
