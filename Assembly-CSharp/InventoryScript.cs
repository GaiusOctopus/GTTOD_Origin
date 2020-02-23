using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
	public bool GiveOnAwake;

	[ConditionalField("GiveOnAwake", null)]
	public int WeaponToGive;

	[ConditionalField("GiveOnAwake", null)]
	public int EquipmentToGive;

	[Header("Inventory Set-Up")]
	public bool ShowSetUp;

	[ConditionalField("ShowSetUp", null)]
	public GameManager GM;

	[ConditionalField("ShowSetUp", null)]
	public Transform AmmoCounter;

	[ConditionalField("ShowSetUp", null)]
	public Text AmmoPoolText;

	[ConditionalField("ShowSetUp", null)]
	public Transform AmmoCounterDefaultPosition;

	[ConditionalField("ShowSetUp", null)]
	public Image Cooldown;

	[ConditionalField("ShowSetUp", null)]
	public bool HasMelee = true;

	[ConditionalField("ShowSetUp", null)]
	public bool HasEquipment;

	[ConditionalField("ShowSetUp", null)]
	public bool UnlimitedAmmo;

	[ConditionalField("ShowSetUp", null)]
	public AudioClip AbilityAvailableEffect;

	[Header("Weapon Directory")]
	public bool ShowWeapons;

	[ConditionalField("ShowWeapons", null)]
	public WeaponScript CurrentWeapon;

	[ConditionalField("ShowWeapons", null)]
	public WeaponScript PrimaryWeapon;

	[ConditionalField("ShowWeapons", null)]
	public WeaponScript SecondaryWeapon;

	[ConditionalField("ShowWeapons", null)]
	public bool isPrimary = true;

	[ConditionalField("ShowWeapons", null)]
	public bool CanSwitch = true;

	[Header("Attacks")]
	public bool ShowAttacks;

	[ConditionalField("ShowAttacks", null)]
	public float MeleeSpeed = 0.1f;

	[ConditionalField("ShowAttacks", null)]
	public float KickStrength = 3f;

	[ConditionalField("ShowAttacks", null)]
	public GameObject PunchObject;

	[ConditionalField("ShowAttacks", null)]
	public GameObject KickObject;

	[ConditionalField("ShowAttacks", null)]
	public GameObject Bullet;

	[ConditionalField("ShowAttacks", null)]
	public GameObject SwallowEffect;

	[Header("Interactions")]
	public bool ShowInteractions;

	[ConditionalField("ShowInteractions", null)]
	public Animator Arms;

	[ConditionalField("ShowInteractions", null)]
	public Animator Legs;

	[ConditionalField("ShowInteractions", null)]
	public Animator WeaponParent;

	[ConditionalField("ShowInteractions", null)]
	public Animation MessagePrint;

	[ConditionalField("ShowInteractions", null)]
	public Text MessageText;

	[ConditionalField("ShowInteractions", null)]
	public Text FancyMessageText;

	[ConditionalField("ShowInteractions", null)]
	public Text TutorialText;

	[ConditionalField("ShowInteractions", null)]
	public Text AmmoText;

	[ConditionalField("ShowInteractions", null)]
	public Text CounterText;

	[ConditionalField("ShowInteractions", null)]
	public GameObject TeleportEffect;

	[ConditionalField("ShowInteractions", null)]
	public GameObject Bubbles;

	[Header("Inventory")]
	public List<WeaponItem> Weapons;

	public List<EquipmentItem> Equipment;

	public List<AudioClip> MessageAudio;

	public List<AmmoType> AmmoTypes;

	[Header("Remove At")]
	public int IndexOf;

	[Header("Private Variables")]
	private AudioClip EquipmentSoundEffect;

	private CrosshairScript Crosshair;

	private ac_CharacterController Player;

	private GrenadeScript MyGrenade;

	private SuperHotTime SHT;

	private AudioSource Audio;

	private WeaponScript ClonedWeapon;

	private WeaponItem NewWeaponItem;

	private GameObject EquipmentObject;

	private Transform EquipmentTransform;

	private Image ConsumptionCooldown;

	private string EquipmentAnimation;

	private float EquipmentShakeMagnitude;

	private float EquipmentActivationTime;

	private float EquipmentDeactivationTime;

	private float EquipmentCoolDown;

	private float EquipmentTimer = 0.35f;

	private float GunConsumptionTimer = 1.5f;

	private bool isGrenade;

	private bool CanReflect = true;

	private bool isGrapple;

	private bool LoweredHand;

	private bool BusyHands;

	private bool RightPunch = true;

	private bool CoolingDown;

	private bool canPunch = true;

	private bool canUseEquipment = true;

	private bool IsFrozen;

	private bool PortalActive;

	private bool TriggeredSwim;

	private bool FinishedTreading;

	[HideInInspector]
	public bool InWheel;

	[HideInInspector]
	public int PrimaryID = -1;

	[HideInInspector]
	public int SecondaryID = -1;

	[HideInInspector]
	public int EquipmentID;

	[HideInInspector]
	public bool IsHolding;

	[HideInInspector]
	public bool DualWielding;

	private PostProcessVolume PostProcessor;

	private ColorGrading ColorGradingProcessing;

	public void Start()
	{
		Audio = GetComponent<AudioSource>();
		Player = GetComponent<ac_CharacterController>();
		Crosshair = GetComponent<CrosshairScript>();
		SHT = GetComponent<SuperHotTime>();
		ConsumptionCooldown = Crosshair.GunCooldown;
		PrimaryID = -1;
		SecondaryID = -1;
		PostProcessor = GetComponent<TimeSlow>().PostProcessor;
		PostProcessor.profile.TryGetSettings(out ColorGradingProcessing);
		SetAllWeaponIDs();
		if (GiveOnAwake)
		{
			StartCoroutine(GiveOnStart());
		}
		foreach (AmmoType ammoType in AmmoTypes)
		{
			ammoType.SetStartingAmmo();
		}
	}

	private IEnumerator GiveOnStart()
	{
		yield return new WaitForSeconds(0.5f);
		GrabWeapon(WeaponToGive);
		if (EquipmentToGive != 0)
		{
			AquireEquipment(EquipmentToGive);
		}
	}

	public void Update()
	{
		if (CurrentWeapon != null && !UnlimitedAmmo)
		{
			AmmoPoolText.text = AmmoTypes[CurrentWeapon.AmmoTypeIndex].AmmoAmount.ToString();
			AmmoCounter.gameObject.SetActive(value: true);
			if (CurrentWeapon.AmmoPoolLocation != null)
			{
				AmmoCounter.position = Vector3.MoveTowards(AmmoCounter.position, CurrentWeapon.AmmoPoolLocation.position, 0.01f);
				AmmoCounter.rotation = CurrentWeapon.AmmoPoolLocation.rotation;
			}
			else
			{
				AmmoCounter.position = Vector3.MoveTowards(AmmoCounter.position, AmmoCounterDefaultPosition.position, 0.01f);
				AmmoCounter.rotation = AmmoCounterDefaultPosition.rotation;
			}
		}
		else
		{
			AmmoCounter.gameObject.SetActive(value: false);
		}
		if (!IsFrozen && !IsHolding && !InWheel && !Player.isFrozen)
		{
			if ((KeyBindingManager.GetKey(KeyAction.Melee) || Input.GetButton("RightClick")) && canPunch && HasMelee && !BusyHands && !Player.Swimming)
			{
				StartCoroutine(Punch());
			}
			if (Input.GetKeyDown(KeyCode.Mouse0) && canPunch && HasMelee && !BusyHands && !Player.Swimming && CurrentWeapon == null && PrimaryWeapon == null && SecondaryWeapon == null)
			{
				StartCoroutine(Punch());
			}
			if ((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButton("Y")) && PrimaryWeapon != null && SecondaryWeapon != null && CanSwitch && !DualWielding)
			{
				StartCoroutine(SwitchCurrentWeapons());
			}
			if ((KeyBindingManager.GetKeyUp(KeyAction.Equipment) || Input.GetAxis("LeftTrigger") == 1f) && canUseEquipment && HasEquipment && !InWheel && !Player.Swimming)
			{
				StartCoroutine(UseEquipment());
			}
			if (KeyBindingManager.GetKey(KeyAction.Reload) || Input.GetButton("X"))
			{
				if (!BusyHands && !Player.Swimming)
				{
					if (GunConsumptionTimer > 0f)
					{
						GunConsumptionTimer -= Time.deltaTime;
						if (GunConsumptionTimer <= 1f)
						{
							ConsumptionCooldown.gameObject.SetActive(value: true);
							ConsumptionCooldown.fillAmount = GunConsumptionTimer;
						}
						else
						{
							ConsumptionCooldown.gameObject.SetActive(value: false);
							ConsumptionCooldown.fillAmount = 1f;
						}
					}
					else
					{
						ConsumptionCooldown.gameObject.SetActive(value: false);
						ConsumptionCooldown.fillAmount = 1f;
						GunConsumptionTimer = 1.5f;
						HoldAction();
					}
				}
			}
			else
			{
				GunConsumptionTimer = 1.5f;
				ConsumptionCooldown.gameObject.SetActive(value: false);
				ConsumptionCooldown.fillAmount = 1f;
			}
		}
		if (!CoolingDown)
		{
			Cooldown.fillAmount = 1f;
			Cooldown.gameObject.SetActive(value: false);
		}
		else
		{
			Cooldown.fillAmount -= Time.deltaTime / EquipmentCoolDown;
			Cooldown.gameObject.SetActive(value: true);
		}
		if (Player.Swimming && Player.TreadingWater)
		{
			if (!TriggeredSwim)
			{
				TriggeredSwim = true;
				Arms.SetTrigger("StartSwimming");
				Arms.SetBool("Swimming", value: true);
				foreach (WeaponItem weapon in Weapons)
				{
					if (weapon.WeaponObject.gameObject.activeSelf)
					{
						weapon.WeaponObject.LowerWeapon();
					}
				}
			}
		}
		else if (TriggeredSwim && FinishedTreading)
		{
			TriggeredSwim = false;
			Arms.SetBool("Swimming", value: false);
			foreach (WeaponItem weapon2 in Weapons)
			{
				if (weapon2.WeaponObject.gameObject.activeSelf)
				{
					weapon2.WeaponObject.RaiseWeapon();
				}
			}
		}
		if (Player.Swimming || SHT.Active)
		{
			if (Time.timeScale == 1f)
			{
				ColorGradingProcessing.temperature.value = -50f;
			}
		}
		else if (Time.timeScale == 1f)
		{
			ColorGradingProcessing.temperature.value = -15f;
		}
	}

	public void GrabWeapon(int ID)
	{
		if (ID != PrimaryID && ID != SecondaryID)
		{
			if (!GM.Ninja)
			{
				foreach (WeaponItem weapon in Weapons)
				{
					weapon.WeaponObject.AddWeaponToInventory(ID);
				}
			}
			else if (Weapons[ID].Name == "KATANA")
			{
				foreach (WeaponItem weapon2 in Weapons)
				{
					weapon2.WeaponObject.AddWeaponToInventory(ID);
				}
			}
		}
	}

	public void PickUpWeapon(int ID, GameObject WeaponObject)
	{
		if (ID != PrimaryID && ID != SecondaryID && !GM.Ninja)
		{
			foreach (WeaponItem weapon in Weapons)
			{
				weapon.WeaponObject.AddWeaponToInventory(ID);
				Object.Destroy(WeaponObject);
			}
		}
		else
		{
			WeaponObject.SendMessage("FailPickup");
		}
	}

	public void SwapWeaponOut(int ID)
	{
		foreach (WeaponItem weapon in Weapons)
		{
			weapon.WeaponObject.SwitchWweaponsInInventory(ID);
		}
	}

	public void TradeWeapon()
	{
		StartCoroutine(TradeWeaponAction());
	}

	public IEnumerator TradeWeaponAction()
	{
		BusyHands = true;
		canUseEquipment = false;
		Arms.SetTrigger("Pocket");
		PlaySound(3);
		foreach (WeaponItem weapon in Weapons)
		{
			if (weapon.WeaponObject.gameObject.activeSelf)
			{
				weapon.WeaponObject.LowerWeapon();
			}
		}
		yield return new WaitForSeconds(1f);
		canUseEquipment = true;
		BusyHands = false;
		foreach (WeaponItem weapon2 in Weapons)
		{
			if (weapon2.WeaponObject.gameObject.activeSelf)
			{
				weapon2.WeaponObject.RaiseWeapon();
			}
		}
	}

	public void RemoveCurrentWeapon()
	{
		CurrentWeapon.DisableMe();
		if (isPrimary)
		{
			if (SecondaryWeapon != null)
			{
				PrimaryWeapon = SecondaryWeapon;
				CurrentWeapon = PrimaryWeapon;
				PrimaryID = PrimaryWeapon.MyWeaponID;
				SecondaryID = -1;
				SecondaryWeapon = null;
				PrimaryWeapon.EnableMe();
			}
			else
			{
				CurrentWeapon = null;
				PrimaryWeapon = null;
				SecondaryWeapon = null;
				PrimaryID = -1;
				SecondaryID = -1;
			}
		}
		else
		{
			SecondaryWeapon = null;
			CurrentWeapon = PrimaryWeapon;
			PrimaryWeapon.EnableMe();
			isPrimary = true;
			PrimaryID = PrimaryWeapon.MyWeaponID;
			SecondaryID = -1;
		}
	}

	public IEnumerator SwitchCurrentWeapons()
	{
		CanSwitch = false;
		if (isPrimary)
		{
			PrimaryWeapon.DisableMe();
			SecondaryWeapon.EnableMe();
			CurrentWeapon = SecondaryWeapon;
			isPrimary = false;
		}
		else if (!isPrimary)
		{
			PrimaryWeapon.EnableMe();
			SecondaryWeapon.DisableMe();
			CurrentWeapon = PrimaryWeapon;
			isPrimary = true;
		}
		BumpWeapons();
		yield return new WaitForSeconds(1f);
		CanSwitch = true;
	}

	public void UpdateWeaponFireButtons()
	{
		foreach (WeaponItem weapon in Weapons)
		{
			weapon.WeaponObject.UpdateFireButtons();
		}
	}

	public void Vault()
	{
		StartCoroutine(VaultObject());
	}

	public void OnWall()
	{
		if (CanSwitch && CurrentWeapon != null && !CurrentWeapon.isReloading)
		{
			LoweredHand = true;
			CurrentWeapon.LeftHandOff();
		}
	}

	public void OffWall()
	{
		if (LoweredHand && CurrentWeapon != null)
		{
			LoweredHand = false;
			CurrentWeapon.LeftHandOn();
		}
	}

	public void TriggerDualWield()
	{
		if (CurrentWeapon != null && PrimaryWeapon != null && SecondaryWeapon != null)
		{
			if (PrimaryWeapon.CanDual && SecondaryWeapon.CanDual)
			{
				SetUpDualWield();
			}
		}
		else if (CurrentWeapon != null && CurrentWeapon.CanDual)
		{
			ClonedWeapon = Object.Instantiate(CurrentWeapon, CurrentWeapon.CenterMovement, CurrentWeapon.CenterRotation).GetComponent<WeaponScript>();
			ClonedWeapon.transform.parent = CurrentWeapon.transform.parent;
			ClonedWeapon.DisableMe();
			NewWeaponItem = new WeaponItem();
			NewWeaponItem.Name = Weapons[CurrentWeapon.MyWeaponID].Name;
			NewWeaponItem.WeaponObject = ClonedWeapon;
			NewWeaponItem.WeaponLevel = 1;
			Weapons.Add(NewWeaponItem);
			NewWeaponItem.SetMyID();
			GrabWeapon(NewWeaponItem.WeaponObject.MyWeaponID);
			SetUpDualWield();
		}
	}

	public void SetUpDualWield()
	{
		if (DualWielding)
		{
			DualWielding = false;
			if (isPrimary)
			{
				PrimaryWeapon.RemoveDualWield(RightHand: true);
				SecondaryWeapon.RemoveDualWield(RightHand: false);
			}
			else
			{
				PrimaryWeapon.RemoveDualWield(RightHand: false);
				SecondaryWeapon.RemoveDualWield(RightHand: true);
			}
		}
		else
		{
			DualWielding = true;
			if (isPrimary)
			{
				PrimaryWeapon.SetUpDualWield(RightHand: true);
				SecondaryWeapon.SetUpDualWield(RightHand: false);
			}
			else
			{
				PrimaryWeapon.SetUpDualWield(RightHand: false);
				SecondaryWeapon.SetUpDualWield(RightHand: true);
			}
		}
	}

	public IEnumerator Punch()
	{
		if (!Player.inAir)
		{
			Object.Instantiate(PunchObject, base.transform.position, base.transform.rotation);
			if (RightPunch)
			{
				Arms.SetTrigger("Melee 1");
				RightPunch = false;
			}
			else
			{
				Arms.SetTrigger("Melee 2");
				RightPunch = true;
			}
			GetComponent<Rigidbody>().AddForce(base.transform.forward * 150f);
			foreach (WeaponItem weapon in Weapons)
			{
				if (weapon.WeaponObject.gameObject.activeSelf)
				{
					weapon.WeaponObject.LowerWeapon();
				}
			}
			canPunch = false;
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(1f, 3f, 2f);
			CameraShaker.Instance.ShakeOnce(25f, 0.75f, 0.15f, 1.5f);
			CameraShaker.Instance.ResetCamera();
			yield return new WaitForSeconds(MeleeSpeed);
			foreach (WeaponItem weapon2 in Weapons)
			{
				if (weapon2.WeaponObject.gameObject.activeSelf)
				{
					weapon2.WeaponObject.RaiseWeapon();
				}
			}
			canPunch = true;
		}
		else
		{
			Object.Instantiate(KickObject, base.transform.position, base.transform.rotation);
			Legs.SetTrigger("Kick");
			if (GetComponent<Rigidbody>().velocity.magnitude <= 10f)
			{
				GetComponent<Rigidbody>().AddForce(base.transform.forward * 200f);
			}
			foreach (WeaponItem weapon3 in Weapons)
			{
				if (weapon3.WeaponObject.gameObject.activeSelf)
				{
					weapon3.WeaponObject.LowerWeapon();
				}
			}
			canPunch = false;
			CameraShaker.Instance.ShakeOnce(15f, 1f, 0.35f, 2f);
			yield return new WaitForSeconds(MeleeSpeed);
			foreach (WeaponItem weapon4 in Weapons)
			{
				if (weapon4.WeaponObject.gameObject.activeSelf)
				{
					weapon4.WeaponObject.RaiseWeapon();
				}
			}
			canPunch = true;
		}
	}

	public void OpenDoor()
	{
		StartCoroutine(Open());
	}

	public IEnumerator Open()
	{
		Arms.SetTrigger("OpenDoor");
		foreach (WeaponItem weapon in Weapons)
		{
			if (weapon.WeaponObject.gameObject.activeSelf)
			{
				weapon.WeaponObject.LowerWeapon();
			}
		}
		BusyHands = true;
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(20f, 2f, 0.35f, 1f);
		CameraShaker.Instance.ResetCamera();
		yield return new WaitForSeconds(0.75f);
		foreach (WeaponItem weapon2 in Weapons)
		{
			if (weapon2.WeaponObject.gameObject.activeSelf)
			{
				weapon2.WeaponObject.RaiseWeapon();
			}
		}
		BusyHands = false;
	}

	public void AquireEquipment(int EquipmentID)
	{
		foreach (EquipmentItem item in Equipment)
		{
			if (Equipment.IndexOf(item) == EquipmentID)
			{
				item.SetEquipment();
			}
		}
	}

	public void SetUpEquipment(GameObject Object, string ObjectPoint, string AnimationName, float ShakeMagnitude, float ActivationTime, float DeactivationTime, float CoolDown, AudioClip SoundEffect, int ItemID)
	{
		EquipmentObject = Object;
		EquipmentTransform = GameObject.FindGameObjectWithTag(ObjectPoint).transform;
		EquipmentAnimation = AnimationName;
		EquipmentShakeMagnitude = ShakeMagnitude;
		EquipmentActivationTime = ActivationTime;
		EquipmentDeactivationTime = DeactivationTime;
		EquipmentCoolDown = CoolDown;
		EquipmentSoundEffect = SoundEffect;
		EquipmentID = ItemID;
		if (DualWielding)
		{
			TriggerDualWield();
		}
		RefreshEquipmentCooldown();
	}

	public IEnumerator UseEquipment()
	{
		BusyHands = true;
		canUseEquipment = false;
		Arms.SetTrigger(EquipmentAnimation);
		Audio.clip = EquipmentSoundEffect;
		Audio.Play();
		GM.RandomStats[1].IncreaseStat();
		foreach (WeaponItem weapon in Weapons)
		{
			if (weapon.WeaponObject.gameObject.activeSelf)
			{
				weapon.WeaponObject.LowerWeapon();
			}
		}
		EquipmentTimer = 0.35f;
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(EquipmentShakeMagnitude, 2f, 0.25f, 4f);
		yield return new WaitForSeconds(EquipmentActivationTime);
		Object.Instantiate(EquipmentObject, EquipmentTransform.position, EquipmentTransform.rotation);
		CameraShaker.Instance.ShakeOnce(EquipmentShakeMagnitude, 2f, 0.25f, 1f);
		CameraShaker.Instance.ResetCamera();
		yield return new WaitForSeconds(EquipmentDeactivationTime);
		EquipmentTimer = 0.35f;
		foreach (WeaponItem weapon2 in Weapons)
		{
			if (weapon2.WeaponObject.gameObject.activeSelf)
			{
				weapon2.WeaponObject.RaiseWeapon();
			}
		}
		BusyHands = false;
		CoolingDown = true;
		yield return new WaitForSeconds(EquipmentCoolDown);
		CoolingDown = false;
		canUseEquipment = true;
		Audio.clip = AbilityAvailableEffect;
		Audio.Play();
	}

	public void RefreshEquipmentCooldown()
	{
		CoolingDown = false;
		HasEquipment = true;
		BusyHands = false;
		canUseEquipment = true;
	}

	public void GetEquipmentName(GameObject Asker, int EquipmentObject)
	{
		string value = "";
		int num = 0;
		foreach (EquipmentItem item in Equipment)
		{
			if (Equipment.IndexOf(item) == EquipmentObject)
			{
				value = item.Name;
				num = item.Price;
			}
		}
		Asker.SendMessage("SetName", value);
		Asker.SendMessage("SetPrice", num);
	}

	public IEnumerator VaultObject()
	{
		Arms.SetTrigger("Clamber");
		foreach (WeaponItem weapon in Weapons)
		{
			if (weapon.WeaponObject.gameObject.activeSelf)
			{
				weapon.WeaponObject.LowerWeapon();
				IsFrozen = true;
			}
		}
		BusyHands = true;
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(10f, 2f, 0.25f, 1f);
		CameraShaker.Instance.ResetCamera();
		yield return new WaitForSeconds(0.75f);
		foreach (WeaponItem weapon2 in Weapons)
		{
			if (weapon2.WeaponObject.gameObject.activeSelf)
			{
				weapon2.WeaponObject.RaiseWeapon();
				IsFrozen = false;
			}
		}
		BusyHands = false;
	}

	public void HoldAction()
	{
		if (PortalActive)
		{
			if (GameManager.GM.GetComponent<GTTODManager>().GameMode == 4)
			{
				Object.Instantiate(TeleportEffect);
				base.transform.position = new Vector3(124.4997f, 7.999512f, -35.30078f);
			}
			else
			{
				Object.Instantiate(TeleportEffect);
				base.transform.position = new Vector3(-1.300148f, 42.69946f, 546.0999f);
			}
		}
		else if (!DualWielding)
		{
			StartCoroutine(EatYourFuckingGunIGuessLolFuckIt());
		}
	}

	public void SetAllWeaponIDs()
	{
		foreach (WeaponItem weapon in Weapons)
		{
			weapon.SetMyID();
		}
	}

	public void Tread()
	{
		FinishedTreading = false;
	}

	public void EndTread()
	{
		FinishedTreading = true;
	}

	public IEnumerator EatYourFuckingGunIGuessLolFuckIt()
	{
		BusyHands = true;
		canUseEquipment = false;
		Arms.SetTrigger("Eat");
		PlaySound(3);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(10f, 2f, 0.25f, 1f);
		CameraShaker.Instance.ResetCamera();
		CurrentWeapon.LowerWeapon();
		yield return new WaitForSeconds(0.75f);
		CurrentWeapon.RaiseWeapon();
		CurrentWeapon.Consume();
		yield return new WaitForSeconds(0.4f);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(15f, 2f, 0.25f, 1f);
		CameraShaker.Instance.ResetCamera();
		Object.Instantiate(SwallowEffect, base.transform.position, base.transform.rotation);
		CurrentWeapon.DisableMe();
		AmmoTypes[CurrentWeapon.AmmoTypeIndex].AddAmmo(Weapons[CurrentWeapon.MyWeaponID].AmmoKillGrant * 5);
		GM.GetComponent<GTTODManager>().Points += Random.Range(100, 500);
		GM.GetComponent<GTTODManager>().CheckPointsUI();
		yield return new WaitForSeconds(0.75f);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(5f, 2f, 0.25f, 1f);
		CameraShaker.Instance.ResetCamera();
		yield return new WaitForSeconds(0.5f);
		RemoveCurrentWeapon();
		BusyHands = false;
		canUseEquipment = true;
	}

	public void PrintMessage(string MessageToPrint)
	{
		MessagePrint.Stop();
		MessagePrint.Play("PrintMessage");
		MessageText.text = MessageToPrint.ToString();
		PlaySound(0);
	}

	public void PrintFancyMessage(string MessageToPrint)
	{
		MessagePrint.Stop();
		MessagePrint.Play("PrintFancyMessage");
		FancyMessageText.text = MessageToPrint.ToString();
		PlaySound(1);
	}

	public void PrintTutorialMessage(string MessageToPrint)
	{
		MessagePrint.Stop();
		MessagePrint.Play("PrintTutorialMessage");
		TutorialText.text = MessageToPrint.ToString();
		PlaySound(0);
	}

	public void PrintObjective(string MessageToPrint)
	{
		MessagePrint.Stop();
		MessagePrint.Play("PrintObjective");
		TutorialText.text = MessageToPrint.ToString();
		PlaySound(2);
	}

	public void PrintAmmoAmount(int AmmoAmount, string AmmoType)
	{
		MessagePrint.Stop();
		MessagePrint.Play("PrintAmmo");
		AmmoText.text = "+ " + AmmoAmount.ToString() + " " + AmmoType.ToString();
	}

	public void PlaySound(int Index)
	{
		Audio.clip = MessageAudio[Index];
		Audio.Play();
	}

	public void DisableWeapons()
	{
		foreach (WeaponItem weapon in Weapons)
		{
			weapon.WeaponObject.gameObject.SetActive(value: false);
		}
	}

	public void EnableWeapon()
	{
		if (CurrentWeapon != null)
		{
			CurrentWeapon.EnableMe();
		}
	}

	public void FreezeWeapons()
	{
		if ((bool)CurrentWeapon)
		{
			CurrentWeapon.LowerWeapon();
			IsFrozen = true;
		}
	}

	public void UnFreezeWeapons()
	{
		if ((bool)CurrentWeapon)
		{
			CurrentWeapon.RaiseWeapon();
			IsFrozen = false;
		}
	}

	public void BumpWeapons()
	{
		if (!BusyHands)
		{
			foreach (WeaponItem weapon in Weapons)
			{
				if (weapon.WeaponObject.gameObject.activeSelf)
				{
					weapon.WeaponObject.BumpWeapon();
				}
			}
		}
	}

	public void DropAllWeapons()
	{
		if (DualWielding)
		{
			TriggerDualWield();
		}
		if (CurrentWeapon != null)
		{
			CurrentWeapon.DisableMe();
			CurrentWeapon = null;
		}
		if (PrimaryWeapon != null)
		{
			PrimaryWeapon.DisableMe();
			PrimaryWeapon = null;
		}
		if (SecondaryWeapon != null)
		{
			SecondaryWeapon.DisableMe();
			SecondaryWeapon = null;
		}
		foreach (AmmoType ammoType in AmmoTypes)
		{
			ammoType.ResetAmmo();
		}
		isPrimary = true;
		PrimaryID = -1;
		SecondaryID = -1;
		HasEquipment = false;
	}

	public void SetPrimaryID(int ID)
	{
		PrimaryID = ID;
	}

	public void SetSecondaryID(int ID)
	{
		SecondaryID = ID;
	}

	public void SetBulletRadius(float radius)
	{
		if (CurrentWeapon != null)
		{
			CurrentWeapon.BulletRadius = radius;
		}
	}

	public void RemoveItemFromInventoryList()
	{
		Weapons.RemoveAt(IndexOf);
	}

	public void AddItemToInvetoryList()
	{
		Weapons.Insert(IndexOf, new WeaponItem());
	}

	public void SetWeaponListNames()
	{
		foreach (WeaponItem weapon in Weapons)
		{
			weapon.SetWeaponName();
		}
		foreach (EquipmentItem item in Equipment)
		{
			item.SetEquipmentName();
		}
	}

	public void PortalActiveToggle(bool Active)
	{
		PortalActive = Active;
	}
}
