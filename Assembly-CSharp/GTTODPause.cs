using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GTTODPause : MonoBehaviour
{
	[Header("References")]
	public GameObject Player;

	public Camera Camera;

	public MenuScript MainMenu;

	public GameObject Pause;

	public GameObject SettingsOptions;

	public GameObject Settings;

	public GameObject Keybinds;

	public GameObject RestartOptions;

	public Dropdown resolutionDropdown;

	[Header("Audio")]
	public Slider VolumeSlider;

	public Slider MusicSlider;

	public AudioSource Music;

	[Header("Mouse and Camera")]
	public Slider FOVSlider;

	public Text FOVSliderValue;

	public Slider SensSlider;

	public Text SensSliderValue;

	public Slider PostSlider;

	private bool isPaused;

	private bool isInverted;

	private ac_CharacterController Mouse;

	private float Volume;

	private float MusicVolume;

	private float FOV;

	private float Sensitivity;

	private Resolution[] resolutions;

	public void Start()
	{
		Sensitivity = PlayerPrefs.GetFloat("SensitivitySave");
		SensSlider.value = PlayerPrefs.GetFloat("SensitivitySave");
		FOV = PlayerPrefs.GetFloat("FOVSave");
		FOVSlider.value = PlayerPrefs.GetFloat("FOVSave");
		Volume = PlayerPrefs.GetFloat("VolumeSave");
		VolumeSlider.value = (Volume = PlayerPrefs.GetFloat("VolumeSave"));
		MusicVolume = PlayerPrefs.GetFloat("MusicVolumeSave");
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolumeSave");
		if (FOV < 60f)
		{
			FOV = 90f;
			PlayerPrefs.SetFloat("FOVSave", FOV);
		}
		if (Sensitivity < 1f)
		{
			Sensitivity = 2f;
			PlayerPrefs.SetFloat("SensitivitySave", Sensitivity);
		}
		if (Volume == 0f)
		{
			Volume = 1f;
			PlayerPrefs.SetFloat("VolumeSave", Volume);
		}
		if (MusicVolume == 0f)
		{
			MusicVolume = 1f;
			PlayerPrefs.SetFloat("MusicVolumeSave", MusicVolume);
		}
		Mouse = Player.GetComponent<ac_CharacterController>();
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();
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
		resolutionDropdown.AddOptions(list);
		resolutionDropdown.value = value;
		resolutionDropdown.RefreshShownValue();
	}

	private void Update()
	{
		Mouse.Sensitivity = Sensitivity;
		SensSliderValue.text = Sensitivity.ToString();
		AudioListener.volume = Volume;
		Music.volume = MusicVolume;
		FOVSliderValue.text = FOV.ToString("0");
		if (Input.GetKeyUp(KeyCode.Escape) && !isPaused)
		{
			DoPause();
		}
	}

	public void DoPause()
	{
		Pause.SetActive(value: true);
		Pause.GetComponent<Animator>().SetTrigger("Open");
		Player.GetComponent<TimeSlow>().enabled = false;
		Time.timeScale = 0f;
		Time.fixedDeltaTime = Time.timeScale;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		isPaused = true;
	}

	public void UnPause()
	{
		Pause.SetActive(value: false);
		Player.GetComponent<TimeSlow>().enabled = true;
		Time.timeScale = 0.8f;
		Time.fixedDeltaTime = 0.02f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		isPaused = false;
		PlayerPrefs.SetFloat("SensitivitySave", Sensitivity);
		PlayerPrefs.SetFloat("FOVSave", FOV);
		PlayerPrefs.SetFloat("VolumeSave", Volume);
		PlayerPrefs.SetFloat("MusicVolumeSave", MusicVolume);
		Sensitivity = PlayerPrefs.GetFloat("SensitivitySave");
		FOV = PlayerPrefs.GetFloat("FOVSave");
		Volume = PlayerPrefs.GetFloat("VolumeSave");
		MusicVolume = PlayerPrefs.GetFloat("MusicVolumeSave");
	}

	public void OpenSettingsOptions()
	{
		SettingsOptions.SetActive(value: true);
	}

	public void OpenSettings()
	{
		Settings.SetActive(value: true);
	}

	public void OpenKeybinds()
	{
		Keybinds.SetActive(value: true);
	}

	public void Back()
	{
		SettingsOptions.SetActive(value: false);
		Settings.SetActive(value: false);
		Keybinds.SetActive(value: false);
		RestartOptions.SetActive(value: false);
	}

	public void OpenRestartOptions()
	{
		RestartOptions.SetActive(value: true);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void OnValueChanged()
	{
		Volume = VolumeSlider.value;
		MusicVolume = MusicSlider.value;
		FOV = FOVSlider.value;
		Camera.fieldOfView = FOV;
		Sensitivity = SensSlider.value;
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetVsync(bool isVsync)
	{
		if (isVsync)
		{
			QualitySettings.vSyncCount = 1;
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
	}

	public void SetResolution(int resolutionIndex)
	{
		Screen.fullScreen = false;
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		Screen.fullScreen = true;
	}
}
