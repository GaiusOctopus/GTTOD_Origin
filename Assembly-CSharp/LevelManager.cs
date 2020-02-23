using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public int Level = 1;

	[Header("LEVEL SELECT OBJECTS")]
	public bool ShowObjects;

	[ConditionalField("ShowObjects", null)]
	public Text ActName;

	[ConditionalField("ShowObjects", null)]
	public Text LevelName;

	[ConditionalField("ShowObjects", null)]
	public Image LevelImage;

	[ConditionalField("ShowObjects", null)]
	public LoadTrigger Trigger;

	[ConditionalField("ShowObjects", null)]
	public Animation WhiteOut;

	[ConditionalField("ShowObjects", null)]
	public GameObject AcceptButton;

	[Header("LEVEL SCREENS")]
	public List<LevelScreens> LevelScreens;

	private ac_CharacterController CharacterController;

	private GTTODManager Manager;

	private AsyncOperation Loader;

	private bool Loading;

	private bool Loaded;

	private bool HasLoadedLevel;

	private int LevelToLoad;

	private void Start()
	{
		Level = 1;
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Manager = GameManager.GM.GetComponent<GTTODManager>();
		SetUpLevelSelect();
	}

	public void SetUpLevelSelect()
	{
		ActName.text = LevelScreens[Level].ActName;
		LevelName.text = LevelScreens[Level].LevelName;
		LevelImage.sprite = LevelScreens[Level].LevelImage;
		if (LevelScreens[Level].Available)
		{
			AcceptButton.SetActive(value: true);
		}
		else
		{
			AcceptButton.SetActive(value: false);
		}
	}

	public void ChangeLevel(bool Right)
	{
		if (Right)
		{
			Level++;
			if (Level == 9)
			{
				Level = 0;
				SetUpLevelSelect();
			}
			else
			{
				SetUpLevelSelect();
			}
		}
		else
		{
			Level--;
			if (Level == -1)
			{
				Level = 8;
				SetUpLevelSelect();
			}
			else
			{
				SetUpLevelSelect();
			}
		}
	}

	public void NextLevel()
	{
		Level = 1;
		Trigger.StartedLoad = false;
		if (Level == 9)
		{
			Level = 0;
			SetUpLevelSelect();
		}
		else
		{
			SetUpLevelSelect();
		}
	}

	public void StartLoadScene()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		LevelToLoad = Level + 3;
		StartCoroutine(LoadSceneAsync());
		Loaded = false;
		Loading = true;
	}

	public void StartNewScene()
	{
		StartCoroutine(StartSceneAsync());
	}

	private IEnumerator LoadSceneAsync()
	{
		yield return new WaitForSeconds(1f);
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		Loader = SceneManager.LoadSceneAsync(LevelToLoad);
		Loader.allowSceneActivation = false;
		while (!Loader.isDone)
		{
			yield return null;
			if (Loader.progress >= 0.9f && !Loaded)
			{
				Loaded = true;
				CharacterController.GetComponent<InventoryScript>().PrintTutorialMessage("Level Loaded. Hold R to teleport to surface portal");
				CharacterController.GetComponent<InventoryScript>().PortalActiveToggle(Active: true);
			}
		}
	}

	private IEnumerator StartSceneAsync()
	{
		WhiteOut.Play("WhiteOut");
		yield return new WaitForSeconds(2f);
		CharacterController.GetComponent<InventoryScript>().PortalActiveToggle(Active: false);
		if (Manager.GameMode == 1)
		{
			Loader.allowSceneActivation = true;
		}
		if (Manager.GameMode == 2)
		{
			Manager.StartRogueLevel();
			LevelHasLoaded();
		}
		if (Manager.GameMode == 3)
		{
			Manager.StartCustomLevel();
			LevelHasLoaded();
		}
	}

	public void LevelHasLoaded()
	{
		WhiteOut.Play("WhiteIn");
		GetComponent<GTTODManager>().SwitchWorld(Valley: true);
		CharacterController.ResetCharacter();
		Loading = false;
		if (Manager.GameMode == 1)
		{
			Manager.ChangeAmbience();
		}
		if (Manager.GameMode == 2)
		{
			if (HasLoadedLevel)
			{
				GetComponent<AIManager>().StartSpawningAtLevel(3);
				return;
			}
			GetComponent<AIManager>().StartSpawning();
			HasLoadedLevel = true;
		}
	}
}
