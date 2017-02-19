using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <CardMovement> cardGOs;
	public Text question, age;
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

		FillParametersDisplay ();
		DisplayPlayerData ();
		DisplayCardData (cardGOs [0]);
	}

	void DisplayPlayerData () {
		age.text = (cardManager._initialAge + cardManager._yearsPassed).ToString ();
	}

	void OnCardSwiped (CardMovement swipedCard) {
		CardMovement.SwipeDirection swipeDirection = swipedCard.GetSwipeResult ();
		cardManager.Swipe (swipeDirection == CardMovement.SwipeDirection.Right);
		swipedCard.SetCardData (cardManager.PickCard ());
		FillParametersDisplay ();
		DisplayPlayerData ();
		CardMovement nextCard = (swipedCard == cardGOs [0]) ? cardGOs [1] : cardGOs [0];
		DisplayCardData (nextCard);
		resetSelectors ();
	}

	void FillParametersDisplay(){
		parameterLove.Fill(cardManager.loveLevel);
		parameterFun.Fill(cardManager.familyLevel);
		parameterHealth.Fill(cardManager.healthLevel);
		parameterMoney.Fill(cardManager.moneyLevel);
	}

	void resetSelectors(){
		parameterLove.ShowHide(false);
		parameterFun.ShowHide(false);
		parameterHealth.ShowHide(false);
		parameterMoney.ShowHide(false);
	}

	void OnCardAnswerDisplayed (int loveDelta, int funDelta, int healthDelta, int moneyDelta) {
		// TODO Implement with ParameterDisplays
		parameterLove.ShowHide (loveDelta != 0);
		parameterFun.ShowHide (funDelta != 0);
		parameterHealth.ShowHide (healthDelta != 0);
		parameterMoney.ShowHide (moneyDelta != 0);
	}

	void OnCardAnswerHidden () {
		// TODO Implement with ParameterDisplays
		parameterLove.ShowHide (false);
		parameterFun.ShowHide (false);
		parameterHealth.ShowHide (false);
		parameterMoney.ShowHide (false);
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
	}
}
