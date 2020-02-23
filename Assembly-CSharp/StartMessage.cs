using System;
using UnityEngine;

[Serializable]
public class StartMessage
{
	public string Message;

	public float PercentageChance;

	public void ChooseMessage(float percentage, GameObject ReturnPoint)
	{
		if (PercentageChance >= percentage)
		{
			ReturnPoint.GetComponent<LoadScene>().StartLoad(Message);
		}
		else
		{
			ReturnPoint.GetComponent<LoadScene>().Load();
		}
	}
}
