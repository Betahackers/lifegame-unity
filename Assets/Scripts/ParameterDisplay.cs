using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ParameterType {Love, Fun, Health, Money};

public class ParameterDisplay : MonoBehaviour {

	public ParameterType parameterType;

	private Image fillImage;

	// Use this for initialization
	void Start () {
		fillImage = transform.GetChild (0).GetComponent <Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
