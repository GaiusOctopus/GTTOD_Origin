using UnityEngine;

public class Driller : MonoBehaviour
{
	public GameObject ShotgunBlast;

	public GameObject RifleRound;

	private WeaponScript MyWeapon;

	private void Awake()
	{
		MyWeapon = GetComponent<WeaponScript>();
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse1))
		{
			MyWeapon.PrimaryBulletConsumption = 2;
			MyWeapon.SecondaryBulletConsumption = 2;
			MyWeapon.PrimaryBullet = RifleRound;
			MyWeapon.SecondaryBullet = RifleRound;
		}
		else
		{
			MyWeapon.PrimaryBulletConsumption = 1;
			MyWeapon.SecondaryBulletConsumption = 1;
			MyWeapon.PrimaryBullet = ShotgunBlast;
			MyWeapon.SecondaryBullet = ShotgunBlast;
		}
	}
}
