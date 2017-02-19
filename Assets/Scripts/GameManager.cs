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

	//Game parameters
	public static int INITIAL_LEVEL=25; //initial level filled on the parameters health, love...
	public static int MAX_LEVEL=50; // maximum level can reach in the game 100% for parameters heath, love, etc
	public static int MAX_AGE=100; // max age can reach the player

	private CardManager cardManager;

	public enum GameOverReason {Aged, FullLove, FullFun, FullHealth, FullMoney,NoLove, NoFun, NoHealth, NoMoney, OutOfCards, Alive};


	public enum ParameterType {Love, Fun, Health, Money};
	// Use this for initialization
	void Start () {
		GameObject cardManagerGO = GameObject.Find ("Card Manager");
		cardManager = cardManagerGO.GetComponent <CardManager> ();

		cardGOs [0].Init (OnCardSwiped, OnCardAnswerDisplayed, OnCardAnswerHidden);
		cardGOs [0].SetCardIdle ();
		cardGOs [0].SetCardData (cardManager.PickCard ());

		cardGOs [1].Init (OnCardSwiped, OnCardAnswerDisplayed, OnCardAnswerHidden);
		cardGOs [1].SetCardData (cardManager.PickCard ());

		parameterLove.Init(GameManager.INITIAL_LEVEL);
		parameterFun.Init(GameManager.INITIAL_LEVEL);
		parameterHealth.Init(GameManager.INITIAL_LEVEL);
		parameterMoney.Init (GameManager.INITIAL_LEVEL);

		resetSelectors ();	
		DisplayPlayerData ();
		DisplayCardData (cardGOs [0]);
	}

	void DisplayPlayerData () {
		age.text = (cardManager._initialAge + cardManager._yearsPassed).ToString ();
	}

	void OnCardSwiped (CardMovement swipedCard) {
		CardMovement.SwipeDirection swipeDirection = swipedCard.GetSwipeResult ();
		cardManager.Swipe (swipeDirection == CardMovement.SwipeDirection.Right);
		FillParametersDisplay ();
		DisplayPlayerData ();

		CardData.Settings card = cardManager.PickCard ();

		CardMovement nextCard = (swipedCard == cardGOs [0]) ? cardGOs [1] : cardGOs [0];
		GameOverReason overReason= CheckGameOver (nextCard);
		if (overReason == GameOverReason.Alive) {
			swipedCard.SetCardData (card);
			DisplayCardData (nextCard);
			resetSelectors ();	
		} else {
			// TODO Implement a game over
			Debug.Log ("GAME OVER! reason: " + overReason.ToString());
		}
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
		resetSelectors ();
	}

	void DisplayCardData (CardMovement card) {
		card.SetCardIdle ();
		question.text =card.GetCardData ().cardText;
	}

	GameOverReason CheckGameOver(CardMovement card){
		Debug.Log ("h" +cardManager.healthLevel+ "L" +cardManager.loveLevel+ "M"+cardManager.moneyLevel+"F" +cardManager.familyLevel);
		CardData.Settings cardData = card.GetCardData ();
		if (cardData == null) {
			return GameOverReason.OutOfCards;
		}
		if (cardManager._yearsPassed > MAX_AGE)
			return GameOverReason.Aged;
		
		if (cardManager.loveLevel <= 0) {
			return GameOverReason.NoLove;
		}
		if (cardManager.loveLevel >= MAX_LEVEL) {
			return GameOverReason.FullLove;
		}
		if(cardManager.moneyLevel <= 0){
			return GameOverReason.NoMoney;
		}
		if (cardManager.moneyLevel >= MAX_LEVEL) {
			return GameOverReason.FullMoney;
		}
		if(cardManager.healthLevel <= 0){
			return GameOverReason.NoHealth;
		}
		if(cardManager.healthLevel >= MAX_LEVEL){
			return GameOverReason.FullHealth;
		}
		if(cardManager.familyLevel <= 0){
			return GameOverReason.NoFun;
		}
		if(cardManager.familyLevel >= MAX_LEVEL){
			return GameOverReason.FullFun;
		}

		return GameOverReason.Alive;
	}
}
