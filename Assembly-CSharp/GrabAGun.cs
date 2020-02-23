using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabAGun : MonoBehaviour
{
	[Header("Grab A Gun Set-Up")]
	public GameObject ScreenParent;

	public GameObject IdleScreen;

	public GameObject ProcessingScreen;

	public GameObject FinishedScreen;

	[Header("Grab A Gun Settings")]
	public bool ChoiceWeapon;

	[ConditionalField("ChoiceWeapon", null)]
	public int WeaponID = 1;

	[Header("Grab A Gun Progression")]
	public float Level;

	public float LevelProgress;

	public Slider LevelIndicator;

	public Slider LevelProgression;

	[Header("Grab A Gun Visuals")]
	public Material GrabAGunMaterial;

	public Material LowProgression;

	public Material HighProgression;

	[Header("Grab A Gun Audio")]
	public AudioClip FinishedSFX;

	[HideInInspector]
	public List<int> PotentialWeapons;

	private InventoryScript Inventory;

	private GTTODManager Manager;

	private AudioSource Audio;

	private Transform Player;

	private bool inTrigger;

	private bool GrabWeapon;

	private bool CanInteract = true;

	private bool Saved;

	private int WeaponListLength;

	private int WeaponToChoose;

	private float ProgressDampener;

	private void Start()
	{
		Manager = GameManager.GM.GetComponent<GTTODManager>();
		Player = GameManager.GM.Player.transform;
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
		Audio = GetComponent<AudioSource>();
		SetUpProgression();
	}

	public void SetUpProgression()
	{
		if (Saved)
		{
			Level = PlayerPrefs.GetFloat("Grab A Gun Level", 0f);
			LevelProgress = PlayerPrefs.GetFloat("Grab A Gun Level Progress", 0f);
			if (Level < 4f)
			{
				if (Level == 0f)
				{
					ProgressDampener = 5f;
				}
				if (Level == 1f)
				{
					ProgressDampener = 10f;
				}
				if (Level == 2f)
				{
					ProgressDampener = 15f;
				}
				if (Level == 3f)
				{
					ProgressDampener = 20f;
				}
			}
			else
			{
				LevelProgress = 0.99f;
				PlayerPrefs.SetFloat("Grab A Gun Level Progress", LevelProgress);
			}
		}
		else if (Level < 4f)
		{
			if (Level == 0f)
			{
				ProgressDampener = 1f;
			}
			if (Level == 1f)
			{
				ProgressDampener = 2f;
			}
			if (Level == 2f)
			{
				ProgressDampener = 5f;
			}
			if (Level == 3f)
			{
				ProgressDampener = 10f;
			}
		}
		else
		{
			LevelProgress = 0.99f;
		}
	}

	private void Update()
	{
		if (inTrigger && CanInteract && (KeyBindingManager.GetKeyDown(KeyAction.Interact) || Input.GetButtonDown("X")))
		{
			if (!GrabWeapon)
			{
				if (Manager.Points >= 950)
				{
					if (ChoiceWeapon)
					{
						Inventory.GrabWeapon(WeaponID);
						CanInteract = false;
					}
					else
					{
						StartCoroutine(SetUpWeapons());
						Progress();
						CanInteract = false;
					}
				}
				else
				{
					Inventory.PrintMessage("Not enough points!");
				}
			}
			else
			{
				FinalInteract();
			}
		}
		if (Vector3.Distance(base.transform.position, Player.position) < 30f)
		{
			ScreenParent.SetActive(value: true);
		}
		else
		{
			ScreenParent.SetActive(value: false);
		}
		LevelIndicator.value = 1f - Level / 4f;
		LevelProgression.value = 1f - LevelProgress;
		GrabAGunMaterial.Lerp(LowProgression, HighProgression, LevelProgress);
	}

	public IEnumerator SetUpWeapons()
	{
		PotentialWeapons.Clear();
		foreach (WeaponItem weapon in Inventory.Weapons)
		{
			if ((float)weapon.WeaponLevel <= Level && weapon.CanUse)
			{
				PotentialWeapons.Add(Inventory.Weapons.IndexOf(weapon));
			}
		}
		yield return new WaitForSeconds(0.1f);
		GiveWeapon();
	}

	public void GiveWeapon()
	{
		WeaponListLength = PotentialWeapons.Count;
		WeaponToChoose = Random.Range(PotentialWeapons[0], PotentialWeapons[0] + WeaponListLength);
		if (WeaponToChoose != Inventory.PrimaryID && WeaponToChoose != Inventory.SecondaryID)
		{
			Manager.Points -= 950;
			Manager.CheckPointsUI();
			StartCoroutine(ProcessWeapon());
		}
		else
		{
			GiveWeapon();
		}
	}

	private IEnumerator ProcessWeapon()
	{
		CanInteract = false;
		Inventory.TradeWeapon();
		IdleScreen.SetActive(value: false);
		ProcessingScreen.SetActive(value: true);
		FinishedScreen.SetActive(value: false);
		yield return new WaitForSeconds(7f);
		IdleScreen.SetActive(value: false);
		ProcessingScreen.SetActive(value: false);
		FinishedScreen.SetActive(value: true);
		Audio.PlayOneShot(FinishedSFX);
		CanInteract = true;
		GrabWeapon = true;
	}

	public void FinalInteract()
	{
		Inventory.GrabWeapon(WeaponToChoose);
		IdleScreen.SetActive(value: true);
		ProcessingScreen.SetActive(value: false);
		FinishedScreen.SetActive(value: false);
		GrabWeapon = false;
	}

	public void Progress()
	{
		if (Level < 4f)
		{
			LevelProgress += 1f / ProgressDampener;
			if (LevelProgress >= 1f)
			{
				LevelUp();
			}
			else
			{
				PlayerPrefs.SetFloat("Grab A Gun Level Progress", LevelProgress);
			}
		}
	}

	public void LevelUp()
	{
		Level += 1f;
		LevelProgress = 0f;
		PlayerPrefs.SetFloat("Grab A Gun Level", Level);
		PlayerPrefs.SetFloat("Grab A Gun Level Progress", LevelProgress);
		SetUpProgression();
	}

	public void ResetProgress()
	{
		Level = 0f;
		LevelProgress = 0f;
		PlayerPrefs.SetFloat("Grab A Gun Level", Level);
		PlayerPrefs.SetFloat("Grab A Gun Level Progress", LevelProgress);
	}

	public void ToggleProgressionType(bool IsSaved)
	{
		if (IsSaved)
		{
			Saved = true;
		}
		else
		{
			Saved = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			inTrigger = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			inTrigger = false;
		}
	}
}
