using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class TimeSlow : MonoBehaviour
{
	public bool Active = true;

	public GameManager GM;

	public AudioLowPassFilter Filter;

	public PostProcessVolume PostProcessor;

	public float slowdownFactor = 0.5f;

	public float NormalFactor = 1f;

	public float slowdownLength = 5f;

	public float DrainSpeed = 0.05f;

	public AudioSource Music;

	public GameObject SlowmoIn;

	public Image LeftSlider;

	public Image RightSlider;

	public Image Icon;

	public Color SlowMoColor;

	private ac_CharacterController Player;

	private AudioSource SFX;

	private Color ColorSave;

	private Image LeftShield;

	private Image RightShield;

	private float MaxSlowmo = 1f;

	private float SlowmoAmount = 2f;

	private bool HoldSlow;

	private bool canSlow = true;

	private bool canUnSlow;

	private bool GameStopped;

	[Header("POST PROCESSING")]
	private ColorGrading ColorGradingProcessing;

	private void Start()
	{
		Player = GM.Player.GetComponent<ac_CharacterController>();
		SFX = GetComponent<AudioSource>();
		LeftShield = GetComponent<HealthScript>().LeftShield;
		RightShield = GetComponent<HealthScript>().RightShield;
		ColorSave = LeftSlider.color;
		PostProcessor.profile.TryGetSettings(out ColorGradingProcessing);
	}

	private void Update()
	{
		if (!Active)
		{
			return;
		}
		if (!GameStopped)
		{
			InputUpdate();
			AbilityUpdate();
			TimeScaleUpdate();
			if (Time.timeScale == 1f)
			{
				GainSlowmo(0.00035f);
			}
		}
		else
		{
			UpdateGameStop();
		}
		if (Player.Swimming)
		{
			Filter.cutoffFrequency = 575f;
		}
		else if (Time.timeScale == 1f && !HoldSlow)
		{
			Filter.cutoffFrequency = 22000f;
		}
	}

	private void InputUpdate()
	{
		if (KeyBindingManager.GetKeyDown(KeyAction.SlowMo) || Input.GetButtonDown("LeftBumper"))
		{
			if (canSlow)
			{
				StartSlowmo();
			}
			if (canUnSlow)
			{
				StopSlowmo();
			}
		}
		if ((KeyBindingManager.GetKeyUp(KeyAction.SlowMo) || Input.GetButtonUp("LeftBumper")) && !canSlow)
		{
			canUnSlow = true;
		}
	}

	public void StartSlowmo()
	{
		canSlow = false;
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
		ColorGradingProcessing.temperature.value = 100f;
		LeftSlider.color = SlowMoColor;
		RightSlider.color = SlowMoColor;
		LeftShield.color = SlowMoColor;
		RightShield.color = SlowMoColor;
		Icon.color = SlowMoColor;
		Object.Instantiate(SlowmoIn);
		HoldSlow = true;
		Music.pitch = 0.75f;
		Filter.cutoffFrequency = 575f;
	}

	public void StopSlowmo()
	{
		Time.timeScale = NormalFactor;
		Time.fixedDeltaTime = 0.02f;
		ColorGradingProcessing.temperature.value = -15f;
		LeftSlider.color = ColorSave;
		RightSlider.color = ColorSave;
		LeftShield.color = ColorSave;
		RightShield.color = ColorSave;
		Icon.color = ColorSave;
		Music.pitch = 1f;
		HoldSlow = false;
		canUnSlow = false;
		Filter.cutoffFrequency = 22000f;
		if (SlowmoAmount > 0f)
		{
			canSlow = true;
		}
	}

	private void AbilityUpdate()
	{
		if (HoldSlow)
		{
			LeftSlider.fillAmount = SlowmoAmount / MaxSlowmo;
			RightSlider.fillAmount = SlowmoAmount / MaxSlowmo;
			SlowmoAmount -= DrainSpeed;
			if (SlowmoAmount <= 0f && HoldSlow)
			{
				SlowmoAmount = 0f;
				LeftSlider.color = ColorSave;
				RightSlider.color = ColorSave;
				LeftShield.color = ColorSave;
				RightShield.color = ColorSave;
				Icon.color = ColorSave;
				HoldSlow = false;
			}
		}
	}

	private void TimeScaleUpdate()
	{
		if (!HoldSlow && Time.timeScale != 1f && !GameStopped)
		{
			canUnSlow = false;
			Time.timeScale += 1f / slowdownLength * Time.unscaledDeltaTime;
			Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
			ColorGradingProcessing.temperature.value = Mathf.Lerp(ColorGradingProcessing.temperature.value, -15f, 0.05f);
			Filter.cutoffFrequency = Mathf.Lerp(Filter.cutoffFrequency, 22000f, 0.05f);
			Music.pitch = Mathf.Lerp(Music.pitch, 1f, 0.05f);
		}
	}

	public void GainSlowmo(float GainAmount)
	{
		SlowmoAmount += GainAmount;
		if (SlowmoAmount > MaxSlowmo)
		{
			SlowmoAmount = MaxSlowmo;
		}
		LeftSlider.fillAmount = SlowmoAmount / MaxSlowmo;
		RightSlider.fillAmount = SlowmoAmount / MaxSlowmo;
		canSlow = true;
	}

	public void StopGameTime(bool CutOffFilter, bool Instant)
	{
		GameStopped = true;
		if (CutOffFilter)
		{
			Filter.cutoffFrequency = 575f;
		}
		if (Instant)
		{
			Time.timeScale = 0.1f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
	}

	public void StartGameTime(bool Instant)
	{
		GameStopped = false;
		if (Instant)
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
	}

	public void UpdateGameStop()
	{
		if (Time.timeScale > 0.05f)
		{
			Time.timeScale -= Time.unscaledDeltaTime * 2f;
		}
		else
		{
			Time.timeScale = 0.05f;
		}
		Time.fixedDeltaTime = Time.timeScale * 0.02f;
	}
}
