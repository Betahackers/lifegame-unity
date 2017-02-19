using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

	public GameObject answerPanel;
	public Text answerText;
	public RawImage cardImage;

	public float movingSpeed = 1500f;
	public float swipingSpeed = 2500f;
	public float minSwipeDistance = 10f;
	public float yLimit = 10f;

	private OnCardSwiped onCardSwiped;
	private State state;
	private Vector3 startPosition, dragPosition, targetPosition;
	private SwipeDirection swipeDirection;
	private Vector3 _fingerOffset;
	private CardData.Settings cardData;
	private OnAnswerDisplayed onAnswerDisplayed;
	private OnAnswerHidden onAnswerHidden;

	enum State {Hidden, Flipping, Idle, Dragging, MovingBack, Swiping};
	public enum SwipeDirection {Left, Right};
	public delegate void OnCardSwiped (CardMovement swipedCard);
	public delegate void OnAnswerDisplayed (int loveDelta, int funDelta, int healthDelta, int moneyDelta);
	public delegate void OnAnswerHidden ();

	void Start () {
		Debug.Log ("Start:" + startPosition);
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

	public SwipeDirection GetSwipeResult () {
		return this.swipeDirection;
	}

	public void SetCardData (CardData.Settings cardData) {
		this.cardData = cardData;
	}

	public CardData.Settings GetCardData () {
		return this.cardData;
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (this.state == State.Idle) {
			this.state = State.Dragging;
			this.dragPosition = Input.mousePosition;
			_fingerOffset = transform.position - Input.mousePosition;
		}
	}

	public void OnDrag (PointerEventData eventData) {
//		Vector3 position = Input.mousePosition + _fingerOffset;
		Vector3 position = Input.mousePosition + (Vector3.up * Screen.height * 0.15f);

		if (Mathf.Abs (position.y) > yLimit) {
			// TODO Implement y limit
//			position.y = Mathf.Sign (position.y) * yLimit;
		}

		float factor = 1;

		Vector3 lastPosition = this.transform.position;
		this.transform.position = position;

		/*
		 //This a misstry to rotate the image ;(
		 if (GetSwipeDirection () == SwipeDirection.Right) {
			Debug.Log ("SwipeDirection: Right");
			if (lastPosition.x < this.transform.position.x) {
				factor = 1;
			} else {
				factor = -1;
			}
		
		} else {
			Debug.Log ("SwipeDirection: Left");
			if (lastPosition.x > this.transform.position.x) {
				factor = 1;
			} else {
				factor = -1;
			}
		}
		Vector3 rotation = new Vector3();
		rotation.x = 0;
		rotation.y = 0;
		rotation.z = 1;//;factor * (Screen.width/2 - (this.transform.position.x));

		rotation.Normalize ();

		this.transform.Rotate (rotation);
		*/


		// TODO Change this later
//		Vector3 displacement = Input.mousePosition - this.dragPosition;
//		this.transform.position += displacement;
//		this.dragPosition = this.transform.position;

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
			answerText.text = cardData.leftOutcome.description.ToUpper ();
			answerText.alignment = TextAnchor.UpperRight;
			cardOutcome = cardData.leftOutcome;
		}
		else {
			answerText.text = cardData.rightOutcome.description.ToUpper ();
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
		float distance = Vector3.Distance (startPosition, transform.position);
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
		if (Mathf.Approximately (transform.position.x, this.targetPosition.x)) {
			FinishSwipe ();
		}
	}

	void FinishSwipe () {
		this.state = State.Hidden;
		this.transform.position = startPosition;
		this.transform.SetAsFirstSibling ();
		onCardSwiped (this);
		HideAnswer ();
	}

	void Flip () {
	}

	void FixedUpdate () {
		switch (this.state) {
		case State.Flipping:
			Flip ();
			break;
		case State.MovingBack:
			MoveBack ();
			break;
		case State.Swiping:
			ContinueSwipe ();
			break;
		}
	}
}
