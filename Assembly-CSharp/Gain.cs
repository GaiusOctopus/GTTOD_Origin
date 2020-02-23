using UnityEngine;

public class Gain : MonoBehaviour
{
	public float Slowmo;

	public int Health;

	private void Start()
	{
		GameManager gM = GameManager.GM;
		InventoryScript component = gM.Player.GetComponent<InventoryScript>();
		gM.GetComponent<GTTODManager>().Gain(Slowmo);
		gM.Player.GetComponent<HealthScript>().Heal(Health);
		if (component.CurrentWeapon != null)
		{
			component.AmmoTypes[component.CurrentWeapon.AmmoTypeIndex].AddAmmo(component.Weapons[component.CurrentWeapon.MyWeaponID].AmmoKillGrant);
		}
	}
}
