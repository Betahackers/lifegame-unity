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
	private float speed = 1f;
	private float speedColor = 15f;

	private Color defaultColor = new Color(255f/256f, 72f/256f, 93f/256f);
	private Color upColor = new Color(80f/256f, 277f/256f, 194f/256f);
	private Color downColor = new Color(30f/256f, 59f/256f, 81f/256f);

	// Use this for initialization
	public void Init () {
		fillImage = transform.GetChild (0).GetComponent <Image> ();
	}

	public void ShowHide(bool show){
		selectedImage.gameObject.SetActive (show);
	}

	public void Fill(float amount){
		targetAmount = amount;
	}
	// Update is called once per frame
	void Update () {
		if (!Mathf.Approximately(fillImage.fillAmount * 100,targetAmount)) {
			if (targetAmount > fillImage.fillAmount * 100) {
				fillImage.color = upColor;
			} else {
				fillImage.color = downColor;
			}
			fillImage.fillAmount = Mathf.MoveTowards (fillImage.fillAmount, targetAmount / 100f, (speed * Time.deltaTime));
		} else {
			fillImage.color = Color.Lerp (fillImage.color, defaultColor,(speedColor * Time.deltaTime));
		}
	}
}
