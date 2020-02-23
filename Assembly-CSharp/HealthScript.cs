using EZCameraShake;
using RetroAesthetics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
	[Header("HEALTH SETTINGS")]
	public bool CanRegen = true;

	public bool Immune;

	public float DamageMultiplier = 1f;

	public float Health = 100f;

	public float Shield = 100f;

	public int Lives = 1;

	[Header("HEALTH SET-UP")]
	public Material HealthMaterial;

	public Image LeftHealth;

	public Image RightHealth;

	public Image LeftShield;

	public Image RightShield;

	public Image DamageFlash;

	public Image ShieldIcon;

	public Image HealthIcon;

	public GameObject DamageBeeps;

	public AudioSource RegenSFX;

	public GameObject HUD;

	public Transform CutsceneManager;

	public GameObject DeathSFX;

	private LayerMask AvailableLayers;

	private GameManager GM;

	private ac_CharacterController CharacterController;

	private GTTODManager Manager;

	private InventoryScript Inventory;

	private MenuScript PauseMenu;

	private AudioLowPassFilter Filter;

	private RetroCameraEffect GlitchEffects;

	private Animation CutsceneAnim;

	private RaycastHit HitD;

	private Color HealthHUDColor;

	private Color FullHealthColor;

	private Color HealthBumpColor;

	private Color FlashHUDColor;

	private float RegenTime = 3f;

	private float FlashAmount;

	private bool WasDamaged;

	private bool PermaDead;

	private bool ShieldOnly;

	private void Start()
	{
		GM = GameManager.GM;
		PauseMenu = GM.GetComponent<MenuScript>();
		Manager = GM.GetComponent<GTTODManager>();
		CharacterController = GetComponent<ac_CharacterController>();
		Inventory = GetComponent<InventoryScript>();
		Filter = CharacterController.MainCamera.GetComponent<AudioLowPassFilter>();
		GlitchEffects = CharacterController.MainCamera.GetComponent<RetroCameraEffect>();
		CutsceneAnim = CutsceneManager.gameObject.GetComponent<Animation>();
		AvailableLayers = CharacterController.AvailableLayers;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.J))
		{
			TrueDamage(200);
		}
		HealthHUDColor.a = 1f - Health * 0.01f;
		RightShield.fillAmount = Shield / 100f;
		LeftShield.fillAmount = Shield / 100f;
		RightHealth.fillAmount = Health / 100f;
		LeftHealth.fillAmount = Health / 100f;
		if (RegenTime <= 0f && Shield < 100f && CanRegen && !PauseMenu.inMenu)
		{
			Shield += Time.deltaTime * 100f / 3f;
			RegenSFX.pitch = Shield / 50f;
			if (!RegenSFX.isPlaying)
			{
				RegenSFX.Play();
			}
		}
		else
		{
			RegenTime -= Time.deltaTime;
			RegenSFX.Stop();
		}
		float x = 20f * Health / 100f;
		float num = 3.5f * Health / 100f;
		float z = 20f * Health / 100f;
		FlashAmount = Mathf.Lerp(FlashAmount, 0f, 0.05f);
		FlashHUDColor.a = FlashAmount;
		DamageFlash.color = FlashHUDColor;
		if (Shield >= 100f)
		{
			Shield = 100f;
		}
		if (Shield <= 0f)
		{
			DamageBeeps.SetActive(value: true);
			ShieldIcon.gameObject.SetActive(value: false);
			HealthIcon.gameObject.SetActive(value: true);
		}
		else
		{
			DamageBeeps.SetActive(value: false);
			ShieldIcon.gameObject.SetActive(value: true);
			HealthIcon.gameObject.SetActive(value: false);
		}
		if (Health <= 20f && Shield <= 0f)
		{
			GlitchEffects.enabled = true;
			WasDamaged = true;
			if (!PermaDead)
			{
				Filter.cutoffFrequency = Mathf.Lerp(Filter.cutoffFrequency, 1000f, 0.1f);
			}
		}
		else
		{
			BumpBackSound();
			GlitchEffects.enabled = false;
		}
		if (Inventory.CurrentWeapon != null)
		{
			if (Inventory.CurrentWeapon.PrimaryOn)
			{
				FullHealthColor = new Vector4(x, num, 0f, 0f);
				HealthMaterial.SetColor("_EmissionColor", FullHealthColor);
			}
			else
			{
				FullHealthColor = new Vector4(0f, num * 2f, z, 0f);
				HealthMaterial.SetColor("_EmissionColor", FullHealthColor);
			}
		}
		else
		{
			FullHealthColor = new Vector4(x, num, 0f, 0f);
			HealthMaterial.SetColor("_EmissionColor", FullHealthColor);
		}
		if (CanRegen)
		{
			PauseMenu.HUD.SetTrigger("Stable");
		}
		else
		{
			PauseMenu.HUD.SetTrigger("Unstable");
		}
		if (ShieldOnly)
		{
			Health = Shield;
		}
	}

	public void BumpBackSound()
	{
		if (WasDamaged)
		{
			Filter.cutoffFrequency = 22000f;
			WasDamaged = false;
		}
	}

	public void Damage(int damage)
	{
		if (!GM.AntiLife)
		{
			if (!Immune)
			{
				if (Shield > 0f)
				{
					Shield -= (float)damage * DamageMultiplier;
					FlashAmount += 0.1f;
					RegenTime = 10f;
					CameraShaker.Instance.ShakeOnce(1.5f, 0.75f, 0.1f, 0.25f);
					return;
				}
				Shield = 0f;
				Health -= 20f * DamageMultiplier;
				Immune = true;
				CheckDeath();
				FlashAmount += 0.1f;
				RegenTime = 10f;
				StartCoroutine(Immunity());
				CameraShaker.Instance.ShakeOnce(2f, 0.75f, 0.1f, 0.25f);
			}
		}
		else
		{
			Die();
		}
	}

	public void TrueDamage(int damage)
	{
		if (!GM.AntiLife)
		{
			if (Shield > 0f)
			{
				Shield -= (float)damage * DamageMultiplier;
				FlashAmount += 0.1f;
				RegenTime = 5f;
			}
			else if (Shield < 0f)
			{
				Health += Shield - (float)damage * DamageMultiplier;
				Shield = 0f;
				CheckDeath();
				FlashAmount += 0.1f;
				RegenTime = 10f;
				CameraShaker.Instance.ShakeOnce(5f, 0.75f, 0.1f, 2f);
			}
			else
			{
				Shield = 0f;
				Health -= (float)damage * DamageMultiplier;
				CheckDeath();
				FlashAmount += 0.1f;
				RegenTime = 10f;
				CameraShaker.Instance.ShakeOnce(5f, 0.75f, 0.1f, 2f);
			}
		}
		else
		{
			Die();
		}
		if (Shield < 0f)
		{
			TrueDamage(0);
		}
	}

	public IEnumerator Immunity()
	{
		Immune = true;
		yield return new WaitForSeconds(1f);
		Immune = false;
	}

	public void Heal(int GainHealth)
	{
		if (Health < 100f)
		{
			Health += GainHealth;
			if (GainHealth >= 10)
			{
				RegenTime = 0f;
			}
			LowerHealth();
		}
	}

	private void LowerHealth()
	{
		if (Health > 100f)
		{
			Health = 100f;
		}
	}

	private void CheckDeath()
	{
		if (Health <= 0f)
		{
			Die();
		}
	}

	public void Die()
	{
		Manager.PlayerDeath();
		Lives--;
		CharacterController.StopDashing();
		Manager.GetComponent<AIManager>().RestartSpawning();
		if (Lives > 0)
		{
			CharacterController.WeaponCamera.GetComponent<Animation>().Play("CameraOn");
			CharacterController.WeaponCamera.GetComponent<AudioSource>().Play();
			CharacterController.SetXRotation(0f);
			CharacterController.SetYRotation(0f);
			base.transform.position = Vector3.zero;
			Inventory.SetPrimaryID(0);
			Inventory.SetSecondaryID(0);
			Health = 100f;
			Shield = 100f;
			RegenTime = 0f;
			Immune = false;
			return;
		}
		if (CharacterController.inAir && Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), base.transform.up * -1f, out HitD, 50f, AvailableLayers))
		{
			CutsceneManager.position = new Vector3(CharacterController.MainCamera.transform.position.x, HitD.point.y + 1.35f, CharacterController.MainCamera.transform.position.z);
		}
		else
		{
			CutsceneManager.position = CharacterController.MainCamera.transform.position;
			CutsceneManager.rotation = CharacterController.MainCamera.transform.rotation;
		}
		CharacterController.CutsceneStart();
		CutsceneManager.gameObject.SetActive(value: true);
		PermaDead = true;
		Manager.StopMusic();
		Manager.gameObject.GetComponent<AIManager>().ClearEnemies();
		CutsceneManager.GetComponent<TutorialStarter>().FixCamera();
		CutsceneAnim.Stop();
		CutsceneAnim.Play("Die");
		Object.Instantiate(DeathSFX);
		Filter.cutoffFrequency = 22000f;
		StartCoroutine(GameOver());
	}

	public IEnumerator GameOver()
	{
		yield return new WaitForSeconds(5.25f);
		CutsceneManager.gameObject.SetActive(value: false);
		GameManager.GM.GetComponent<MenuScript>().GameOver();
	}

	public void SetShieldsOnly()
	{
		ShieldOnly = true;
	}
}
