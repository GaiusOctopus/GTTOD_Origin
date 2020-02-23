using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
	public bool CanPauseGame;

	[Header("Set-Up")]
	public GameObject Player;

	public Camera PlayerCamera;

	public Camera WeaponCamera;

	public Animator MenuCamera;

	public PostProcessVolume PostProcessor;

	public PostProcessVolume MainPostProcessor;

	public GameObject MenuBuzz;

	[Header("Menu Screens")]
	public GameObject Menu;

	public GameObject Selections;

	public GameObject Options;

	public GameObject Keybinds;

	public GameObject ExitOptions;

	public GameObject GameOverOptions;

	public GameObject LevelSelect;

	public GameObject NotesScreen;

	[Header("Master Volume Options")]
	public float MasterVolume;

	public Slider VolumeSlider;

	public Text VolumeValue;

	[Header("Music Volume Options")]
	public float MusicVolume;

	public Slider MusicSlider;

	public Text MusicValue;

	[Header("Sensitivity Options")]
	public float Sensitivity;

	public Slider SensitivitySlider;

	public Text SensitivityValue;

	[Header("Field-of-View Options")]
	public FOVManager FOVManager;

	public float FieldOfView;

	public Slider FOVSlider;

	public Text FOVValue;

	[Header("Color Options")]
	public float Color;

	public Slider ColorSlider;

	public Text ColorValue;

	[Header("Bloom Options")]
	public float Bloom;

	public Slider BloomSlider;

	public Text BloomValue;

	[Header("Camera-Shake Options")]
	public float CameraShake;

	public Slider CameraShakeSlider;

	public Text CameraShakeValue;

	[Header("Dropdown Options")]
	public Dropdown ResolutionDropdown;

	public Resolution[] resolutions;

	public Dropdown QualityDropdown;

	[Header("Toggle Options")]
	public Toggle VsyncToggle;

	public Toggle InvertToggle;

	public Toggle Fullscreen;

	public Toggle CrouchToggle;

	public Toggle KeyboardMouseToggle;

	public Toggle AmbientOcclusionToggle;

	[Header("Player HUD")]
	public Animator HUD;

	public GameObject HUDElements;

	public Text TimerText;

	[Header("Player HUD")]
	public Text NotesTitle;

	public Text NotesBody;

	public List<Notes> Notes;

	[Header("End Public Variables")]
	[HideInInspector]
	public bool canSwitch;

	[HideInInspector]
	public bool inMenu;

	private bool Vsync;

	private bool Invert;

	private bool Crouch;

	private bool KeyboardMouse;

	private bool AmbientOcclusion = true;

	private ac_CharacterController CharacterController;

	private int QualityLevel;

	private float GameTime;

	private float RoundedGameTime;

	[Header("POST PROCESSING")]
	private ColorGrading ColorGradingProcessing;

	private Bloom BloomProcessing;

	private AmbientOcclusion AmbientOcclusionProcessing;

	public void Start()
	{
		PostProcessor.profile.TryGetSettings(out ColorGradingProcessing);
		PostProcessor.profile.TryGetSettings(out BloomProcessing);
		MainPostProcessor.profile.TryGetSettings(out AmbientOcclusionProcessing);
		CharacterController = Player.GetComponent<ac_CharacterController>();
		Player.GetComponent<InventoryScript>().EnableWeapon();
		resolutions = Screen.resolutions;
		ResolutionDropdown.ClearOptions();
		List<string> list = new List<string>();
		int value = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string item = resolutions[i].width + " x " + resolutions[i].height;
			list.Add(item);
			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				value = i;
			}
		}
		ResolutionDropdown.AddOptions(list);
		ResolutionDropdown.value = value;
		ResolutionDropdown.RefreshShownValue();
		SetSettings();
	}

	public void Update()
	{
		if (!CanPauseGame)
		{
			return;
		}
		if (Player.activeInHierarchy)
		{
			if (canSwitch && !Player.GetComponent<PopUpManager>().PopUpOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")))
			{
				if (!inMenu)
				{
					OpenMenu();
					canSwitch = false;
				}
				else if (Selections.activeInHierarchy)
				{
					CloseMenu();
					canSwitch = false;
				}
				else
				{
					Back();
					canSwitch = false;
				}
			}
			if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Start"))
			{
				canSwitch = true;
			}
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start"))
		{
			if (!inMenu)
			{
				OpenMenu();
				canSwitch = false;
			}
			else if (Selections.activeInHierarchy)
			{
				CloseMenu();
				canSwitch = false;
			}
			else
			{
				Back();
				canSwitch = false;
			}
		}
		if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Start"))
		{
			canSwitch = true;
		}
	}

	public void OpenMenu()
	{
		if (Player.activeInHierarchy)
		{
			CharacterController.FreezePlayer();
			inMenu = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0f;
			Player.GetComponent<TimeSlow>().enabled = false;
			Object.Instantiate(MenuBuzz);
			Menu.SetActive(value: true);
			CloseMenus();
			Selections.SetActive(value: true);
			HUD.gameObject.SetActive(value: false);
		}
		else
		{
			SoftOpen();
		}
	}

	public void CloseMenu()
	{
		if (Player.activeInHierarchy)
		{
			CharacterController.UnFreezePlayer();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Time.timeScale = 1f;
			Player.GetComponent<TimeSlow>().enabled = true;
			Player.GetComponent<TimeSlow>().StopSlowmo();
			WeaponCamera.gameObject.GetComponent<Animation>().Play("CameraOn");
			HUD.gameObject.SetActive(value: true);
		}
		else
		{
			GetComponent<GTTODManager>().Back();
		}
		SaveSettings();
		RefreshProcessing();
		FOVManager.CheckFOV();
		Menu.SetActive(value: false);
		inMenu = false;
	}

	public void ForceCloseMenu()
	{
		CharacterController.UnFreezePlayer();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f;
		Player.GetComponent<TimeSlow>().enabled = true;
		Player.GetComponent<TimeSlow>().StopSlowmo();
		WeaponCamera.gameObject.GetComponent<Animation>().Play("CameraOn");
		HUD.gameObject.SetActive(value: true);
		SaveSettings();
		RefreshProcessing();
		FOVManager.CheckFOV();
		Menu.SetActive(value: false);
		inMenu = false;
	}

	public void GameOver()
	{
		CharacterController.FreezePlayer();
		inMenu = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0f;
		Player.GetComponent<TimeSlow>().enabled = false;
		Object.Instantiate(MenuBuzz);
		Menu.SetActive(value: true);
		HUD.gameObject.SetActive(value: false);
		CloseMenus();
		GameOverOptions.SetActive(value: true);
	}

	public void SelectLevel()
	{
		CharacterController.FreezePlayer();
		inMenu = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0f;
		Player.GetComponent<TimeSlow>().enabled = false;
		Object.Instantiate(MenuBuzz);
		Menu.SetActive(value: true);
		HUD.gameObject.SetActive(value: false);
		CloseMenus();
		LevelSelect.SetActive(value: true);
	}

	public void OpenNotes(int NoteToOpen)
	{
		CharacterController.FreezePlayer();
		inMenu = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0f;
		Player.GetComponent<TimeSlow>().enabled = false;
		Object.Instantiate(MenuBuzz);
		Menu.SetActive(value: true);
		HUD.gameObject.SetActive(value: false);
		CloseMenus();
		NotesTitle.text = Notes[NoteToOpen].NoteTitle;
		NotesBody.text = Notes[NoteToOpen].NoteBody;
		NotesScreen.SetActive(value: true);
	}

	public void SoftOpen()
	{
		inMenu = true;
		Object.Instantiate(MenuBuzz);
		Menu.SetActive(value: true);
		CloseMenus();
		GetComponent<GTTODManager>().CloseAllMenus();
		Selections.SetActive(value: true);
	}

	public void OpenOptions()
	{
		CloseMenus();
		Options.SetActive(value: true);
		MenuCamera.SetTrigger("Transition");
		Object.Instantiate(MenuBuzz);
	}

	public void OpenKeybinds()
	{
		CloseMenus();
		Keybinds.SetActive(value: true);
		MenuCamera.SetTrigger("Transition");
		Object.Instantiate(MenuBuzz);
	}

	public void OpenExitOptions()
	{
		CloseMenus();
		ExitOptions.SetActive(value: true);
		MenuCamera.SetTrigger("Transition");
		Object.Instantiate(MenuBuzz);
	}

	public void KillPlayer()
	{
		if (Player.activeInHierarchy)
		{
			CloseMenu();
			CharacterController.gameObject.GetComponent<HealthScript>().Die();
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(1);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f;
		Object.Destroy(base.gameObject);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void Discord()
	{
		Application.OpenURL("https://discord.gg/PBpYjwu");
	}

	public void Reddit()
	{
		Application.OpenURL("https://www.reddit.com/r/GTTOD/");
	}

	public void Back()
	{
		CloseMenus();
		Selections.SetActive(value: true);
		MenuCamera.SetTrigger("Transition");
		Object.Instantiate(MenuBuzz);
	}

	public void SetSettings()
	{
		VolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 50f);
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 50f);
		SensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 20f);
		FOVSlider.value = PlayerPrefs.GetFloat("FieldOfView", 100f);
		ColorSlider.value = PlayerPrefs.GetFloat("Color", 180f);
		CameraShakeSlider.value = PlayerPrefs.GetFloat("CameraShake", 100f);
		BloomSlider.value = PlayerPrefs.GetFloat("Bloom", 100f);
		QualityDropdown.value = PlayerPrefs.GetInt("Quality", 0);
		Vsync = PlayerPrefsPlus.GetBool("VsyncToggle", defaultValue: true);
		Invert = PlayerPrefsPlus.GetBool("Invert", defaultValue: false);
		Crouch = PlayerPrefsPlus.GetBool("CrouchOn", defaultValue: true);
		KeyboardMouse = PlayerPrefsPlus.GetBool("KeyboardMouse", defaultValue: true);
		AmbientOcclusion = PlayerPrefsPlus.GetBool("AmbientOcclusion", defaultValue: true);
		OnValueChanged();
		FOVManager.CheckFOV();
		SetQuality(QualityDropdown.value);
	}

	public void OnValueChanged()
	{
		MasterVolume = VolumeSlider.value;
		MusicVolume = MusicSlider.value;
		Sensitivity = SensitivitySlider.value;
		FieldOfView = FOVSlider.value;
		Color = ColorSlider.value;
		CameraShake = CameraShakeSlider.value;
		Bloom = BloomSlider.value;
		UpdateSettingEffects();
		RefreshValues();
	}

	public void RefreshValues()
	{
		VolumeValue.text = MasterVolume.ToString();
		MusicValue.text = MusicVolume.ToString();
		SensitivityValue.text = Sensitivity.ToString();
		FOVValue.text = FieldOfView.ToString();
		ColorValue.text = Color.ToString();
		CameraShakeValue.text = CameraShake.ToString();
		BloomValue.text = Bloom.ToString();
		QualityDropdown.value = QualityLevel;
		VsyncToggle.isOn = Vsync;
		InvertToggle.isOn = Invert;
		Fullscreen.isOn = Screen.fullScreen;
		CrouchToggle.isOn = Crouch;
		KeyboardMouseToggle.isOn = KeyboardMouse;
		AmbientOcclusionToggle.isOn = AmbientOcclusion;
	}

	public void RefreshProcessing()
	{
		if (QualityLevel > 0)
		{
			if (QualityLevel == 4)
			{
				WeaponCamera.gameObject.GetComponent<PixelateImageEffect>().enabled = true;
			}
			else
			{
				WeaponCamera.gameObject.GetComponent<PixelateImageEffect>().enabled = false;
			}
			if (QualityLevel == 5)
			{
				PostProcessor.gameObject.SetActive(value: false);
			}
			else
			{
				PostProcessor.gameObject.SetActive(value: true);
			}
		}
		else
		{
			PostProcessor.gameObject.SetActive(value: true);
			WeaponCamera.gameObject.GetComponent<PixelateImageEffect>().enabled = false;
		}
		ColorGradingProcessing.hueShift.value = Color - 180f;
		BloomProcessing.intensity.value = Bloom / 2f;
		AmbientOcclusionProcessing.active = AmbientOcclusion;
	}

	public void UpdateSettingEffects()
	{
		AudioListener.volume = MasterVolume / 100f;
		GetComponent<AudioSource>().volume = MusicVolume / 100f;
		CharacterController.Sensitivity = Sensitivity / 10f;
		PlayerCamera.fieldOfView = FieldOfView;
		PlayerCamera.GetComponent<CameraShaker>().CameraShakeModifier = CameraShake / 100f;
		CharacterController.InvertControls = Invert;
		CharacterController.ToggleCrouch = Crouch;
		CharacterController.KeyboardMouse = KeyboardMouse;
		AmbientOcclusionProcessing.active = AmbientOcclusion;
	}

	public void SaveSettings()
	{
		PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
		PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
		PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
		PlayerPrefs.SetFloat("FieldOfView", FieldOfView);
		PlayerPrefs.SetFloat("Color", Color);
		PlayerPrefs.SetFloat("CameraShake", CameraShake);
		PlayerPrefs.SetFloat("Bloom", Bloom);
		PlayerPrefs.SetInt("Quality", QualityLevel);
		PlayerPrefsPlus.SetBool("VsyncToggle", Vsync);
		PlayerPrefsPlus.SetBool("Invert", Invert);
		PlayerPrefsPlus.SetBool("CrouchOn", Crouch);
		PlayerPrefsPlus.SetBool("KeyboardMouse", KeyboardMouse);
		PlayerPrefsPlus.SetBool("AmbientOcclusion", AmbientOcclusion);
		Player.GetComponent<InventoryScript>().UpdateWeaponFireButtons();
	}

	public void SetResolution(int resolutionIndex)
	{
		Screen.fullScreen = false;
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		Screen.fullScreen = true;
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
		QualityLevel = qualityIndex;
		RefreshProcessing();
	}

	public void SetVsync(bool isVsync)
	{
		Vsync = isVsync;
		if (isVsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
	}

	public void SetInvert(bool isInvert)
	{
		Invert = isInvert;
		UpdateSettingEffects();
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetCrouch(bool isCrouch)
	{
		Crouch = isCrouch;
		UpdateSettingEffects();
	}

	public void SetKeyboardMouse(bool isKeyboardMouse)
	{
		KeyboardMouse = isKeyboardMouse;
		UpdateSettingEffects();
	}

	public void SetAmbientOcclusion(bool HasAmbientOcclusion)
	{
		AmbientOcclusion = HasAmbientOcclusion;
		AmbientOcclusionProcessing.active = AmbientOcclusion;
		UpdateSettingEffects();
	}

	public void RefreshProcessingTimer()
	{
		StartCoroutine(TimedRefresh());
	}

	public IEnumerator TimedRefresh()
	{
		yield return new WaitForSeconds(0.5f);
		RefreshProcessing();
	}

	public void CloseMenus()
	{
		Selections.SetActive(value: false);
		Options.SetActive(value: false);
		Keybinds.SetActive(value: false);
		ExitOptions.SetActive(value: false);
		GameOverOptions.SetActive(value: false);
		LevelSelect.SetActive(value: false);
		NotesScreen.SetActive(value: false);
	}
}
