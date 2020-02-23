using System.Collections.Generic;

namespace CommandTerminal
{
	public class CommandAutocomplete
	{
		private List<string> known_words = new List<string>();

		private List<string> buffer = new List<string>();

		public void Register(string word)
		{
			known_words.Add(word.ToLower());
		}

		public string[] Complete(ref string text)
		{
			string value = EatLastWord(ref text).ToLower();
			buffer.Clear();
			for (int i = 0; i < known_words.Count; i++)
			{
				string text2 = known_words[i];
				if (text2.StartsWith(value))
				{
					buffer.Add(text2);
				}
			}
			return buffer.ToArray();
		}

		private string EatLastWord(ref string text)
		{
			int num = text.LastIndexOf(' ');
			string result = text.Substring(num + 1);
			text = text.Substring(0, num + 1);
			return result;
		}
	}
}
