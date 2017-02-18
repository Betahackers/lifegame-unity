using System;

namespace AssemblyCSharp
{
	[System.Serializable]
	public class JSON_Card
	{
		int id;
		string title;
		string person;
		string url_image;
		JSON_Answer[] answers;

		public JSON_Card ()
		{
			
		}
	}
}

