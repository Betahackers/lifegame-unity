using UnityEngine;
using System.Collections;

public class SplashAnimation : MonoBehaviour {

	public delegate void OnSplashAnimationEnded ();

	OnSplashAnimationEnded onSplashAnimationEnded;

	public void Init (OnSplashAnimationEnded onSplashAnimationEnded) {
		this.onSplashAnimationEnded = onSplashAnimationEnded;
	}

	public void OnAnimationEnded () {
		this.onSplashAnimationEnded ();
	}
}
