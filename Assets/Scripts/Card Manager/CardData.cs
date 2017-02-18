using UnityEngine;
using System.Collections;

public class CardData : MonoBehaviour {


	[System.Serializable]
	public class Settings
	{
		[Header("Card definition")]
		public string characterName;
		public string cardText;
		public Sprite cradImage;

		[Header("Swipe data")]
		public Outcome leftOutcome;
		public Outcome rightOutcome;

		[Header("Internal data")]
		public Chances chances;
	}

	[System.Serializable]
	public class Outcome
	{
		public string description;

		public int family;
		public int love;
		public int money;
		public int health;
	}

	[System.Serializable]
	public class Chances
	{
		public int probability;

		public int ageConstraint;
		public float ageChanceBase;
		public float ageChanceIncrement;
	}
}
