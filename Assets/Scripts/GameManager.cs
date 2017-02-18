using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public List <CardMovement> cardGOs;
	public Text question, characterName;
	public Image loveImage, funImage, healthImage, moneyImage;

	private int cardCounter;

	// Use this for initialization
	void Start () {
		cardCounter = 0;
		cardGOs [0].Init (OnCardSwiped, true);
		cardGOs [0].SetAnswers ("Yes " + cardCounter, "No " + cardCounter);
		cardCounter++;
		cardGOs [1].Init (OnCardSwiped, false);
		cardGOs [1].SetAnswers ("Yes " + cardCounter, "No " + cardCounter);
		cardCounter++;
	}

	void OnCardSwiped (CardMovement swipedCard) {
		CardMovement.SwipeDirection swipeDirection = swipedCard.GetSwipeResult ();
		swipedCard.SetAnswers ("Yes " + cardCounter, "No " + cardCounter);
		cardCounter++;
	}
}
