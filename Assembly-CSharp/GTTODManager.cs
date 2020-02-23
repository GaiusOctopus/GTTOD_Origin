using GILES.Example;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GTTODManager : MonoBehaviour
{
	[Header("MANAGEMENT")]
	public bool ShowManagement;

	[ConditionalField("ShowManagement", null)]
	public bool FullGame = true;

	[ConditionalField("ShowManagement", null)]
	public ac_CharacterController Player;

	[ConditionalField("ShowManagement", null)]
	public TimeSlow Slowmo;

	[ConditionalField("ShowManagement", null)]
	public AudioClip DropIntro;

	[ConditionalField("ShowManagement", null)]
	[Tooltip("0 == Scenario /// 1 == Overhaul /// 2 == Rogue /// 3 == Custom /// 4 == Endless")]
	public int GameMode;

	[Header("MENUS")]
	public bool ShowMenus;

	[ConditionalField("ShowMenus", null)]
	public Animator MenuAnimator;

	[ConditionalField("ShowMenus", null)]
	public GameObject Background;

	[ConditionalField("ShowMenus", null)]
	public GameObject MainButtons;

	[ConditionalField("ShowMenus", null)]
	public GameObject ModeButtons;

	[ConditionalField("ShowMenus", null)]
	public GameObject OverhaulButtons;

	[ConditionalField("ShowMenus", null)]
	public GameObject CustomButtons;

	[ConditionalField("ShowMenus", null)]
	public GameObject CardButtons;

	[ConditionalField("ShowMenus", null)]
	public GameObject Timeline;

	[ConditionalField("ShowMenus", null)]
	public GameObject ExtraBack;

	[ConditionalField("ShowMenus", null)]
	public GameObject Exit;

	[Header("OST MANAGER")]
	public bool ShowOST;

	[ConditionalField("ShowOST", null)]
	public Animator ArtistCard;

	[ConditionalField("ShowOST", null)]
	public Text SongName;

	[Header("POINTS MANAGER")]
	public bool ShowPoints;

	[ConditionalField("ShowPoints", null)]
	public int Points;

	[ConditionalField("ShowPoints", null)]
	public int PointsMultiplyer;

	[ConditionalField("ShowPoints", null)]
	public int MaxMultiplier;

	[ConditionalField("ShowPoints", null)]
	public PointsTransfer ScoreCanvas;

	[Header("OBJECTS")]
	public bool ShowObjects;

	[ConditionalField("ShowObjects", null)]
	public GameObject MenuObject;

	[ConditionalField("ShowObjects", null)]
	public Door Enterance;

	[ConditionalField("ShowObjects", null)]
	public AbyssTrigger AbyssTrigger;

	[ConditionalField("ShowObjects", null)]
	public GameObject FadeSteps;

	[ConditionalField("ShowObjects", null)]
	public GameObject LevelComplete;

	[ConditionalField("ShowObjects", null)]
	public GameObject StarterCardsContent;

	[ConditionalField("ShowObjects", null)]
	public GameObject Island;

	[ConditionalField("ShowObjects", null)]
	public GameObject TempGrabAGun;

	[ConditionalField("ShowObjects", null)]
	public StartingIsland SurfacePortal;

	[ConditionalField("ShowObjects", null)]
	public GrabAGun GrabAGun;

	[Header("MODE OBJECTS")]
	public bool ShowModeObjects;

	[ConditionalField("ShowModeObjects", null)]
	public GameObject Surface;

	[ConditionalField("ShowModeObjects", null)]
	public GameObject Abyss;

	[ConditionalField("ShowModeObjects", null)]
	public GameObject Rogue;

	[ConditionalField("ShowModeObjects", null)]
	public GameObject QuickPlay;

	[Header("LISTS")]
	public StarterCard[] StarterCards;

	public List<GTTODTrack> Tracks;

	public List<GTTODAmbientTrack> AmbientTracks;

	public List<GameObject> RogueSegments;

	[HideInInspector]
	[Header("Priavate Variables")]
	public ObjectResetter[] CustomResets;

	[HideInInspector]
	public LevelSegment[] LevelSegments;

	[HideInInspector]
	public bool PlayingOverhaul = true;

	private GameManager GM;

	private AIManager AIManager;

	private AsyncOperation Loader;

	private AudioSource Audio;

	private AudioSource IntroAudio;

	private MenuScript Menu;

	private NavMeshSurface NavMesh;

	private InventoryScript Inventory;

	private HealthScript PlayerHealth;

	private ScenarioManager scenarioManager;

	private Transform CustomMap;

	private GameObject CurrentLevel;

	public int CardWeapon;

	private bool inMenu;

	private bool canTransition = true;

	private bool HasGivenInfo;

	private void Start()
	{
		CheckPointsUI();
		GM = GameManager.GM;
		Audio = GetComponent<AudioSource>();
		IntroAudio = MenuObject.GetComponent<AudioSource>();
		Menu = GetComponent<MenuScript>();
		NavMesh = GetComponent<NavMeshSurface>();
		Inventory = Player.gameObject.GetComponent<InventoryScript>();
		PlayerHealth = Player.gameObject.GetComponent<HealthScript>();
		AIManager = GetComponent<AIManager>();
		scenarioManager = QuickPlay.GetComponent<ScenarioManager>();
		Time.timeScale = 1f;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		StarterCards = StarterCardsContent.GetComponentsInChildren<StarterCard>();
	}

	private void Update()
	{
		if (!inMenu && canTransition && Input.anyKeyDown && !Player.gameObject.activeInHierarchy)
		{
			canTransition = false;
			if (!PlayerPrefsPlus.GetBool("FirstTimeBoot", defaultValue: true))
			{
				inMenu = true;
				MenuAnimator.SetTrigger("Position");
				IntroAudio.Play();
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				Menu.CanPauseGame = true;
			}
			else
			{
				StartCoroutine(DemoStart());
				IntroAudio.PlayOneShot(DropIntro);
				MenuAnimator.SetTrigger("DropIntro");
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		IntroAudio.volume = Menu.MusicVolume / 100f;
	}

	private IEnumerator DemoStart()
	{
		yield return new WaitForSeconds(6f);
		MenuObject.SetActive(value: false);
		GameMode = 1;
		MenuObject.SetActive(value: false);
		Player.gameObject.SetActive(value: true);
		Island.SetActive(value: false);
		AbyssTrigger.AbyssStart();
	}

	public void SetGameMode(int GameModeToSet)
	{
		GameMode = GameModeToSet;
	}

	public void StartGame(int GameModeChange)
	{
		GameMode = GameModeChange;
		StartCoroutine(StartSceneFade(GameModeChange));
	}

	public IEnumerator StartSceneFade(int ModeInt)
	{
		MenuAnimator.SetTrigger("Fade");
		Object.Instantiate(FadeSteps);
		CloseAllMenus();
		if (GameMode == 0)
		{
			Rogue.SetActive(value: true);
			foreach (GameObject rogueSegment in RogueSegments)
			{
				rogueSegment.SetActive(value: false);
			}
			StartCoroutine(QuickBuildRogueLevel());
		}
		yield return new WaitForSeconds(2f);
		GameMode = ModeInt;
		if (GameMode == 0)
		{
			yield return new WaitForSeconds(1f);
			MenuObject.SetActive(value: false);
			Player.gameObject.SetActive(value: true);
			Player.transform.position = new Vector3(0f, 0.35f, 10f);
			StartRogueLevel();
			yield return new WaitForSeconds(2f);
			GetComponent<AIManager>().StartSpawning();
			TempGrabAGun.SetActive(value: true);
			GrabAGun.ToggleProgressionType(IsSaved: true);
			StartCoroutine(ForceInformation("Practice against waves of enemies, buy equipment in the level, and buy guns on the starting platform"));
		}
		if (GameMode == 1)
		{
			MenuObject.SetActive(value: false);
			Player.gameObject.SetActive(value: true);
			Island.SetActive(value: false);
			AbyssTrigger.AbyssStart();
			TempGrabAGun.SetActive(value: false);
			StartCoroutine(ForceInformation("Loading Level --- Reach the back of the ravine to load the next level"));
		}
		if (GameMode == 2)
		{
			MenuObject.SetActive(value: false);
			Player.gameObject.SetActive(value: true);
			Island.SetActive(value: false);
			AbyssTrigger.AbyssStart();
			Rogue.SetActive(value: true);
			GetComponent<AIManager>().StopSpawning();
			BuildRogueLevel();
			TempGrabAGun.SetActive(value: false);
			StartCoroutine(ForceInformation("Level Generating --- Reach the back of the ravine to load the next level"));
			StartCoroutine(TimedLoad());
		}
		if (GameMode == 3)
		{
			MenuObject.SetActive(value: false);
			Player.gameObject.SetActive(value: true);
			Island.SetActive(value: false);
			AbyssTrigger.AbyssStart();
			BuildCustomLevel();
			TempGrabAGun.SetActive(value: false);
			StartCoroutine(ForceInformation("Generating Custom Level --- Reach the back of the ravine to load the next level"));
			StartCoroutine(TimedLoad());
		}
		if (GameMode == 4)
		{
			MenuObject.SetActive(value: false);
			Player.gameObject.SetActive(value: true);
			Island.SetActive(value: false);
			AbyssTrigger.AbyssStart();
			GrabAGun.ToggleProgressionType(IsSaved: true);
			GrabAGun.SetUpProgression();
			StartCoroutine(ForceInformation("Buy weapons and upgrade your Grab A Gun in the back of the ravine. Hold R to teleport to arena"));
			yield return new WaitForSeconds(5f);
			GetComponent<AIManager>().ToggleAbyssSpawning();
			Player.GetComponent<InventoryScript>().PortalActiveToggle(Active: true);
			ChangeSongs();
		}
	}

	public void ModeSelect()
	{
		MainButtons.SetActive(value: false);
		ModeButtons.SetActive(value: true);
		ExtraBack.SetActive(value: true);
		Exit.SetActive(value: false);
	}

	public void OverhaulSelect()
	{
		Background.SetActive(value: false);
		MainButtons.SetActive(value: false);
		ModeButtons.SetActive(value: false);
		OverhaulButtons.SetActive(value: true);
		ExtraBack.SetActive(value: false);
		Exit.SetActive(value: false);
	}

	public void ChooseCharacterType(bool Slayer)
	{
	}

	public void CustomButtonsSelect()
	{
		CustomButtons.SetActive(value: true);
		ModeButtons.SetActive(value: false);
		Exit.SetActive(value: false);
		ExtraBack.SetActive(value: true);
	}

	public void StarterCardsSelect()
	{
		CardButtons.SetActive(value: true);
		Background.SetActive(value: false);
		MainButtons.SetActive(value: false);
		Exit.SetActive(value: false);
		ExtraBack.SetActive(value: false);
		Inventory.SetAllWeaponIDs();
	}

	public void StartCardMode(int WeaponID, int EquipmentID, GameObject CustomSettings)
	{
		Inventory.GiveOnAwake = true;
		Inventory.WeaponToGive = WeaponID;
		Inventory.EquipmentToGive = EquipmentID;
		Object.Instantiate(CustomSettings);
		if (GameMode == 3)
		{
			GameManager.GM.GetComponent<LoadAndPlayScene>().OpenLoadPanel();
			CloseAllMenus();
		}
		else
		{
			StartGame(GameMode);
		}
	}

	public void LoadScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void CustomButtonsBack()
	{
		Background.SetActive(value: true);
		MainButtons.SetActive(value: false);
		ModeButtons.SetActive(value: true);
		Timeline.SetActive(value: false);
		CustomButtons.SetActive(value: false);
		Exit.SetActive(value: false);
		ExtraBack.SetActive(value: true);
	}

	public void TimelineSelect()
	{
		Timeline.SetActive(value: true);
		Exit.SetActive(value: false);
	}

	public void Back()
	{
		Background.SetActive(value: true);
		MainButtons.SetActive(value: true);
		Exit.SetActive(value: true);
		ExtraBack.SetActive(value: false);
		ModeButtons.SetActive(value: false);
		Timeline.SetActive(value: false);
		CustomButtons.SetActive(value: false);
		CardButtons.SetActive(value: false);
		OverhaulButtons.SetActive(value: false);
	}

	public void CloseAllMenus()
	{
		Background.SetActive(value: false);
		MainButtons.SetActive(value: false);
		Exit.SetActive(value: false);
		ExtraBack.SetActive(value: false);
		ModeButtons.SetActive(value: false);
		Timeline.SetActive(value: false);
		CustomButtons.SetActive(value: false);
		CardButtons.SetActive(value: false);
		OverhaulButtons.SetActive(value: false);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void ResetLevel()
	{
		CustomResets = Object.FindObjectsOfType<ObjectResetter>();
		if (CustomResets.Length != 0)
		{
			ObjectResetter[] customResets = CustomResets;
			for (int i = 0; i < customResets.Length; i++)
			{
				customResets[i].ResetMe();
			}
		}
	}

	public IEnumerator QuickBuildRogueLevel()
	{
		Rogue.transform.position = new Vector3(0f, 1500f, 0f);
		Rogue.GetComponent<NavMeshSurface>().RemoveData();
		yield return new WaitForSeconds(1f);
		LevelSegment[] array = Object.FindObjectsOfType<LevelSegment>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetSegment();
		}
		yield return new WaitForSeconds(1f);
		Rogue.GetComponent<NavMeshSurface>().BuildNavMesh();
	}

	public void BuildRogueLevel()
	{
		StartCoroutine(BuildRogueAsync());
	}

	public IEnumerator BuildRogueAsync()
	{
		Rogue.transform.position = new Vector3(0f, 1500f, 0f);
		Rogue.GetComponent<NavMeshSurface>().RemoveData();
		yield return new WaitForSeconds(3f);
		LevelSegment[] array = Object.FindObjectsOfType<LevelSegment>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetSegment();
		}
		yield return new WaitForSeconds(3f);
		Rogue.GetComponent<NavMeshSurface>().BuildNavMeshAsync();
	}

	public void StartRogueLevel()
	{
		Rogue.transform.position = Vector3.zero;
		Rogue.gameObject.GetComponent<AudioRange>().PlayAudio();
		LevelSegment[] array = Object.FindObjectsOfType<LevelSegment>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].HideSegment();
		}
		ChangeSongs();
	}

	public void BuildCustomLevel()
	{
		StartCoroutine(BuildCustomLevelAsync());
	}

	public IEnumerator BuildCustomLevelAsync()
	{
		CustomMap = GM.CustomMapParent;
		CustomMap.gameObject.SetActive(value: true);
		CustomMap.position = new Vector3(0f, 1500f, 0f);
		CustomMap.GetComponent<NavMeshSurface>().RemoveData();
		yield return new WaitForSeconds(3f);
		CustomMap.GetComponent<NavMeshSurface>().BuildNavMeshAsync();
	}

	public void StartCustomLevel()
	{
		CustomMap.transform.position = Vector3.zero;
		GetComponent<AIManager>().StartSpawning();
		ChangeSongs();
	}

	public void ChangeSongs()
	{
		int songID = Random.Range(0, Tracks.Count);
		ChooseSong(songID);
	}

	public void ChangeAmbience()
	{
		int songID = Random.Range(0, AmbientTracks.Count);
		ChooseAmbience(songID);
	}

	public void ChooseSong(int SongID)
	{
		foreach (GTTODTrack track in Tracks)
		{
			track.PlayAudio(SongID);
		}
	}

	public void ChooseAmbience(int SongID)
	{
		foreach (GTTODAmbientTrack ambientTrack in AmbientTracks)
		{
			ambientTrack.PlayAudio(SongID);
		}
	}

	public void StopMusic()
	{
		Audio.Stop();
	}

	public void Gain(float slowmo)
	{
		Slowmo.GainSlowmo(slowmo);
		PointsMultiplyer *= 2;
		Points += PointsMultiplyer;
		if (PointsMultiplyer >= MaxMultiplier)
		{
			PointsMultiplyer = MaxMultiplier;
		}
		CheckPointsUI();
		ScoreCanvas.RestartUI();
		AIManager.TalkerReact();
	}

	public void ResetPoints()
	{
		PointsMultiplyer = 1;
		Points = 0;
		CheckPointsUI();
	}

	public void CheckPointsUI()
	{
		if (Points < 0)
		{
			Points = 0;
		}
		ScoreCanvas.PointsUI.text = Points.ToString();
	}

	public void ReduceMultiplyer()
	{
		PointsMultiplyer = 1;
		CheckPointsUI();
		ScoreCanvas.DisableUI();
	}

	public void GainAchievement(string AchievementName)
	{
		SteamUserStats.SetAchievement(AchievementName);
		SteamUserStats.StoreStats();
	}

	public void PlayerDeath()
	{
		ResetPoints();
		ResetLevel();
	}

	public void EnterDoor(bool ExitLevel)
	{
		Player.transform.position = Enterance.MySpot.position;
		Player.GetComponent<ac_CharacterController>().ReParent();
		Player.SetXRotation(0f);
		Player.SetYRotation(0f);
		Player.Unground();
		Player.StopDashing();
		Player.GetComponent<Rigidbody>().AddForce(base.transform.forward * 500f);
		Player.GetComponent<InventoryScript>().OpenDoor();
		Player.RevertPoint.position = Vector3.zero;
		Enterance.GetComponent<Animation>().Play("EnterDoor");
		Enterance.Bump();
		if (ExitLevel)
		{
			GameManager.GM.DoorsOpenedStat++;
			GameManager.GM.UpdateStats();
			SurfacePortal.ResetIsland();
			if (GameMode == 0)
			{
				scenarioManager.StartScenario();
				GrabAGun.ToggleProgressionType(IsSaved: true);
				GrabAGun.SetUpProgression();
			}
			if (GameMode == 1)
			{
				SwitchWorld(Valley: false);
				GrabAGun.ToggleProgressionType(IsSaved: true);
				GrabAGun.SetUpProgression();
				GetComponent<LevelManager>().NextLevel();
				StartCoroutine(CheckCardUnlocks());
				if (CurrentLevel != null)
				{
					Object.Destroy(CurrentLevel);
				}
				StartCoroutine(RestartGame());
			}
			if (GameMode == 2)
			{
				SwitchWorld(Valley: false);
				BuildRogueLevel();
				GrabAGun.ToggleProgressionType(IsSaved: false);
				GrabAGun.SetUpProgression();
				StartCoroutine(CheckCardUnlocks());
				StartCoroutine(GiveInformation("Spend points on new weapons and equipment and then travel to the surface to begin a new level"));
				StartCoroutine(TimedLoad());
			}
		}
		else if (GameMode == 2)
		{
			SwitchWorld(Valley: true);
		}
	}

	public IEnumerator GiveInformation(string Info)
	{
		if (!HasGivenInfo)
		{
			yield return new WaitForSeconds(15f);
			HasGivenInfo = true;
			Inventory.PrintTutorialMessage(Info);
		}
	}

	public IEnumerator ForceInformation(string Info)
	{
		yield return new WaitForSeconds(5f);
		Inventory.PrintTutorialMessage(Info);
	}

	public IEnumerator TimedLoad()
	{
		yield return new WaitForSeconds(Random.Range(10f, 20f));
		Inventory.PrintTutorialMessage("Level generated. Hold R to teleport to surface portal");
		Inventory.PortalActiveToggle(Active: true);
	}

	public IEnumerator RestartGame()
	{
		yield return new WaitForSeconds(10f);
		Player.GetComponent<PopUpManager>().InformPlayer("Overhaul is GTTOD's most WIP mode, and because of this, there is only one main level available for the time being. You can replay this level as much as you want, OR you can head back to the main menu for more modes. Your choice.");
	}

	public void SwitchWorld(bool Valley)
	{
		if (Valley)
		{
			Surface.SetActive(value: true);
			Abyss.SetActive(value: false);
			Island.SetActive(value: true);
			return;
		}
		Surface.SetActive(value: false);
		Abyss.SetActive(value: true);
		Island.SetActive(value: false);
		StopMusic();
		GetComponent<AIManager>().StopSpawning();
		Object.Instantiate(LevelComplete);
	}

	public IEnumerator CheckCardUnlocks()
	{
		yield return new WaitForSeconds(7f);
		StarterCard[] starterCards = StarterCards;
		for (int i = 0; i < starterCards.Length; i++)
		{
			starterCards[i].CheckCardUnlocks();
		}
	}

	public void SetLevelObject(GameObject Level)
	{
		CurrentLevel = Level;
	}
}
