using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CardDownloader : MonoBehaviour {

	public JSON_Deck gameDeck;
	public static CardDownloader instance;

	public delegate void OnCardsDownloaded ();
	private OnCardsDownloaded onCardsDownloaded;

	// Use this for initialization
	public void Init (OnCardsDownloaded onCardsDownloaded) {
		this.onCardsDownloaded = onCardsDownloaded;
		WWW www = new WWW("https://lifegame-api.herokuapp.com/game");
		StartCoroutine(WaitForRequest(www));

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else {
			Destroy (instance.gameObject);
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		// check for errors
		if (www.error == null)
		{
//			Debug.Log("WWW Ok!");
			string json_raw = www.text;
//			JSON_Deck new_deck = JsonUtility.FromJson<JSON_Deck>("{\"cards\":"+json_raw+"}");

//			gameDeck = JsonUtility.FromJson<JSON_Deck>("{\"cards\":"+json_raw+"}");
			gameDeck = JsonUtility.FromJson <JSON_Deck> (json_raw);

			CardManager.instance.Init ();
			this.onCardsDownloaded ();

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
