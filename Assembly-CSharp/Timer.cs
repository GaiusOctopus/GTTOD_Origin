using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text TimeText;

	private float TimeToUpdate = 60f;

	private bool PM;

	private bool AddZero;

	private void Start()
	{
		CountClock();
		TimeToUpdate = 60 - DateTime.Now.Second;
	}

	private void Update()
	{
		if (TimeToUpdate <= 0f)
		{
			CountClock();
			TimeToUpdate = 60f;
		}
		else
		{
			TimeToUpdate -= Time.unscaledDeltaTime;
		}
	}

	private void CountClock()
	{
		int num = DateTime.Now.Hour;
		int minute = DateTime.Now.Minute;
		if (num > 12)
		{
			num -= 12;
			PM = true;
		}
		else
		{
			PM = false;
		}
		if (minute < 10)
		{
			AddZero = true;
		}
		else
		{
			AddZero = false;
		}
		if (!AddZero)
		{
			TimeText.text = num + ":" + minute;
		}
		else
		{
			TimeText.text = num + ":0" + minute;
		}
		if (!PM)
		{
			TimeText.text += " AM";
		}
		else
		{
			TimeText.text += " PM";
		}
	}
}
