using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager GM;

	[Header("OBJECTS")]
	public bool ShowObjects;

	[ConditionalField("ShowObjects", null)]
	public GameObject Player;

	[ConditionalField("ShowObjects", null)]
	public GameObject GodModePlayer;

	[ConditionalField("ShowObjects", null)]
	public Transform CustomMapParent;

	[ConditionalField("ShowObjects", null)]
	public Text PlayerKillsText;

	[ConditionalField("ShowObjects", null)]
	public Text DoorsOpenedText;

	[ConditionalField("ShowObjects", null)]
	public Text GamesWonText;

	[ConditionalField("ShowObjects", null)]
	public Text PlayerDeathsText;

	[Header("STATS")]
	public bool ShowStats;

	[ConditionalField("ShowStats", null)]
	public Text RandomStatText;

	[ConditionalField("ShowStats", null)]
	public Text RandomStatValueText;

	[ConditionalField("ShowStats", null)]
	public int PlayerKillsStat;

	[ConditionalField("ShowStats", null)]
	public int DoorsOpenedStat;

	[ConditionalField("ShowStats", null)]
	public int GamesWonStat;

	[ConditionalField("ShowStats", null)]
	public int PlayerDeathsStat;

	[Header("LISTS")]
	public List<RandomStat> RandomStats;

	public List<KeyBinding> KeyBinds;

	[HideInInspector]
	[Header("Custom Starter Cards")]
	public bool AntiLife;

	[HideInInspector]
	public bool Speedrunner;

	[HideInInspector]
	public bool Ninja;

	[HideInInspector]
	public List<TimeStopObject> Objects;

	[HideInInspector]
	public bool TimeStopped;

	[HideInInspector]
	public int StatIndex;

	[HideInInspector]
	public float EnemyDamageModifier = 1f;

	private GTTODManager MyManager;

	private GameObject CurrentGodMode;

	private Transform CustomMap;

	private bool NoClipping;

	public void Start()
	{
		MyManager = GetComponent<GTTODManager>();
		PlayerKillsStat = PlayerPrefs.GetInt("PlayerKills", 0);
		DoorsOpenedStat = PlayerPrefs.GetInt("DoorsOpened", 0);
		GamesWonStat = PlayerPrefs.GetInt("GamesWon", 0);
		PlayerDeathsStat = PlayerPrefs.GetInt("DoorsOpened", 0);
		foreach (RandomStat randomStat in RandomStats)
		{
			randomStat.SetStat();
		}
		foreach (KeyBinding keyBind in KeyBinds)
		{
			keyBind.LoadKeybinds();
		}
		StartCoroutine(StartUpWait());
	}

	public IEnumerator StartUpWait()
	{
		yield return new WaitForSeconds(1f);
		UpdateStats();
	}

	private void Awake()
	{
		if (GM == null)
		{
			GM = this;
		}
		else if (GM != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void StartCustomMap()
	{
		CustomMap = GameObject.Find("Level Editor SceneGraph Root").transform;
		CustomMap.parent = CustomMapParent;
		base.gameObject.GetComponent<GTTODManager>().StartGame(3);
	}

	public void NoClip()
	{
		if (!NoClipping)
		{
			NoClipping = true;
			Player.GetComponent<ac_CharacterController>().CutsceneStart();
			CurrentGodMode = Object.Instantiate(GodModePlayer, Player.transform.position, Player.transform.rotation);
		}
		else
		{
			NoClipping = false;
			Player.GetComponent<ac_CharacterController>().CutsceneEnd(CurrentGodMode.transform);
			Object.Destroy(CurrentGodMode);
		}
	}

	public void UpdateStats()
	{
		PlayerKillsText.text = PlayerKillsStat.ToString();
		GamesWonText.text = GamesWonStat.ToString();
		DoorsOpenedText.text = DoorsOpenedStat.ToString();
		PlayerDeathsText.text = PlayerDeathsStat.ToString();
		StatIndex = Random.Range(0, RandomStats.Count);
		RandomStats[StatIndex].PushStat();
		SaveStats();
	}

	public void SaveStats()
	{
		PlayerPrefs.SetInt("PlayerKills", PlayerKillsStat);
		PlayerPrefs.SetInt("DoorsOpened", DoorsOpenedStat);
		PlayerPrefs.SetInt("GamesWon", GamesWonStat);
		PlayerPrefs.SetInt("PlayerDeaths", PlayerDeathsStat);
		if (PlayerKillsStat >= 1)
		{
			MyManager.GainAchievement("Ach_LowStandards");
		}
		if (PlayerKillsStat >= 100)
		{
			MyManager.GainAchievement("Ach_MediumStandards");
		}
		if (PlayerKillsStat >= 1000)
		{
			MyManager.GainAchievement("Ach_HighStandards");
		}
		if (PlayerKillsStat >= 5000)
		{
			MyManager.GainAchievement("Ach_UltraHighStandards");
		}
	}

	public void SetRandomStat(string StatName, int Value)
	{
		RandomStatText.text = StatName;
		RandomStatValueText.text = Value.ToString();
	}

	public void FreezeGame()
	{
		TimeStopped = true;
		Player.GetComponent<ac_CharacterController>().WeaponCamera.gameObject.GetComponent<Animation>().Play("TimeStop");
		foreach (TimeStopObject @object in Objects)
		{
			@object.Freeze();
		}
	}

	public void UnfreezeGame()
	{
		TimeStopped = false;
		StartCoroutine(EffectOff());
		Player.GetComponent<ac_CharacterController>().WeaponCamera.gameObject.GetComponent<Animation>().Play("TimeStart");
		foreach (TimeStopObject @object in Objects)
		{
			@object.UnFreeze();
		}
	}

	private IEnumerator EffectOff()
	{
		yield return new WaitForSeconds(0.5f);
	}
}
