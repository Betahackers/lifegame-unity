using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class JsonExample : MonoBehaviour {


	public JSON_Deck gameDeck;
	public GameManager manager;


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
//			JSON_Deck new_deck = JsonUtility.FromJson<JSON_Deck>("{\"cards\":"+json_raw+"}");
			gameDeck = JsonUtility.FromJson<JSON_Deck>("{\"cards\":"+json_raw+"}");
		
			foreach(JSON_Card new_card in gameDeck.cards) {
				print(new_card);
			}

		} else {
			Debug.Log("WWW Error: "+ www.error);
		} 

		CardManager.instance.GetCardData ();
		manager.Init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
