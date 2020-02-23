using System;
using UnityEngine;

[Serializable]
public class RandomStat
{
	public string StatName = "StatName";

	public int Stat;

	private GameManager GM;

	public void SetStat()
	{
		Stat = PlayerPrefs.GetInt(StatName, 0);
		GM = GameManager.GM;
	}

	public void SaveStat()
	{
		PlayerPrefs.SetInt(StatName, Stat);
	}

	public void IncreaseStat()
	{
		Stat++;
		SaveStat();
		PushStat();
	}

	public void PushStat()
	{
		if (GM.StatIndex == GM.RandomStats.IndexOf(this))
		{
			GM.SetRandomStat(StatName, Stat);
		}
	}
}
