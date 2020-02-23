using UnityEngine;

public class LAW : MonoBehaviour
{
	private WeaponScript Weapon;

	private void Awake()
	{
		Weapon = GetComponent<WeaponScript>();
	}

	private void Update()
	{
		if (Weapon.PrimaryOn)
		{
			Weapon.Crosshair.ResetCrosshair();
		}
		else
		{
			Weapon.Crosshair.RotateCrosshair();
		}
	}

	private void OnDisable()
	{
		Weapon.Crosshair.ResetCrosshair();
	}
}
