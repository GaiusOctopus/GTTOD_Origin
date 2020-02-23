using EZCameraShake;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponScript : MonoBehaviour
{
	[Header("Weapon Settings")]
	public bool ShowWeaponSettings;

	[ConditionalField("ShowWeaponSettings", null)]
	public int MaxAmmo;

	[ConditionalField("ShowWeaponSettings", null)]
	public float ReloadDuration = 1.5f;

	[ConditionalField("ShowWeaponSettings", null)]
	public string ShellPoolName = "NoShell";

	[ConditionalField("ShowWeaponSettings", null)]
	public string AmmoTypeName = "BasicAmmo";

	[ConditionalField("ShowWeaponSettings", null)]
	public GameObject ThrownWeapon;

	[Header("Position Settings")]
	public bool PositionSettings;

	[ConditionalField("PositionSettings", null)]
	public Vector3 CenterMovement;

	[ConditionalField("PositionSettings", null)]
	public Vector3 AlteredMovement;

	[ConditionalField("PositionSettings", null)]
	public Vector3 CrouchedPostion;

	[ConditionalField("PositionSettings", null)]
	public Vector3 CrouchedRotation;

	[ConditionalField("PositionSettings", null)]
	public Vector3 ConsumePosition;

	[ConditionalField("PositionSettings", null)]
	public Vector2 DualOffset;

	[Header("Movement Settings")]
	public bool ShowMovementSettings;

	[ConditionalField("ShowMovementSettings", null)]
	public float MovementSpeed = 5f;

	[ConditionalField("ShowMovementSettings", null)]
	public float SwayAmount;

	[ConditionalField("ShowMovementSettings", null)]
	public float MaxSway;

	[ConditionalField("ShowMovementSettings", null)]
	public float RecoilMagnitude = 2f;

	[ConditionalField("ShowMovementSettings", null)]
	public float RecoilRoughness = 10f;

	[ConditionalField("ShowMovementSettings", null)]
	public float RecoilFadeInTime = 5f;

	[ConditionalField("ShowMovementSettings", null)]
	public float RecoilFadeOutTime = 5f;

	[Header("Fire Mode Settings")]
	public bool ShowFireModeSettings;

	[ConditionalField("ShowFireModeSettings", null)]
	public bool PrimarySemi;

	[ConditionalField("ShowFireModeSettings", null)]
	public bool CustomPrimary;

	[ConditionalField("ShowFireModeSettings", null)]
	public float PrimaryRateOfFire = 0.1f;

	[ConditionalField("ShowFireModeSettings", null)]
	public int PrimaryBursts;

	[ConditionalField("ShowFireModeSettings", null)]
	public float PrimaryWait;

	[ConditionalField("ShowFireModeSettings", null)]
	public int PrimaryBulletConsumption = 1;

	[ConditionalField("ShowFireModeSettings", null)]
	public bool SecondarySemi;

	[ConditionalField("ShowFireModeSettings", null)]
	public bool CustomSecondary;

	[ConditionalField("ShowFireModeSettings", null)]
	public float SecondaryRateOfFire = 0.1f;

	[ConditionalField("ShowFireModeSettings", null)]
	public int SecondaryBursts;

	[ConditionalField("ShowFireModeSettings", null)]
	public float SecondaryWait;

	[ConditionalField("ShowFireModeSettings", null)]
	public int SecondaryBulletConsumption = 1;

	[Header("Bullet Settings")]
	public bool ShowBulletSettings;

	[ConditionalField("ShowBulletSettings", null)]
	public bool GenericPrimary;

	[ConditionalField("ShowBulletSettings", null)]
	public bool PrimaryChargeUp;

	[ConditionalField("ShowBulletSettings", null)]
	public GameObject PrimaryBullet;

	[ConditionalField("ShowBulletSettings", null)]
	public float PrimaryBulletDamage = 50f;

	[ConditionalField("ShowBulletSettings", null)]
	public float PrimaryBulletSpeed = 100f;

	[ConditionalField("ShowBulletSettings", null)]
	public float PrimaryBulletRange = 50f;

	[ConditionalField("ShowBulletSettings", null)]
	public bool GenericSecondary;

	[ConditionalField("ShowBulletSettings", null)]
	public bool SecondaryChargeUp;

	[ConditionalField("ShowBulletSettings", null)]
	public GameObject SecondaryBullet;

	[ConditionalField("ShowBulletSettings", null)]
	public float SecondaryBulletDamage = 50f;

	[ConditionalField("ShowBulletSettings", null)]
	public float SecondaryBulletSpeed = 100f;

	[ConditionalField("ShowBulletSettings", null)]
	public float SecondaryBulletRange = 50f;

	[Header("Weapon Set-Up")]
	public bool ShowWeaponSetUp;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool HasMag = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool HasMuzzleFlash = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool CanInterrupt;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool IsSegmentedReload;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool HasSecondary = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool CanDual = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool ToggleSwitch = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool HasReloadShake = true;

	[ConditionalField("ShowWeaponSetUp", null)]
	public bool EndlessAmmo;

	[ConditionalField("ShowWeaponSetUp", null)]
	public float ViewModelBump = 1f;

	[ConditionalField("ShowWeaponSetUp", null)]
	public float ChargeTime = 0.5f;

	[Header("Weapon Objects")]
	public bool ShowObjects;

	[ConditionalField("ShowObjects", null)]
	public ac_CharacterController Player;

	[ConditionalField("ShowObjects", null)]
	public InventoryScript Inventory;

	[ConditionalField("ShowObjects", null)]
	public ac_ObjectPool MyObjectPool;

	[ConditionalField("ShowObjects", null)]
	public Transform ShellLocation;

	[ConditionalField("ShowObjects", null)]
	public Transform MuzzleLocation;

	[ConditionalField("ShowObjects", null)]
	public Transform AmmoPoolLocation;

	[ConditionalField("ShowObjects", null)]
	public Transform WeaponHideTransform;

	[ConditionalField("ShowObjects", null)]
	public Transform WeaponCamera;

	[ConditionalField("ShowObjects", null)]
	public Transform AimFocus;

	[ConditionalField("ShowObjects", null)]
	public Animator IdleSway;

	[ConditionalField("ShowObjects", null)]
	public FOVManager FOVManager;

	[ConditionalField("ShowObjects", null)]
	public AudioClip WieldSFX;

	[ConditionalField("ShowObjects", null)]
	public AudioClip PrimaryFireSFX;

	[ConditionalField("ShowObjects", null)]
	public AudioClip SecondaryFireSFX;

	[ConditionalField("ShowObjects", null)]
	public AudioClip ReloadSFX;

	[ConditionalField("ShowObjects", null)]
	public AudioClip SecondaryReloadSFX;

	[ConditionalField("ShowObjects", null)]
	public AudioClip SwitchFireModeSFX;

	[Header("End Visible Variables")]
	private GameManager GM;

	[HideInInspector]
	public int MyWeaponID;

	[HideInInspector]
	public bool CanFire = true;

	[HideInInspector]
	public int CurrentAmmo;

	[HideInInspector]
	public int AmmoNeeded;

	[HideInInspector]
	public int AmmoTypeIndex;

	[HideInInspector]
	public float BulletRadius;

	[HideInInspector]
	public bool PrimaryOn = true;

	[HideInInspector]
	public bool CanReload;

	[HideInInspector]
	public float WeaponMovementSpeed;

	[HideInInspector]
	public bool isReloading;

	[HideInInspector]
	public CrosshairScript Crosshair;

	[HideInInspector]
	public KeyCode FireKey = KeyCode.Mouse0;

	[HideInInspector]
	public KeyCode SwapKey = KeyCode.Mouse1;

	[HideInInspector]
	public Quaternion ConsumeRotation;

	[HideInInspector]
	public Quaternion CenterRotation;

	[HideInInspector]
	public string CurrentFireAnimation;

	private Vector3 Direction;

	private Vector3 DirectionalCenter;

	private Quaternion TargetRotation;

	private WeaponFunctions WeaponFunctionality;

	private WeaponAudio LastShell;

	private AudioSource Audio;

	private AudioSource MagAudio;

	private AudioRange Echo;

	private bool HasDelayed;

	private bool IsCrouched;

	private bool CanMoveWeapon = true;

	private bool IsActive;

	private bool canSwitchFiremode = true;

	private bool isBumping;

	private bool canAnimate = true;

	private bool BootingUp;

	private bool DualWielding;

	private bool SemiDone = true;

	private bool ReloadedEnough;

	private bool EditorDual1 = true;

	private bool Consuming;

	private bool ForceMovement;

	private bool FireSaved;

	private bool AutoLoading;

	private bool MidBurst;

	private bool BurstWaiting;

	private bool ChargingUp;

	private float FallingMovement;

	private float SwaySave;

	private float CenterXSave;

	private float BaseYSave;

	private float MagAudioAmount;

	private float movementX;

	private float movementY;

	private float ForwardBack;

	private float LeftRight;

	private float RateOfFireCheck;

	private float BurstWaitCheck;

	private float ChargeTimeSave;

	private int BurstCountSave;

	private Transform WeaponParent;

	private Animator Anim;

	private BulletScript Bullet;

	private Rigidbody ShellVelocity;

	private GameObject Shell;

	private GameObject Flash;

	private Text AmmoText;

	private void Awake()
	{
		GM = GameManager.GM;
		WeaponParent = base.transform.parent;
		SwaySave = SwayAmount;
		Consuming = false;
		ForceMovement = false;
		CanMoveWeapon = true;
		Draw();
		MagAudio = WeaponHideTransform.GetComponent<AudioSource>();
		MagAudioAmount = 1 / MaxAmmo;
		Echo = MagAudio.gameObject.transform.GetChild(0).GetComponent<AudioRange>();
		WeaponFunctionality = GetComponent<WeaponFunctions>();
		WeaponMovementSpeed = MovementSpeed;
		Anim = GetComponent<Animator>();
		AmmoText = Inventory.CounterText;
		AmmoText.text = CurrentAmmo.ToString();
		Audio = GetComponent<AudioSource>();
		Audio.clip = WieldSFX;
		Audio.Play();
		ChargeTimeSave = ChargeTime;
		CenterRotation = base.transform.localRotation;
		ShellVelocity = Player.GetComponent<Rigidbody>();
		Crosshair = Player.gameObject.GetComponent<CrosshairScript>();
		CenterXSave = CenterMovement.x;
		BaseYSave = CenterRotation.y;
		foreach (AmmoType ammoType in Inventory.AmmoTypes)
		{
			if (ammoType.AmmoName == AmmoTypeName)
			{
				AmmoTypeIndex = Inventory.AmmoTypes.IndexOf(ammoType);
			}
		}
		UpdateFireButtons();
	}

	public void SetWeaponID(int ID)
	{
		MyWeaponID = ID;
	}

	private void Update()
	{
		if (IsActive && !Player.isFrozen)
		{
			MuzzleLocation.LookAt(AimFocus);
			UpdateInputs();
			UpdateRateOfFire();
		}
		UpdateSound();
		UpdateWeaponMovement();
		BumpUpdate();
	}

	public void AddWeaponToInventory(int ID)
	{
		if (MyWeaponID == ID)
		{
			CheckInventoryPlacement();
			Inventory.PrintMessage(Inventory.Weapons[MyWeaponID].Name + " ACQUIRED");
			Draw();
		}
		else if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void SwitchWweaponsInInventory(int ID)
	{
		if (MyWeaponID == ID)
		{
			Inventory.CurrentWeapon.DisableMe();
			Inventory.CurrentWeapon = null;
			Inventory.PrimaryWeapon = null;
			base.gameObject.SetActive(value: true);
			Inventory.PrimaryWeapon = this;
			Inventory.CurrentWeapon = this;
			Inventory.isPrimary = true;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void CheckInventoryPlacement()
	{
		CurrentAmmo = MaxAmmo;
		AmmoNeeded = 0;
		EnableMe();
		if (Inventory.PrimaryWeapon == null)
		{
			Inventory.CurrentWeapon = this;
			Inventory.PrimaryWeapon = this;
			Inventory.isPrimary = true;
			base.gameObject.SetActive(value: true);
			Inventory.SetPrimaryID(MyWeaponID);
		}
		else if (Inventory.PrimaryWeapon != null && Inventory.SecondaryWeapon == null)
		{
			Inventory.CurrentWeapon = this;
			Inventory.SecondaryWeapon = this;
			Inventory.isPrimary = false;
			base.gameObject.SetActive(value: true);
			Inventory.SetSecondaryID(MyWeaponID);
		}
		else if (Inventory.PrimaryWeapon != null && Inventory.SecondaryWeapon != null)
		{
			if (Inventory.isPrimary)
			{
				Inventory.PrimaryWeapon.DropWeapon();
				Inventory.CurrentWeapon = this;
				Inventory.PrimaryWeapon = this;
				Inventory.SetPrimaryID(MyWeaponID);
				base.gameObject.SetActive(value: true);
			}
			else if (!Inventory.isPrimary)
			{
				Inventory.SecondaryWeapon.DropWeapon();
				Inventory.CurrentWeapon = this;
				Inventory.SecondaryWeapon = this;
				Inventory.SetSecondaryID(MyWeaponID);
				base.gameObject.SetActive(value: true);
			}
		}
	}

	public void EnableMe()
	{
		base.gameObject.SetActive(value: true);
		RaiseWeapon();
		isReloading = false;
		CanReload = true;
		CanFire = true;
		HasDelayed = true;
		BootingUp = false;
		canAnimate = true;
		Consuming = false;
		ForceMovement = false;
		CanMoveWeapon = true;
		CheckAmmo();
		Draw();
	}

	public void DisableMe()
	{
		base.gameObject.SetActive(value: false);
		if (isReloading && ReloadedEnough)
		{
			CurrentAmmo = MaxAmmo;
			AmmoNeeded = 0;
			isReloading = false;
			ReloadedEnough = false;
		}
	}

	public void DropWeapon()
	{
		if (ThrownWeapon != null)
		{
			Object.Instantiate(ThrownWeapon, new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z + 1f), Player.transform.rotation);
		}
	}

	private void UpdateInputs()
	{
		if (Input.GetKey(FireKey) || (Input.GetAxis("RightTrigger") == 1f && !Player.KeyboardMouse))
		{
			if (PrimaryOn && CanFire && CurrentAmmo > 0 && SemiDone && !MidBurst && BurstCountSave <= 0)
			{
				if (!PrimaryChargeUp)
				{
					if (CanInterrupt)
					{
						base.gameObject.SetActive(value: false);
						base.gameObject.SetActive(value: true);
						isReloading = false;
						CanReload = true;
						CanFire = true;
						HasDelayed = true;
						BootingUp = false;
						canAnimate = true;
						NewPrimaryFire(PrimaryBursts);
					}
					else if (!isReloading)
					{
						NewPrimaryFire(PrimaryBursts);
					}
				}
				else
				{
					if (!ChargingUp)
					{
						ChargingUp = true;
						CanFire = false;
						CanReload = false;
						Audio.clip = SecondaryReloadSFX;
						Audio.Play();
						AnimateWeapon("ChargeUp");
						CameraShaker.Instance.ShakeOnce(2f, 1f, 0.35f, 2f);
					}
					ChargeTime -= Time.deltaTime;
					if (ChargeTime <= 0f)
					{
						NewPrimaryFire(PrimaryBursts);
						ChargeTime = ChargeTimeSave;
					}
				}
			}
			else if (CanFire && CurrentAmmo > 0 && !isReloading && SemiDone && !MidBurst && BurstCountSave <= 0)
			{
				if (!SecondaryChargeUp)
				{
					if (CanInterrupt)
					{
						NewSecondaryFire(SecondaryBursts);
						if (!isReloading)
						{
							isReloading = false;
							CanReload = true;
							StopCoroutine(Reload());
							StopCoroutine(SegmentedReload());
						}
					}
					else if (!isReloading)
					{
						NewSecondaryFire(SecondaryBursts);
					}
				}
				else
				{
					if (!ChargingUp)
					{
						ChargingUp = true;
						CanFire = false;
						CanReload = false;
						Audio.clip = SecondaryReloadSFX;
						Audio.Play();
						AnimateWeapon("ChargeUp");
						CameraShaker.Instance.ShakeOnce(2f, 1f, 0.35f, 2f);
					}
					ChargeTime -= Time.deltaTime;
					if (ChargeTime <= 0f)
					{
						NewSecondaryFire(SecondaryBursts);
						ChargeTime = ChargeTimeSave;
					}
				}
			}
		}
		else
		{
			if (PrimarySemi)
			{
				SemiDone = true;
			}
			if (SecondarySemi)
			{
				SemiDone = true;
			}
			if (PrimaryChargeUp && ChargingUp)
			{
				ChargingUp = false;
				AnimateWeapon("Return");
				ChargeTime = ChargeTimeSave;
			}
		}
		if (Input.GetKey(SwapKey) || (Input.GetButton("RightBumper") && !Player.KeyboardMouse))
		{
			if (canSwitchFiremode && HasSecondary && !DualWielding)
			{
				canSwitchFiremode = false;
				if (PrimaryOn)
				{
					PrimaryOn = false;
					BurstCountSave = 0;
					BurstWaiting = false;
					BurstWaitCheck = 0f;
				}
				else
				{
					PrimaryOn = true;
					BurstCountSave = 0;
					BurstWaiting = false;
					BurstWaitCheck = 0f;
				}
				if (!isReloading)
				{
					Audio.clip = SwitchFireModeSFX;
					Audio.Play();
				}
			}
		}
		else if (!ToggleSwitch && !canSwitchFiremode)
		{
			if (PrimaryOn)
			{
				PrimaryOn = false;
			}
			else
			{
				PrimaryOn = true;
			}
			if (!isReloading && !Audio.isPlaying)
			{
				Audio.clip = SwitchFireModeSFX;
				Audio.Play();
			}
			canSwitchFiremode = true;
		}
		else
		{
			canSwitchFiremode = true;
		}
		if ((KeyBindingManager.GetKey(KeyAction.Reload) || (Input.GetButton("X") && !Player.KeyboardMouse)) && CanReload && !isReloading && !AutoLoading && CurrentAmmo != MaxAmmo && ReloadDuration != 0f && Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount > 0)
		{
			if (!IsSegmentedReload)
			{
				StartCoroutine(Reload());
			}
			else if (CanFire)
			{
				AnimateWeapon("Reload");
				Audio.clip = WieldSFX;
				Audio.Play();
				StartCoroutine(SegmentedReload());
			}
		}
	}

	public void UpdateRateOfFire()
	{
		if (PrimaryOn)
		{
			if (RateOfFireCheck <= 0f)
			{
				if (PrimaryBursts == 0)
				{
					CanFire = true;
					MidBurst = false;
				}
				else if (PrimaryBursts != 0 && CurrentAmmo != 0)
				{
					if (BurstCountSave > 0 && CurrentAmmo != 0)
					{
						NewPrimaryFire(BurstCountSave);
						MidBurst = true;
					}
					else if (BurstCountSave <= 0 && !BurstWaiting)
					{
						BurstWaitCheck = PrimaryWait;
						BurstWaiting = true;
					}
				}
			}
			else
			{
				RateOfFireCheck -= Time.deltaTime;
			}
		}
		else if (RateOfFireCheck <= 0f)
		{
			if (SecondaryBursts == 0)
			{
				CanFire = true;
				MidBurst = false;
			}
			else if (SecondaryBursts != 0 && CurrentAmmo != 0)
			{
				if (BurstCountSave > 0 && CurrentAmmo != 0)
				{
					NewSecondaryFire(BurstCountSave);
					MidBurst = true;
				}
				else if (BurstCountSave <= 0 && !BurstWaiting)
				{
					BurstWaitCheck = SecondaryWait;
					BurstWaiting = true;
				}
			}
		}
		else
		{
			RateOfFireCheck -= Time.deltaTime;
		}
		if (BurstWaitCheck <= 0f && BurstWaiting)
		{
			CanFire = true;
			CanReload = true;
			MidBurst = false;
			BurstWaiting = false;
		}
		else
		{
			BurstWaitCheck -= Time.deltaTime;
		}
	}

	public void NewPrimaryFire(int BurstCount)
	{
		if (CustomPrimary)
		{
			base.gameObject.SendMessage("CustomFire");
		}
		CanFire = false;
		CanReload = false;
		CurrentAmmo -= PrimaryBulletConsumption;
		AmmoNeeded += PrimaryBulletConsumption;
		BurstCountSave = BurstCount;
		BurstCountSave--;
		MidBurst = true;
		CrosshairFire();
		WeaponFireRotate();
		CheckAmmo();
		if (!ForceMovement)
		{
			AnimateWeapon(CurrentFireAnimation);
		}
		Bullet = Object.Instantiate(PrimaryBullet, MuzzleLocation.position, MuzzleLocation.rotation).GetComponent<BulletScript>();
		MuzzleFlash();
		if (GenericPrimary)
		{
			Bullet.damage = PrimaryBulletDamage;
			Bullet.speed = PrimaryBulletSpeed;
			Bullet.EffectiveRange = PrimaryBulletRange;
			Bullet.BulletAssist = BulletRadius;
		}
		if (PrimarySemi)
		{
			SemiDone = false;
		}
		if (!IsSegmentedReload)
		{
			EjectShell();
		}
		else
		{
			Audio.clip = PrimaryFireSFX;
			Audio.Play();
			StartCoroutine(ShellDelay());
		}
		CameraShaker.Instance.ShakeOnce(RecoilMagnitude, RecoilRoughness, RecoilFadeInTime, RecoilFadeOutTime);
		FOVManager.BumpFOV(1f);
		RateOfFireCheck = PrimaryRateOfFire;
	}

	public void NewSecondaryFire(int BurstCount)
	{
		if (CustomSecondary)
		{
			base.gameObject.SendMessage("CustomFire");
		}
		CanFire = false;
		CurrentAmmo -= SecondaryBulletConsumption;
		AmmoNeeded += SecondaryBulletConsumption;
		BurstCountSave = BurstCount;
		BurstCountSave--;
		MidBurst = true;
		CrosshairFire();
		WeaponFireRotate();
		CheckAmmo();
		if (!ForceMovement)
		{
			AnimateWeapon(CurrentFireAnimation);
		}
		Bullet = Object.Instantiate(SecondaryBullet, MuzzleLocation.position, MuzzleLocation.rotation).GetComponent<BulletScript>();
		MuzzleFlash();
		if (GenericSecondary)
		{
			Bullet.damage = SecondaryBulletDamage;
			Bullet.speed = SecondaryBulletSpeed;
			Bullet.EffectiveRange = SecondaryBulletRange;
			Bullet.BulletAssist = BulletRadius;
		}
		if (SecondarySemi)
		{
			SemiDone = false;
		}
		if (!IsSegmentedReload)
		{
			EjectShell();
		}
		else
		{
			Audio.clip = PrimaryFireSFX;
			Audio.Play();
			StartCoroutine(ShellDelay());
		}
		CameraShaker.Instance.ShakeOnce(RecoilMagnitude, RecoilRoughness, 0f, RecoilFadeOutTime);
		FOVManager.BumpFOV(1f);
		RateOfFireCheck = SecondaryRateOfFire;
	}

	private IEnumerator ChargeUpWait()
	{
		CanFire = false;
		CanReload = false;
		Audio.clip = SecondaryReloadSFX;
		Audio.Play();
		AnimateWeapon("ChargeUp");
		CameraShaker.Instance.ShakeOnce(2f, 1f, 0.35f, 2f);
		yield return new WaitForSeconds(0.5f);
		if (PrimaryOn)
		{
			NewPrimaryFire(PrimaryBursts);
		}
		else
		{
			NewSecondaryFire(SecondaryBursts);
		}
		ChargingUp = false;
	}

	private IEnumerator Reload()
	{
		AmmoText.gameObject.SetActive(value: false);
		BurstCountSave = 0;
		BurstWaiting = false;
		BurstWaitCheck = 0f;
		AmmoText.text = "0";
		AmmoText.color = Color.red;
		if (WeaponFunctionality.PhysicsMag)
		{
			WeaponFunctionality.Reload();
		}
		if (DualWielding)
		{
			base.transform.position = new Vector3(WeaponHideTransform.transform.position.x, WeaponHideTransform.transform.position.y, WeaponHideTransform.transform.position.z);
			CanMoveWeapon = false;
			IsActive = false;
			CanReload = false;
			isReloading = true;
			CanFire = false;
			Audio.clip = ReloadSFX;
			Audio.Play();
			StartCoroutine(ReloadShake());
			yield return new WaitForSeconds(ReloadDuration);
			if (!Inventory.UnlimitedAmmo && !EndlessAmmo)
			{
				if (Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount >= AmmoNeeded)
				{
					CurrentAmmo = MaxAmmo;
					Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount -= AmmoNeeded;
					AmmoNeeded = 0;
				}
				if (Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount < AmmoNeeded)
				{
					CurrentAmmo += Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount;
					Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount = 0;
					AmmoNeeded = 0;
				}
			}
			else
			{
				CurrentAmmo = MaxAmmo;
			}
			canAnimate = true;
			CanReload = true;
			isReloading = false;
			CanFire = true;
			CheckAmmo();
			CanMoveWeapon = true;
			IsActive = true;
			yield break;
		}
		CanReload = false;
		isReloading = true;
		CanFire = false;
		Audio.clip = ReloadSFX;
		Audio.Play();
		AnimateWeapon("Reload");
		canAnimate = false;
		StartCoroutine(ReloadShake());
		yield return new WaitForSeconds(ReloadDuration);
		if (!Inventory.UnlimitedAmmo && !EndlessAmmo)
		{
			if (Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount >= AmmoNeeded)
			{
				CurrentAmmo = MaxAmmo;
				Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount -= AmmoNeeded;
				AmmoNeeded = 0;
			}
			if (Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount < AmmoNeeded)
			{
				CurrentAmmo += Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount;
				Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount = 0;
				AmmoNeeded = 0;
			}
		}
		else
		{
			CurrentAmmo = MaxAmmo;
		}
		canAnimate = true;
		CanReload = true;
		isReloading = false;
		CanFire = true;
		CheckAmmo();
	}

	private IEnumerator ReloadShake()
	{
		if (HasReloadShake)
		{
			ReloadedEnough = false;
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(8f, 1.5f, 0.4f, 1.5f);
			yield return new WaitForSeconds(ReloadDuration / 4f);
			CameraShaker.Instance.ShakeOnce(8f, 1.5f, 0.4f, 1.5f);
			yield return new WaitForSeconds(ReloadDuration / 4f);
			ReloadedEnough = true;
			CameraShaker.Instance.ShakeOnce(8f, 1.5f, 0.4f, 1.5f);
			yield return new WaitForSeconds(ReloadDuration / 4f);
			CameraShaker.Instance.ShakeOnce(8f, 2.25f, 0.35f, 1f);
			CameraShaker.Instance.ResetCamera();
			ReloadedEnough = false;
		}
	}

	private IEnumerator SegmentedReload()
	{
		if (!isReloading)
		{
			CanReload = false;
			isReloading = true;
			AmmoText.gameObject.SetActive(value: false);
			yield return new WaitForSeconds(0.225f);
			AnimateWeapon("Insert");
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(2f, 1.5f, 0.4f, 2.5f);
			CameraShaker.Instance.ResetCamera();
			Audio.clip = ReloadSFX;
			Audio.Play();
			yield return new WaitForSeconds(0.225f);
			CurrentAmmo++;
			if (!Inventory.UnlimitedAmmo && !EndlessAmmo)
			{
				Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount--;
			}
			CheckAmmo();
			yield return new WaitForSeconds(0.225f);
			if (CurrentAmmo != MaxAmmo && Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount > 0)
			{
				StartCoroutine(SegmentedReload());
				yield break;
			}
			AnimateWeapon("Return");
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(8f, 1.35f, 0.5f, 2f);
			CameraShaker.Instance.ResetCamera();
			Audio.clip = SecondaryReloadSFX;
			Audio.Play();
			yield return new WaitForSeconds(0.35f);
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(8f, 1.35f, 0.5f, 2f);
			CameraShaker.Instance.ResetCamera();
			yield return new WaitForSeconds(0.65f);
			isReloading = false;
		}
		else
		{
			CanReload = false;
			isReloading = true;
			AmmoText.gameObject.SetActive(value: false);
			AnimateWeapon("Insert");
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(2f, 1.5f, 0.4f, 2.5f);
			CameraShaker.Instance.ResetCamera();
			Audio.clip = ReloadSFX;
			Audio.Play();
			yield return new WaitForSeconds(0.225f);
			CurrentAmmo++;
			if (!Inventory.UnlimitedAmmo && !EndlessAmmo)
			{
				Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount--;
			}
			CheckAmmo();
			yield return new WaitForSeconds(0.225f);
			if (CurrentAmmo != MaxAmmo && Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount > 0)
			{
				StartCoroutine(SegmentedReload());
				yield break;
			}
			AnimateWeapon("Return");
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(8f, 1.35f, 0.5f, 2f);
			CameraShaker.Instance.ResetCamera();
			Audio.clip = SecondaryReloadSFX;
			Audio.Play();
			yield return new WaitForSeconds(0.35f);
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(8f, 1.35f, 0.5f, 2f);
			CameraShaker.Instance.ResetCamera();
			yield return new WaitForSeconds(0.65f);
			isReloading = false;
		}
	}

	public void UpdateWeaponMovement()
	{
		if (CanMoveWeapon)
		{
			if (Player.KeyboardMouse)
			{
				movementX = (0f - Input.GetAxis("Mouse X")) * SwayAmount;
				movementY = (0f - Input.GetAxis("Mouse Y")) * SwayAmount;
				if (KeyBindingManager.GetKey(KeyAction.Left) || KeyBindingManager.GetKey(KeyAction.Right))
				{
					if (KeyBindingManager.GetKey(KeyAction.Left) && KeyBindingManager.GetKey(KeyAction.Right))
					{
						LeftRight = 0f;
					}
					else
					{
						if (KeyBindingManager.GetKey(KeyAction.Right))
						{
							LeftRight = -1f;
						}
						if (KeyBindingManager.GetKey(KeyAction.Left))
						{
							LeftRight = 1f;
						}
					}
				}
				else
				{
					LeftRight = 0f;
				}
				if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward))
				{
					if (KeyBindingManager.GetKey(KeyAction.Forward) && KeyBindingManager.GetKey(KeyAction.Backward))
					{
						ForwardBack = 0f;
					}
					else
					{
						if (KeyBindingManager.GetKey(KeyAction.Forward))
						{
							ForwardBack = 0.5f;
						}
						if (KeyBindingManager.GetKey(KeyAction.Backward))
						{
							ForwardBack = -0.5f;
						}
					}
				}
				else
				{
					ForwardBack = 0f;
				}
			}
			else
			{
				movementX = (0f - Input.GetAxis("Controller X")) * SwayAmount;
				movementY = (0f - Input.GetAxis("Controller Y")) * SwayAmount;
				ForwardBack = Input.GetAxis("Vertical") * -0.5f;
				LeftRight = Input.GetAxis("Horizontal") * -0.5f;
			}
			movementX = Mathf.Clamp(movementX, 0f - MaxSway, MaxSway);
			movementY = Mathf.Clamp(movementY, 0f - MaxSway, MaxSway);
			Vector3 a = new Vector3(movementX, movementY, 0f);
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, a + DirectionalCenter, Time.deltaTime * WeaponMovementSpeed);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, TargetRotation, Time.deltaTime * WeaponMovementSpeed);
			if (!IsCrouched)
			{
				TargetRotation = CenterRotation;
				DirectionalCenter = new Vector3(CenterMovement.x + AlteredMovement.x * LeftRight, DirectionalCenter.y, CenterMovement.z + AlteredMovement.z * ForwardBack);
				if (Player.isFalling)
				{
					FallingMovement = Mathf.Lerp(DirectionalCenter.y, AlteredMovement.y, 0.0035f);
					DirectionalCenter = new Vector3(DirectionalCenter.x, FallingMovement, DirectionalCenter.z);
				}
				else
				{
					FallingMovement = CenterMovement.y;
					DirectionalCenter = new Vector3(DirectionalCenter.x, CenterMovement.y, DirectionalCenter.z);
				}
			}
			else if (IsCrouched)
			{
				TargetRotation = Quaternion.Euler(CrouchedRotation.x, CrouchedRotation.y, CrouchedRotation.z);
				DirectionalCenter = new Vector3(CrouchedPostion.x, CrouchedPostion.y, CrouchedPostion.z);
			}
		}
		if (ForceMovement)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, DirectionalCenter, Time.deltaTime * 20f);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, TargetRotation, Time.deltaTime * 20f);
		}
	}

	public void LowerWeapon()
	{
		base.transform.position = new Vector3(WeaponHideTransform.transform.position.x, WeaponHideTransform.transform.position.y, WeaponHideTransform.transform.position.z);
		WeaponMovementSpeed = 0f;
		IsActive = false;
	}

	public void RaiseWeapon()
	{
		WeaponMovementSpeed = MovementSpeed;
		IsActive = true;
	}

	public void BumpWeapon()
	{
		StartCoroutine(BumpWait());
	}

	public IEnumerator BumpWait()
	{
		isBumping = true;
		yield return new WaitForSeconds(0.1f);
		isBumping = false;
	}

	public void BumpUpdate()
	{
		if (isBumping)
		{
			float y = Mathf.Lerp(CenterMovement.y, CenterMovement.y - 0.2f, 0.5f);
			DirectionalCenter = new Vector3(DirectionalCenter.x, y, DirectionalCenter.z);
		}
		if (Player.isCrouching)
		{
			if (!IsCrouched)
			{
				IsCrouched = true;
				WeaponMovementSpeed = 10f;
			}
		}
		else if (IsCrouched)
		{
			IsCrouched = false;
			WeaponMovementSpeed = MovementSpeed;
		}
	}

	public void WeaponFireRotate()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z - 0.1f * ViewModelBump);
	}

	public void SetUpDualWield(bool RightHand)
	{
		if (RightHand)
		{
			base.gameObject.SetActive(value: true);
			CenterXSave = CenterMovement.x;
			CenterMovement = new Vector3(DualOffset.x, CenterMovement.y, CenterMovement.z);
			BaseYSave = CenterRotation.y;
			CenterRotation = new Quaternion(CenterRotation.x, 0f, CenterRotation.z, CenterRotation.w);
			Draw();
			CanMoveWeapon = true;
			DualWielding = true;
			PrimaryOn = true;
			FireKey = (KeyCode)PlayerPrefs.GetInt(KeyAction.SwitchFireMode.ToString());
			Anim.SetTrigger("LowerHand");
			ChangeFireAnimation("FireOneHand");
		}
		else
		{
			base.gameObject.SetActive(value: true);
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, base.transform.localScale.y, base.transform.localScale.z);
			AmmoText.rectTransform.localScale = new Vector3(AmmoText.rectTransform.localScale.x * -1f, AmmoText.rectTransform.localScale.y, AmmoText.rectTransform.localScale.z);
			CenterXSave = CenterMovement.x;
			CenterMovement = new Vector3(DualOffset.y, CenterMovement.y, CenterMovement.z);
			BaseYSave = CenterRotation.y;
			CenterRotation = new Quaternion(CenterRotation.x, 0f, CenterRotation.z, CenterRotation.w);
			Draw();
			CanMoveWeapon = true;
			DualWielding = true;
			PrimaryOn = true;
			FireKey = (KeyCode)PlayerPrefs.GetInt(KeyAction.Fire.ToString());
			Anim.SetTrigger("LowerHand");
			ChangeFireAnimation("FireOneHand");
		}
	}

	public void RemoveDualWield(bool RightHand)
	{
		if (RightHand)
		{
			base.gameObject.SetActive(value: true);
			CenterMovement = new Vector3(CenterXSave, CenterMovement.y, CenterMovement.z);
			CenterRotation = new Quaternion(CenterRotation.x, BaseYSave, CenterRotation.z, CenterRotation.w);
			Draw();
			CanMoveWeapon = true;
			DualWielding = false;
			PrimaryOn = true;
			UpdateFireButtons();
			Anim.SetTrigger("RaiseHand");
			ChangeFireAnimation("Fire");
		}
		else
		{
			base.gameObject.SetActive(value: false);
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, base.transform.localScale.y, base.transform.localScale.z);
			AmmoText.rectTransform.localScale = new Vector3(AmmoText.rectTransform.localScale.x * -1f, AmmoText.rectTransform.localScale.y, AmmoText.rectTransform.localScale.z);
			CenterMovement = new Vector3(CenterXSave, CenterMovement.y, CenterMovement.z);
			CenterRotation = new Quaternion(CenterRotation.x, BaseYSave, CenterRotation.z, CenterRotation.w);
			CanMoveWeapon = true;
			DualWielding = false;
			PrimaryOn = true;
			UpdateFireButtons();
			Anim.SetTrigger("RaiseHand");
			ChangeFireAnimation("Fire");
		}
	}

	public void LeftHandOff()
	{
		if (!DualWielding)
		{
			if (!isReloading)
			{
				Anim.SetTrigger("LowerHand");
			}
			ChangeFireAnimation("FireOneHand");
		}
	}

	public void LeftHandOn()
	{
		if (!DualWielding)
		{
			if (!isReloading)
			{
				Anim.SetTrigger("RaiseHand");
			}
			ChangeFireAnimation("Fire");
		}
	}

	public void Draw()
	{
		Audio = GetComponent<AudioSource>();
		Audio.clip = WieldSFX;
		Audio.Play();
		base.transform.position = new Vector3(WeaponHideTransform.position.x, WeaponHideTransform.position.y, WeaponHideTransform.position.z);
		IsActive = true;
		CanMoveWeapon = true;
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(8f, 1.25f, 0.25f, 1.35f);
		CameraShaker.Instance.ResetCamera();
		if (DualWielding)
		{
			ChangeFireAnimation("FireOneHand");
		}
		else
		{
			ChangeFireAnimation("Fire");
		}
	}

	public void Consume()
	{
		IsActive = false;
		DirectionalCenter = ConsumePosition;
		TargetRotation = ConsumeRotation;
		CanMoveWeapon = false;
		ForceMovement = true;
		Consuming = true;
	}

	public void ADSIn(Vector3 ADSPosition, Quaternion ADSRotation)
	{
		CanMoveWeapon = false;
		ForceMovement = true;
		DirectionalCenter = ADSPosition;
		TargetRotation = ADSRotation;
		IdleSway.speed = 0f;
		IdleSway.transform.localPosition = Vector3.zero;
		IdleSway.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	public void ADSOut()
	{
		CanMoveWeapon = true;
		ForceMovement = false;
		DirectionalCenter = CenterMovement;
		TargetRotation = CenterRotation;
	}

	public void CheckAmmo()
	{
		AmmoText.text = CurrentAmmo.ToString();
		if (CurrentAmmo != MaxAmmo)
		{
			CanReload = true;
		}
		if (CurrentAmmo <= 0 && base.isActiveAndEnabled && Inventory.AmmoTypes[AmmoTypeIndex].AmmoAmount > 0)
		{
			CurrentAmmo = 0;
			AmmoText.text = CurrentAmmo.ToString();
			StartCoroutine(AutoReload());
		}
		if (!isReloading && !EndlessAmmo)
		{
			if ((float)CurrentAmmo > (float)MaxAmmo * 0.5f)
			{
				AmmoText.color = Color.clear;
				AmmoText.gameObject.SetActive(value: true);
			}
			if ((float)CurrentAmmo < (float)MaxAmmo * 0.35f && (float)CurrentAmmo > (float)MaxAmmo * 0.15f)
			{
				AmmoText.color = Color.yellow;
			}
			if ((float)CurrentAmmo < (float)MaxAmmo * 0.15f)
			{
				AmmoText.color = Color.red;
			}
		}
	}

	public IEnumerator AutoReload()
	{
		AutoLoading = true;
		if (PrimaryOn)
		{
			yield return new WaitForSeconds(PrimaryRateOfFire);
		}
		else
		{
			yield return new WaitForSeconds(SecondaryRateOfFire);
		}
		if (!IsSegmentedReload && !isReloading)
		{
			StartCoroutine(Reload());
			AutoLoading = false;
			yield break;
		}
		yield return new WaitForSeconds(0.5f);
		AnimateWeapon("Reload");
		Audio.clip = WieldSFX;
		Audio.Play();
		if (!isReloading)
		{
			StartCoroutine(SegmentedReload());
			AutoLoading = false;
		}
	}

	public IEnumerator ShellDelay()
	{
		yield return new WaitForSeconds(0.45f);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 1f, 1f);
		CameraShaker.Instance.ShakeOnce(6f, 1.25f, 0.15f, 1f);
		CameraShaker.Instance.ResetCamera();
		CameraShaker.Instance.ResetCamera();
		EjectShell();
	}

	public void EjectShell()
	{
		MagAudioAmount = 1f / ((float)CurrentAmmo / 5f);
		MagAudio.volume = MagAudioAmount;
		if (!IsSegmentedReload)
		{
			if (HasMag)
			{
				MagAudio.Play();
				Echo.PlayAudio();
			}
			if (PrimaryOn)
			{
				Shell = MyObjectPool.GetObject(ShellPoolName);
				Shell.SetActive(value: true);
				Shell.GetComponent<WeaponAudio>().Fire(PrimaryFireSFX, ShellLocation, ShellVelocity.velocity);
			}
			else
			{
				Shell = MyObjectPool.GetObject(ShellPoolName);
				Shell.SetActive(value: true);
				Shell.GetComponent<WeaponAudio>().Fire(SecondaryFireSFX, ShellLocation, ShellVelocity.velocity);
			}
		}
		else
		{
			Shell = MyObjectPool.GetObject(ShellPoolName);
			Shell.SetActive(value: true);
			Shell.GetComponent<WeaponAudio>().Fire(SwitchFireModeSFX, ShellLocation, ShellVelocity.velocity);
		}
	}

	public void MuzzleFlash()
	{
		if (HasMuzzleFlash)
		{
			Flash = MyObjectPool.GetObject("MuzzleFlash");
			Flash.SetActive(value: true);
			Flash.transform.position = MuzzleLocation.position;
			Flash.transform.rotation = MuzzleLocation.rotation;
			Flash.GetComponent<ParticleSystem>().Play();
		}
	}

	public void AnimateWeapon(string Name)
	{
		if (canAnimate)
		{
			Anim.SetTrigger(Name);
		}
	}

	public void ChangeFireAnimation(string Name)
	{
		if (!IsSegmentedReload)
		{
			CurrentFireAnimation = Name;
		}
		else
		{
			CurrentFireAnimation = "Fire";
		}
	}

	public void CrosshairFire()
	{
		Crosshair.Fire(0.35f);
	}

	private void UpdateSound()
	{
		if (Time.timeScale != 1f)
		{
			Audio.pitch = Time.timeScale;
		}
		else
		{
			Audio.pitch = 1f;
		}
	}

	public void UpdateFireButtons()
	{
		FireKey = (KeyCode)PlayerPrefs.GetInt(KeyAction.Fire.ToString());
		SwapKey = (KeyCode)PlayerPrefs.GetInt(KeyAction.SwitchFireMode.ToString());
	}

	public void SetBaseView()
	{
		base.transform.position = CenterMovement;
		base.transform.rotation = CenterRotation;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void SetCrouchView()
	{
		base.transform.position = CrouchedPostion;
		base.transform.rotation = Quaternion.Euler(CrouchedRotation.x, CrouchedRotation.y, CrouchedRotation.z);
	}

	public void SetDualView()
	{
		if (EditorDual1)
		{
			EditorDual1 = false;
			base.transform.position = new Vector3(DualOffset.x, CenterMovement.y, CenterMovement.z);
			base.transform.rotation = new Quaternion(CenterRotation.x, 0f, CenterRotation.z, CenterRotation.w);
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			EditorDual1 = true;
			base.transform.position = new Vector3(DualOffset.y, CenterMovement.y, CenterMovement.z);
			base.transform.rotation = new Quaternion(CenterRotation.x, 0f, CenterRotation.z, CenterRotation.w);
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public void SetConsumeView()
	{
		base.transform.position = ConsumePosition;
		base.transform.rotation = ConsumeRotation;
	}

	public void ApplyBaseView()
	{
		CenterMovement = base.transform.position;
		CenterRotation = base.transform.rotation;
	}

	public void ApplyConsumeView()
	{
		ConsumePosition = base.transform.position;
		ConsumeRotation = base.transform.rotation;
	}
}
