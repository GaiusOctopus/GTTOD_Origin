using System;

[Serializable]
public class TyperMessage
{
	public string Text;

	public void FixText()
	{
		Text += " ";
	}
}
