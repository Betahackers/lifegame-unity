using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {

	public Color upColorText, downColorText;
	public Color upColorIcon, downColorIcon;
	public ScrollRect scrollRect;
	public RectTransform gameOverPanel;
	public Text gameOverText, playAgainText;
	public Image parameterIcon;

	private float scrollSpeed = 0.35f;
	private float targetPosition = 0f;

	// Use this for initialization
	void Start () {
		scrollRect.verticalNormalizedPosition = 0.5f;

		string gameOverString = "After a 150-second orgasm, your heart gives up and you die on top of your various lovers.";
		string parameterName = "Love";
		parameterIcon.sprite = GetParemeterSprite (parameterName);
		parameterIcon.color = (targetPosition == 1f) ? upColorIcon : downColorIcon;

		gameOverText.text = gameOverString;
		gameOverText.color = (targetPosition == 1f) ? upColorText : downColorText;
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
