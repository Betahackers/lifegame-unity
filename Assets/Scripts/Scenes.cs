using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scenes {

	public static readonly int Splash = 0;
	public static readonly int Main = 1;

	public static void LoadScene (int sceneIndex) {
		SceneManager.LoadScene (sceneIndex);
	}
}
