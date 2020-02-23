using UnityEngine;

public class Interactable : MonoBehaviour
{
	[Header("Weapon Interactions")]
	public int myID;

	public WeaponOrb WeaponOrb;

	public GameObject PickUpEffect;

	[Header("Private Variables")]
	private GameObject Player;

	private InventoryScript Inventory;

	private RaycastAim Raycaster;

	private WeaponOrb OrbSave;

	private bool Active;

	private bool Pulling;

	private void Start()
	{
		Physics.IgnoreLayerCollision(2, 19);
		Player = GameManager.GM.Player;
		Inventory = Player.GetComponent<InventoryScript>();
		Raycaster = Player.GetComponent<ac_CharacterController>().MainCamera.GetComponent<RaycastAim>();
	}

	private void Update()
	{
		if (!Active || myID == Inventory.PrimaryID || myID == Inventory.SecondaryID || GameManager.GM.Ninja)
		{
			return;
		}
		if (Raycaster.MyLookedAtObject != base.gameObject)
		{
			Stable();
		}
		if (KeyBindingManager.GetKey(KeyAction.Interact) || Input.GetButton("X"))
		{
			if (Raycaster.FillAmount > 0f)
			{
				Raycaster.FillAmount -= Time.deltaTime * 4f;
			}
			else if (!Pulling)
			{
				Pulling = true;
				Object.Instantiate(PickUpEffect, base.transform.position, base.transform.rotation);
				OrbSave = Object.Instantiate(WeaponOrb, base.transform.position, base.transform.rotation);
				OrbSave.SetItem(Player.transform, myID);
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			Raycaster.FillAmount = 1f;
		}
	}

	public void Interacting(GameObject CurrentPlayer)
	{
		if (!Active)
		{
			Active = true;
			Player = CurrentPlayer;
			Raycaster = Player.GetComponent<RaycastAim>();
		}
	}

	public void Stable()
	{
		if (Active)
		{
			Active = false;
			Raycaster.FillAmount = 1f;
		}
	}
}
