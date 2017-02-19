using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {

	public SplashAnimation splashAnimation;
	public CardDownloader cardDownloader;
	public GameObject audioManager;

	private bool animationEnded, cardsLoaded;

	// Use this for initialization
	void Start () {
		animationEnded = cardsLoaded = false;
		splashAnimation.Init (OnAnimationEnded);
		cardDownloader.Init (OnCardsDownloaded);
		DontDestroyOnLoad (audioManager);
	}

	void OnAnimationEnded () {
		animationEnded = true;
		LoadGameIfFinished ();
	}

	void OnCardsDownloaded () {
		cardsLoaded = true;
		LoadGameIfFinished ();
	}

	void LoadGameIfFinished () {
		if (animationEnded && cardsLoaded) {
			Scenes.LoadScene (Scenes.Main);
		}
	}
}
