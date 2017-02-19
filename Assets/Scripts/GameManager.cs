using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public CardManager cardManager;
	public List <CardMovement> cardGOs;
	public Text question, characterName, age;
	public Image loveImage, funImage, healthImage, moneyImage;
	public GameObject loadingPanel;

	// Use this for initialization
	public void Init () {
		loadingPanel.SetActive (false);
//		cardManager.StartDeck ();
		SetPlayerData ();

		cardGOs [0].Init (OnCardSwiped, true);
		SetCardData (cardGOs [0], cardManager.PickCard ());
		cardGOs [1].Init (OnCardSwiped, false);
		SetCardData (cardGOs [1], cardManager.PickCard ());
	}

	void SetPlayerData () {
		age.text = (cardManager._initialAge + cardManager._yearsPassed).ToString ();
		loveImage.fillAmount = cardManager.loveLevel / 100f;
		funImage.fillAmount = cardManager.familyLevel / 100f;
		healthImage.fillAmount = cardManager.healthLevel / 100f;
		moneyImage.fillAmount = cardManager.moneyLevel / 100f;
	}

	void OnCardSwiped (CardMovement swipedCard) {
		CardMovement.SwipeDirection swipeDirection = swipedCard.GetSwipeResult ();
		cardManager.Swipe (swipeDirection == CardMovement.SwipeDirection.Right);
		SetCardData (swipedCard, cardManager.PickCard ());
		SetPlayerData ();
	}

	void SetCardData (CardMovement cardMovement, CardData.Settings cardData) {
		// TODO Also send the card image
		question.text = cardData.cardText;
		characterName.text = cardData.characterName.ToUpper ();
		CardData.Outcome outcome = cardData.rightOutcome;
//		Debug.Log ("outcome: " + outcome);
		cardMovement.SetAnswers (cardData.leftOutcome.description, cardData.rightOutcome.description);
	}
}
