using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public GameObject answerPanel;
	public Text answerText;
	public Image cardImage;
	public Text characterName;

	public float movingSpeed = 1500f;
	public float swipingSpeed = 2500f;
	public float minSwipeDistance = 10f;
	public float yLimit = 10f;

	private OnCardSwiped onCardSwiped;
	private State state;
	private Vector3 startPosition, targetPosition;
	private SwipeDirection swipeDirection;
	private Vector3 fingerOffset;
	private CardData.Settings cardData;
	private OnAnswerDisplayed onAnswerDisplayed;
	private OnAnswerHidden onAnswerHidden;

	enum State {Hidden, Idle, Dragging, MovingBack, Swiping};
	public enum SwipeDirection {Left, Right};
	public delegate void OnCardSwiped (CardMovement swipedCard);
	public delegate void OnAnswerDisplayed (int loveDelta, int funDelta, int healthDelta, int moneyDelta);
	public delegate void OnAnswerHidden ();

	void Start () {
		this.startPosition = this.transform.position;
	}

	public void Init (OnCardSwiped onCardSwiped, OnAnswerDisplayed onAnswerDisplayed, OnAnswerHidden onAnswerHidden) {
		this.onCardSwiped = onCardSwiped;
		this.onAnswerDisplayed = onAnswerDisplayed;
		this.onAnswerHidden = onAnswerHidden;
		this.state = State.Hidden;
	}

	public void SetCardIdle () {
		this.state = State.Idle;
	}

	public CardData.Outcome GetSwipeOutcome () {
		CardData.Outcome outcome = (this.swipeDirection == SwipeDirection.Left) ? this.cardData.leftOutcome : this.cardData.rightOutcome;
		return outcome;
	}

	public void SetCardData (CardData.Settings cardData) {
		this.cardData = cardData;
		// If not ran out of cards, set the new data
		if (this.cardData != null) {
			this.characterName.text = cardData.characterName.ToUpper ();
			this.cardImage.sprite = Resources.Load <Sprite> (GetCardImagePath (cardData.characterName));
		}
	}

	private static string GetCardImagePath (string characterName) {
		switch (characterName) {
		case "ex":
		case "kid":
		case "lover":
		case "sex friends":
			characterName += (Random.Range (0, 2) == 1) ? " M" : " F";
			break;
		}
		return "Cards/" + characterName;
	}

	public CardData.Settings GetCardData () {
		return this.cardData;
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (this.state == State.Idle) {
			this.state = State.Dragging;
			this.fingerOffset = transform.position - Input.mousePosition;
		}
	}

	// Card is rotated in OnDrag, MoveBack ContinueSwipe and FinishSwipe
	void RotateCard () {
		Quaternion rotation = transform.rotation;
		float zAngle = GetDragAngle ();
		rotation.eulerAngles = new Vector3 (0f, 0f, zAngle);
		transform.rotation = rotation;
	}

	float GetDragAngle () {
		float angleMultiplier = 0.05f;
		float xDisplacement = startPosition.x - transform.position.x;
		return xDisplacement * angleMultiplier;
	}

	public void OnDrag (PointerEventData eventData) {
//		Vector3 position = Input.mousePosition + (Vector3.up * Screen.height * 0.15f);
		Vector3 position = Input.mousePosition + fingerOffset;
		transform.position = position;

		RotateCard ();

		if (IsAboveSwipeDistance ()) {
			DisplayAnswer ();
		}
		else {
			HideAnswer ();
		}
	}

	void DisplayAnswer () {
		answerPanel.SetActive (true);
		SwipeDirection swipeDirection = GetSwipeDirection ();
		CardData.Outcome cardOutcome;
		if (swipeDirection == SwipeDirection.Left) {
			answerText.text = cardData.leftOutcome.description;
			answerText.alignment = TextAnchor.UpperRight;
			cardOutcome = cardData.leftOutcome;
		}
		else {
			answerText.text = cardData.rightOutcome.description;
			answerText.alignment = TextAnchor.UpperLeft;
			cardOutcome = cardData.rightOutcome;
		}
		onAnswerDisplayed (cardOutcome.love, cardOutcome.fun, cardOutcome.health, cardOutcome.money);
	}

	void HideAnswer () {
		answerPanel.SetActive (false);
		onAnswerHidden ();
	}

	bool IsAboveSwipeDistance () {
//		float distance = Vector3.Distance (startPosition, transform.position);
		float distance = Mathf.Abs (startPosition.x - transform.position.x);
		return distance > this.minSwipeDistance;
	}

	SwipeDirection GetSwipeDirection () {
		bool isSwipingRight = transform.position.x > startPosition.x;
		return (isSwipingRight) ? SwipeDirection.Right : SwipeDirection.Left;
	}

	public void OnEndDrag (PointerEventData eventData) {
		this.state = (IsAboveSwipeDistance ()) ? State.Swiping : State.MovingBack;
		if (this.state == State.Swiping) {
			StartSwipe ();
		}
	}

	void MoveBack () {
		float step = this.movingSpeed * Time.deltaTime;
		this.transform.position = Vector2.MoveTowards (transform.position, startPosition, step);
		RotateCard ();
		if (this.transform.position == startPosition) {
			this.state = State.Idle;
		}
	}

	void StartSwipe () {
		this.swipeDirection = (transform.position.x > startPosition.x) ? SwipeDirection.Right : SwipeDirection.Left;
		this.targetPosition = this.startPosition;
		if (this.swipeDirection == SwipeDirection.Left) {
			this.targetPosition.x -= this.startPosition.x + Screen.width * 0.7f;
		}
		else {
			this.targetPosition.x += this.startPosition.x + Screen.width * 0.7f;
		}
	}

	void ContinueSwipe () {
		float step = this.swipingSpeed * Time.deltaTime;
		this.transform.position = Vector3.MoveTowards (this.transform.position, this.targetPosition, step);
		RotateCard ();
		if (Mathf.Approximately (transform.position.x, this.targetPosition.x)) {
			FinishSwipe ();
		}
	}

	void FinishSwipe () {
		this.state = State.Hidden;
		this.transform.position = startPosition;
		RotateCard ();
		this.transform.SetAsFirstSibling ();
		onCardSwiped (this);
		HideAnswer ();
	}

	void FixedUpdate () {
		switch (this.state) {
		case State.MovingBack:
			MoveBack ();
			break;
		case State.Swiping:
			ContinueSwipe ();
			break;
		}
	}
}
