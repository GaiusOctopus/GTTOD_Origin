using UnityEngine;

public class Specter : MonoBehaviour
{
	public GameObject Grenade;

	public GameObject SoundEffect;

	public GameObject ElectricMag;

	public int AmmoDividend = 2;

	private WeaponScript Weapon;

	private Transform LastMag;

	private void Start()
	{
		Weapon = GetComponent<WeaponScript>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1) && Weapon.CurrentAmmo >= Weapon.MaxAmmo / AmmoDividend)
		{
			Object.Instantiate(Grenade, Weapon.MuzzleLocation.transform.position, Weapon.MuzzleLocation.transform.rotation);
			Object.Instantiate(SoundEffect);
			Weapon.CurrentAmmo -= Weapon.MaxAmmo / AmmoDividend;
			Weapon.CheckAmmo();
			Weapon.AnimateWeapon("Fire");
			Weapon.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, base.transform.localPosition.z - 0.5f);
		}
		if (KeyBindingManager.GetKeyDown(KeyAction.Reload) && Weapon.CurrentAmmo < Weapon.MaxAmmo)
		{
			LastMag = Object.Instantiate(ElectricMag, Weapon.Player.transform.position, Weapon.Player.transform.rotation).transform;
			LastMag.parent = Weapon.Player.transform;
		}
	}
}
