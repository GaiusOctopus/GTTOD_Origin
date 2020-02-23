using UnityEngine;

public class ZoomScript : MonoBehaviour
{
	public FOVManager FOVManagement;

	public Transform ScopedMuzzle;

	public GameObject Scope;

	public GameObject HUD;

	public float ZoomAmount = 35f;

	public bool MoveWeaponModel;

	[HideInInspector]
	public Vector3 ADSPosition;

	[HideInInspector]
	public Quaternion ADSRotation;

	private Transform MuzzleSave;

	private Transform WeaponHideTransform;

	private float ZoomFOV = 45f;

	private WeaponScript Weapon;

	private InventoryScript Inventory;

	private ac_CharacterController CharacterController;

	private MenuScript Menu;

	private bool hasLowered;

	private float SensitivitySave;

	private void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Menu = GameManager.GM.GetComponent<MenuScript>();
	}

	private void Awake()
	{
		ZoomFOV = FOVManagement.NormalFOV / 2f;
		Weapon = GetComponent<WeaponScript>();
		Inventory = Weapon.Inventory;
		WeaponHideTransform = Weapon.WeaponHideTransform;
		MuzzleSave = Weapon.MuzzleLocation;
	}

	private void Update()
	{
		if (Input.GetKey(Weapon.SwapKey) || Input.GetButton("RightBumper"))
		{
			if (!Weapon.isReloading && !Inventory.DualWielding)
			{
				FOVManagement.Zoom(ZoomAmount);
				Menu.CanPauseGame = false;
				if (MoveWeaponModel)
				{
					Weapon.ADSIn(ADSPosition, ADSRotation);
				}
				else
				{
					base.transform.position = new Vector3(WeaponHideTransform.transform.position.x, WeaponHideTransform.transform.position.y, WeaponHideTransform.transform.position.z);
					Weapon.MuzzleLocation = ScopedMuzzle.transform;
				}
				if (!hasLowered)
				{
					hasLowered = true;
					SensitivitySave = CharacterController.Sensitivity;
					CharacterController.Sensitivity /= 2.5f;
					Scope.SetActive(value: true);
					HUD.SetActive(value: false);
				}
				return;
			}
			FOVManagement.UnZoom();
			Scope.SetActive(value: false);
			if (hasLowered)
			{
				hasLowered = false;
				Menu.CanPauseGame = true;
				HUD.SetActive(value: true);
				CharacterController.Sensitivity = SensitivitySave;
				if (MoveWeaponModel)
				{
					Weapon.ADSOut();
					return;
				}
				Weapon.RaiseWeapon();
				Weapon.MuzzleLocation = MuzzleSave.transform;
			}
			return;
		}
		FOVManagement.UnZoom();
		Scope.SetActive(value: false);
		if (hasLowered)
		{
			hasLowered = false;
			Menu.CanPauseGame = true;
			HUD.SetActive(value: true);
			CharacterController.Sensitivity = SensitivitySave;
			if (MoveWeaponModel)
			{
				Weapon.ADSOut();
				return;
			}
			Weapon.RaiseWeapon();
			Weapon.MuzzleLocation = MuzzleSave.transform;
		}
	}

	private void OnDisable()
	{
		FOVManagement.QuickUnzoom();
		Scope.SetActive(value: false);
		if (hasLowered)
		{
			hasLowered = false;
			Menu.CanPauseGame = true;
			HUD.SetActive(value: true);
			CharacterController.Sensitivity = SensitivitySave;
			if (MoveWeaponModel)
			{
				Weapon.ADSOut();
				return;
			}
			Weapon.RaiseWeapon();
			Weapon.MuzzleLocation = MuzzleSave.transform;
		}
	}

	public void SetADSPosition()
	{
		ADSPosition = base.transform.position;
		ADSRotation = base.transform.rotation;
	}

	public void PlaceADSPosition()
	{
		base.transform.position = ADSPosition;
		base.transform.rotation = ADSRotation;
	}
}
