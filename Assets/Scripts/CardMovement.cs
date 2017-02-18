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
	private string answer1, answer2;

	enum State {Hidden, Rotating, Idle, Dragging, MovingBack, Swiping};
	public enum SwipeDirection {Left, Right};
	public delegate void OnCardSwiped (CardMovement swipedCard);

	void Start () {
		this.startPosition = this.transform.position;
		this.targetPosition = this.startPosition;
		this.targetPosition.x = Screen.width * 0.7f;
	}

	public void Init (OnCardSwiped onCardSwiped, bool isFirstCard) {
		this.onCardSwiped = onCardSwiped;
		this.state = (isFirstCard) ? State.Idle : State.Hidden;
	}

	public SwipeDirection GetSwipeResult () {
		return this.swipeDirection;
	}

	public void SetAnswers (string answer1, string answer2) {
		this.answer1 = answer1;
		this.answer2 = answer2;
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if (this.state == State.Idle) {
			this.state = State.Dragging;
			this.dragPosition = Input.mousePosition;
		}
	}

	public void OnDrag (PointerEventData eventData) {
		Vector3 position = Input.mousePosition + (Vector3.up * Screen.height * 0.15f);
		if (Mathf.Abs (position.y) > yLimit) {
			// TODO Implement y limit
//			position.y = Mathf.Sign (position.y) * yLimit;
		}
		this.transform.position = position;

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
		if (swipeDirection == SwipeDirection.Left) {
			answerText.text = answer1.ToUpper ();
//			answerText.alignment = TextAnchor.UpperLeft;
		}
		else {
			answerText.text = answer2.ToUpper ();
//			answerText.alignment = TextAnchor.UpperRight;
		}
	}

	void HideAnswer () {
		answerPanel.SetActive (false);
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
			this.swipeDirection = (transform.position.x > startPosition.x) ? SwipeDirection.Right : SwipeDirection.Left;
		}
	}

	void MoveBack () {
		float step = this.movingSpeed * Time.deltaTime;
		this.transform.position = Vector2.MoveTowards (transform.position, startPosition, step);
		if (this.transform.position == startPosition) {
			this.state = State.Idle;
		}
	}

	void Swipe () {
		float step = this.swipingSpeed * Time.deltaTime;
		float sign = (this.swipeDirection == SwipeDirection.Right) ? 1 : -1;
		Vector3 target = targetPosition * sign;
		Debug.Log ("TARGET: " + target);
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, step);
		Debug.Log (transform.position);
		if (Mathf.Approximately (transform.position.x, target.x)) {
			Debug.Log ("Card swiped!!!");
			CardSwiped ();
		}
	}

	void CardSwiped () {
		this.state = State.Hidden;
		this.transform.position = startPosition;
		this.transform.SetAsFirstSibling ();
		onCardSwiped (this);
		HideAnswer ();
		// TODO Get new card data here
	}

	void Rotate () {
	}

	void FixedUpdate () {
		switch (this.state) {
		case State.Rotating:
			Rotate ();
			break;
		case State.MovingBack:
			MoveBack ();
			break;
		case State.Swiping:
			Swipe ();
			break;
		}
	}
}
