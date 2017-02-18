using System;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class JSON_Answer
	{
		public int id;
		public string text;
		public string type;
		public JSON_Point[] points;

		public JSON_Answer ()
		{
		}
	}
}

