using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class WeaponWheel : MonoBehaviour
{
	public bool Active;

	public PostProcessVolume PostProcessor;

	public GameObject WeaponWheelObject;

	public RectTransform WheelRotator;

	public Text WeaponName;

	public Text AmmoAmount;

	private GameManager GM;

	private MenuScript Menu;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private TimeSlow TimeManager;

	private Animation Anim;

	private PopUpManager PopUp;

	private float TimeToOpen = 0.5f;

	private bool WheelOpen;

	private bool WheelCooldown;

	[Header("POST PROCESSING")]
	private DepthOfField DepthOfFieldProcessing;

	private void Start()
	{
		GM = GameManager.GM;
		Menu = GM.GetComponent<MenuScript>();
		CharacterController = GM.Player.GetComponent<ac_CharacterController>();
		Inventory = GM.Player.GetComponent<InventoryScript>();
		TimeManager = GM.Player.GetComponent<TimeSlow>();
		Anim = WeaponWheelObject.GetComponent<Animation>();
		PopUp = GetComponent<PopUpManager>();
		PostProcessor.profile.TryGetSettings(out DepthOfFieldProcessing);
	}

	private void Update()
	{
		Vector2 vector = Input.mousePosition - WheelRotator.position;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		WheelRotator.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		if (KeyBindingManager.GetKey(KeyAction.Equipment) && !WheelCooldown && Active)
		{
			if (!WheelOpen)
			{
				if (TimeToOpen <= 0f)
				{
					WheelOpen = true;
					Inventory.InWheel = true;
					WeaponWheelObject.SetActive(value: true);
					Anim.Stop();
					Anim.Play("WeaponWheelIn");
					CharacterController.WheelFreezePlayer();
					CharacterController.LockScreen();
					TimeManager.StopGameTime(CutOffFilter: true, Instant: false);
					Menu.HUDElements.SetActive(value: false);
					Menu.CanPauseGame = false;
				}
				else
				{
					TimeToOpen -= Time.deltaTime;
				}
			}
		}
		else
		{
			TimeToOpen = 0.5f;
			if (WheelOpen)
			{
				WheelOpen = false;
				Anim.Stop();
				Anim.Play("WeaponWheelOut");
				CharacterController.WheelUnFreezePlayer();
				CharacterController.UnlockScreen();
				TimeManager.StartGameTime(Instant: false);
				StartCoroutine(WheelCooldownTimer());
			}
		}
		if (WheelOpen || PopUp.PopUpOpen)
		{
			if (DepthOfFieldProcessing.focalLength.value < 75f)
			{
				DepthOfFieldProcessing.focalLength.value += Time.deltaTime * 100f;
			}
			else
			{
				DepthOfFieldProcessing.focalLength.value = 75f;
			}
		}
		else if (DepthOfFieldProcessing.focalLength.value > 0f)
		{
			DepthOfFieldProcessing.focalLength.value -= Time.deltaTime * 100f;
		}
		else
		{
			DepthOfFieldProcessing.focalLength.value = 0f;
		}
	}

	private IEnumerator WheelCooldownTimer()
	{
		WheelCooldown = true;
		yield return new WaitForSeconds(0.25f);
		Menu.HUDElements.SetActive(value: true);
		yield return new WaitForSeconds(0.25f);
		WeaponWheelObject.SetActive(value: false);
		WheelCooldown = false;
		Inventory.InWheel = false;
		Menu.CanPauseGame = true;
	}

	public void SelectWeapon(WeaponScript Weapon)
	{
		WeaponName.text = Inventory.Weapons[Weapon.MyWeaponID].Name.ToString();
		AmmoAmount.text = Inventory.AmmoTypes[Weapon.AmmoTypeIndex].AmmoAmount.ToString();
	}

	public void GiveWeapon(WeaponScript Weapon)
	{
		Inventory.GrabWeapon(Weapon.MyWeaponID);
		if (WheelOpen)
		{
			WheelOpen = false;
			Anim.Stop();
			Anim.Play("WeaponWheelOut");
			CharacterController.WheelUnFreezePlayer();
			CharacterController.UnlockScreen();
			TimeManager.StartGameTime(Instant: false);
			StartCoroutine(WheelCooldownTimer());
		}
	}
}
