using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class JsonExample : MonoBehaviour {

	// Use this for initialization
	void Start () {
		WWW www = new WWW("https://lifegame-api.herokuapp.com/cards");
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!");
			string json_raw = www.text;
			JSON_Deck new_deck = JsonUtility.FromJson<JSON_Deck>("{\"cards\":"+json_raw+"}");
		
			foreach(JSON_Card new_card in new_deck.cards) {
				print(new_card);
			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
