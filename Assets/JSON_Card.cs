using System;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class JSON_Card
	{
		public int id;
		public string title;
		public string person;
		public string url_image;
		public JSON_Answer[] answers;

		public JSON_Card ()
		{
			
		}
	}
}

