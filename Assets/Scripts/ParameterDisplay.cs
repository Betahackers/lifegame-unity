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
	private float speed = 0.5f;

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
		if (fillImage != null && fillImage.fillAmount*100 != targetAmount) {
			fillImage.fillAmount  = Mathf.MoveTowards (fillImage.fillAmount, targetAmount/100f,(speed*Time.deltaTime));
		}
	}
}
