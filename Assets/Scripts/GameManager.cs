using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <CardMovement> cardGOs;
	public Text question, characterName, age;
	public Image loveImage, funImage, healthImage, moneyImage;
	public List <ParameterDisplay> parameterDisplays;

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

		DisplayPlayerData ();
		DisplayCardData (cardGOs [0]);
	}

	void DisplayPlayerData () {
		age.text = (cardManager._initialAge + cardManager._yearsPassed).ToString ();
		loveImage.fillAmount = cardManager.loveLevel / 100f;
		funImage.fillAmount = cardManager.familyLevel / 100f;
		healthImage.fillAmount = cardManager.healthLevel / 100f;
		moneyImage.fillAmount = cardManager.moneyLevel / 100f;
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
