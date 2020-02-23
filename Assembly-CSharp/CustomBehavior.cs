using System.Collections;
using UnityEngine;

public class CustomBehavior : MonoBehaviour
{
	public bool AntiLife;

	public bool Speedrunner;

	public bool Ninja;

	public bool SWAT;

	public bool GlassCannon;

	public bool Cowboy;

	public bool Butterfingers;

	public bool UltraCold;

	public bool OutOfMap;

	private void Start()
	{
		if (AntiLife)
		{
			GameManager.GM.AntiLife = true;
		}
		if (Speedrunner)
		{
			GameManager.GM.Speedrunner = true;
		}
		if (Ninja)
		{
			GameManager.GM.Ninja = true;
		}
		if (GlassCannon)
		{
			GameManager.GM.Player.GetComponent<HealthScript>().DamageMultiplier = 4f;
			GameManager.GM.EnemyDamageModifier = 2f;
		}
		if (SWAT)
		{
			GameManager.GM.Player.GetComponent<HealthScript>().SetShieldsOnly();
		}
		if (Cowboy)
		{
			StartCoroutine(GiveWeaponWait());
		}
		if (Butterfingers)
		{
			GameManager.GM.Player.GetComponent<CrosshairScript>().RecoilMultiplier = 10f;
		}
		if (UltraCold)
		{
			GameManager.GM.AntiLife = true;
			GameManager.GM.Player.GetComponent<TimeSlow>().Active = false;
			GameManager.GM.Player.GetComponent<SuperHotTime>().Active = true;
		}
		if (OutOfMap)
		{
			GameManager.GM.EnemyDamageModifier = 0f;
		}
		Object.Destroy(base.gameObject, 10f);
	}

	private IEnumerator GiveWeaponWait()
	{
		yield return new WaitForSeconds(5f);
		GameManager.GM.Player.GetComponent<InventoryScript>().GrabWeapon(14);
	}
}
