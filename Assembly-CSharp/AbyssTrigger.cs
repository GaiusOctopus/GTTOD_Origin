using System.Collections;
using UnityEngine;

public class AbyssTrigger : MonoBehaviour
{
	[ConditionalField("TriggerAbyss", null)]
	public Animation CameraEffects;

	[ConditionalField("TriggerAbyss", null)]
	public GameObject Surface;

	[ConditionalField("TriggerAbyss", null)]
	public GameObject Abyss;

	[ConditionalField("TriggerAbyss", null)]
	public TutorialStarter TutorialObject;

	private Transform Player;

	private ac_CharacterController CharacterController;

	private HealthScript PlayerHealth;

	private InventoryScript Inventory;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		CharacterController = Player.GetComponent<ac_CharacterController>();
		PlayerHealth = Player.GetComponent<HealthScript>();
		Inventory = Player.GetComponent<InventoryScript>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (Player.GetComponent<HealthScript>().Lives > 1)
			{
				PlayerHealth.Die();
			}
			else
			{
				StartCoroutine(Fall());
			}
		}
		if (other.tag == "WeakPoint")
		{
			other.GetComponent<ExplosionTransfer>().Base.SendMessage("Die");
		}
	}

	public IEnumerator Fall()
	{
		CameraEffects.Play("AbyssFall");
		base.gameObject.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(6f);
		foreach (WeaponItem weapon in Inventory.Weapons)
		{
			if (weapon.WeaponObject.gameObject.activeSelf)
			{
				weapon.WeaponObject.LowerWeapon();
			}
		}
		yield return new WaitForSeconds(2f);
		foreach (WeaponItem weapon2 in Inventory.Weapons)
		{
			if (weapon2.WeaponObject.gameObject.activeSelf)
			{
				weapon2.WeaponObject.RaiseWeapon();
			}
		}
		AbyssStart();
	}

	public void AbyssStart()
	{
		Inventory.DropAllWeapons();
		Player.transform.position = Vector3.zero;
		CharacterController.SetXRotation(0f);
		CharacterController.SetYRotation(0f);
		CharacterController.CutsceneStart();
		Surface.SetActive(value: false);
		Abyss.SetActive(value: true);
		TutorialObject.gameObject.SetActive(value: true);
		if (PlayerPrefsPlus.GetBool("FirstTimeBoot", defaultValue: true))
		{
			TutorialObject.SetLandingType(Successful: false);
		}
		else
		{
			TutorialObject.SetLandingType(Successful: true);
		}
	}
}
