using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

	public Color upColorText, downColorText;
	public Color upColorIcon, downColorIcon;
	public ScrollRect scrollRect;
	public RectTransform gameOverPanel;
	public Text causeText, effectText, playAgainText, ageText;
	public Image parameterIcon;

	private float scrollSpeed = 0.35f;
	private float targetPosition = 0f;
	private static GameManager.GameOverReason gameOverReason;
	private static int deathAge;

	// Use this for initialization
	void Start () {
		scrollRect.verticalNormalizedPosition = 0.5f;
		SetupGameOver ();
	}

	public static void SetupGameOver (GameManager.GameOverReason _gameOverReason, int _deathAge) {
		gameOverReason = _gameOverReason;
		deathAge = _deathAge;
	}

	float GetTargetPosition () {
		switch (gameOverReason) {
		case GameManager.GameOverReason.NoMoney:
		case GameManager.GameOverReason.NoFun:
		case GameManager.GameOverReason.NoHealth:
		case GameManager.GameOverReason.NoLove:
			return 0f;
		default:
			return 1f;
		}
	}

	string GetParameterName () {
		switch (gameOverReason) {
		default:
			return "";
		case GameManager.GameOverReason.FullFun:
		case GameManager.GameOverReason.NoFun:
			return "Fun";
		case GameManager.GameOverReason.FullHealth:
		case GameManager.GameOverReason.NoHealth:
			return "Health";
		case GameManager.GameOverReason.FullLove:
		case GameManager.GameOverReason.NoLove:
			return "Love";
		case GameManager.GameOverReason.FullMoney:
		case GameManager.GameOverReason.NoMoney:
			return "Money";
		}
	}

	private void SetupGameOver () {
		ageText.text = deathAge.ToString ();
		targetPosition = GetTargetPosition ();

		if (gameOverReason == GameManager.GameOverReason.Aged || gameOverReason == GameManager.GameOverReason.OutOfCards) {
			parameterIcon.gameObject.SetActive (false);
		}
		else {
			string parameterName = GetParameterName ();
			parameterIcon.sprite = GetParemeterSprite (parameterName);
			parameterIcon.color = (targetPosition == 1f) ? upColorIcon : downColorIcon;
		}

		JSON_Ending jsonEnding = CardDownloader.instance.gameDeck.GetEnding (gameOverReason);
		causeText.text = jsonEnding.cause;
		effectText.text = jsonEnding.effect;
		causeText.color = (targetPosition == 1f) ? upColorText : downColorText;
		effectText.color = (targetPosition == 1f) ? upColorText : downColorText;
		playAgainText.color = (targetPosition == 1f) ? upColorText : downColorText;

		gameOverPanel.anchorMax = new Vector2 (0.5f, targetPosition);
		gameOverPanel.anchorMin = new Vector2 (0.5f, targetPosition);
		gameOverPanel.pivot = new Vector2 (0.5f, targetPosition);
	}

	static Sprite GetParemeterSprite (string parameterName) {
		return Resources.Load <Sprite> ("Parameter Icons/" + parameterName);
	}

	public void PlayAgain () {
		Scenes.LoadScene (Scenes.Splash);
	}
	
	// Update is called once per frame
	void Update () {
		scrollRect.verticalNormalizedPosition = Mathf.MoveTowards (scrollRect.verticalNormalizedPosition, targetPosition, scrollSpeed * Time.deltaTime);
//		if (Mathf.Approximately (scrollRect.verticalNormalizedPosition, targetPosition)) {
//			gameOverPanel.SetActive (true);
//		}
	}
}
