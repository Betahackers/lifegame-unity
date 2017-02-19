using System;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class JSON_Deck
	{
		public JSON_Card[] cards;
		public JSON_Ending[] endings;

		public JSON_Deck ()
		{
		}

		public JSON_Ending GetEnding (GameManager.GameOverReason gameOverReason) {
			string endingReason = GetEndingReason (gameOverReason);
			for (int i = 0; i < endings.Length; i++) {
				if (endings [i].reason.Equals (endingReason)) {
					return endings [i];
				}
			}
			return endings [0];
		}

		private static string GetEndingReason (GameManager.GameOverReason gameOverReason) {
			switch (gameOverReason) {
			default:
				return "age_max";
			case GameManager.GameOverReason.FullFun:
				return "fun_max";
			case GameManager.GameOverReason.NoFun:
				return "fun_min";
			case GameManager.GameOverReason.FullHealth:
				return "health_max";
			case GameManager.GameOverReason.NoHealth:
				return "health_min";
			case GameManager.GameOverReason.FullLove:
				return "love_max";
			case GameManager.GameOverReason.NoLove:
				return "love_min";
			case GameManager.GameOverReason.FullMoney:
				return "money_max";
			case GameManager.GameOverReason.NoMoney:
				return "money_min";
			}
		}
	}
}