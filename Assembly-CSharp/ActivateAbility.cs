using UnityEngine;

public class ActivateAbility : MonoBehaviour
{
	public bool WallRun;

	public bool Vault;

	public bool DoubleJump;

	public bool Slide;

	public bool Dash;

	public bool Regen;

	public bool Melee;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			AquireAbility();
		}
	}

	public void AquireAbility()
	{
		if (WallRun)
		{
			CharacterController.CanWallRun = true;
		}
		if (Vault)
		{
			CharacterController.CanVault = true;
		}
		if (DoubleJump)
		{
			CharacterController.CanDoubleJump = true;
		}
		if (Slide)
		{
			CharacterController.CanSlide = true;
		}
		if (Dash)
		{
			CharacterController.CanDash = true;
		}
		if (Regen)
		{
			CharacterController.gameObject.GetComponent<HealthScript>().CanRegen = true;
		}
		if (Melee)
		{
			Inventory.HasMelee = true;
		}
	}
}
