using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ParameterType {Love, Fun, Health, Money};

public class ParameterDisplay : MonoBehaviour {

	public ParameterType parameterType;
	public Image selectedImage;

	private Image fillImage;
	private float targetAmount;
	private float startAmount;
	// Fill speed
	private float speed = 0.8f;
	// Color change speed
	private float speedColor = 5f;

	private Color downColor = new Color(255f/256f, 72f/256f, 93f/256f);
	private Color upColor = new Color(80f/256f, 277f/256f, 194f/256f);
	private Color defaultColor = new Color(30f/256f, 59f/256f, 81f/256f);
	private bool filling = false;

	// Use this for initialization
	public void Init (float initValue) {
		fillImage = transform.GetChild (0).GetComponent <Image> ();
		fillImage.color = defaultColor;
		fillImage.fillAmount = initValue / GameManager.MAX_LEVEL;
	}

	public void ShowHideSelector (bool show){
		selectedImage.gameObject.SetActive (show);
	}

	public void Fill (float amount) {
		filling = true;
		targetAmount = amount;
	}
	// Update is called once per frame
	void Update () {
		if (filling  && !Mathf.Approximately(fillImage.fillAmount * GameManager.MAX_LEVEL,targetAmount)) {
			if (targetAmount > fillImage.fillAmount * GameManager.MAX_LEVEL) {
				fillImage.color = upColor;
			} else {
				fillImage.color = downColor;
			}
			fillImage.fillAmount = Mathf.MoveTowards (fillImage.fillAmount, targetAmount / GameManager.MAX_LEVEL, (speed * Time.deltaTime));
		} else {
			filling = false;
			fillImage.color = Color.Lerp (fillImage.color, defaultColor,(speedColor * Time.deltaTime));
		}
	}
}
