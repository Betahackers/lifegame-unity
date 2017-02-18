using System;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class JSON_Answer
	{
		int id;
		string text;
		string type;
		JSON_Point[] points;

		public JSON_Answer ()
		{
		}
	}
}

