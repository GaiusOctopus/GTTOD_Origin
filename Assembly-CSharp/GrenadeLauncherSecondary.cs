using UnityEngine;

public class GrenadeLauncherSecondary : MonoBehaviour
{
	public GameObject Grenade;

	public GameObject SoundEffect;

	public int AmmoDividend = 2;

	private WeaponScript Weapon;

	private Transform LastMag;

	private void Start()
	{
		Weapon = GetComponent<WeaponScript>();
	}

	private void Update()
	{
		if ((Input.GetKeyDown(Weapon.SwapKey) || Input.GetButtonDown("RightBumper")) && Weapon.CurrentAmmo >= Weapon.MaxAmmo / AmmoDividend && !Weapon.Inventory.DualWielding)
		{
			Object.Instantiate(Grenade, Weapon.MuzzleLocation.transform.position, Weapon.MuzzleLocation.transform.rotation);
			Object.Instantiate(SoundEffect);
			Weapon.CurrentAmmo -= Weapon.MaxAmmo / AmmoDividend;
			Weapon.CheckAmmo();
			Weapon.AnimateWeapon("Fire");
			Weapon.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z - 0.5f);
		}
	}
}
