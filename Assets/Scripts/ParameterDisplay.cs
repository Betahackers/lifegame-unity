using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ParameterType {Love, Fun, Health, Money};

public class ParameterDisplay : MonoBehaviour {

	public ParameterType parameterType;

	private Image fillImage;
	private Image selectedImage;

	// Use this for initialization
	public void Init () {
		fillImage = transform.GetChild (0).GetComponent <Image> ();
		selectedImage = transform.GetChild (1).GetComponent <Image> ();
	}

	public void ShowHide(bool show){
		Color c =	selectedImage.color;
		if (show) {
			c.a = 1f;
		} else {
			c.a = 0f;
		}

		selectedImage.color = c; 
	}

	public void Fill(float amount){
		selectedImage.fillAmount =selectedImage.fillAmount + amount;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
