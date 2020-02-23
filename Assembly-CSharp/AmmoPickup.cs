using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
	public int AmmoMin = 1;

	public int AmmoMax = 15;

	public string AmmoName = "BasicAmmo";

	public GameObject SpawnObject;

	private InventoryScript Inventory;

	private int AmmoIndex;

	private int AmmoAmount;

	private void Start()
	{
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
		AmmoAmount = Random.Range(AmmoMin, AmmoMax);
		foreach (AmmoType ammoType in Inventory.AmmoTypes)
		{
			if (ammoType.AmmoName == AmmoName)
			{
				AmmoIndex = Inventory.AmmoTypes.IndexOf(ammoType);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && Inventory.AmmoTypes[AmmoIndex].AmmoAmount < Inventory.AmmoTypes[AmmoIndex].MaxAmmoLimit)
		{
			Inventory.AmmoTypes[AmmoIndex].AddAmmo(AmmoAmount);
			Inventory.PrintAmmoAmount(AmmoAmount, AmmoName);
			Object.Instantiate(SpawnObject, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
		}
	}
}
