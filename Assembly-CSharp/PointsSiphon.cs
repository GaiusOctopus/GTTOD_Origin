using UnityEngine;

public class PointsSiphon : MonoBehaviour
{
	public GTTODManager Manager;

	public int Points = 1;

	private WeaponScript Weapon;

	private bool Reset;

	private void Start()
	{
		Weapon = GetComponent<WeaponScript>();
	}

	public void CustomFire()
	{
		Manager.Points -= Points;
		Manager.CheckPointsUI();
	}

	public void Update()
	{
		if (!Weapon.PrimaryOn && Weapon.CustomSecondary)
		{
			if (Manager.Points <= Points)
			{
				Weapon.CanFire = false;
				Reset = true;
			}
		}
		else if (Reset)
		{
			Weapon.CanFire = true;
			Reset = false;
		}
	}
}
