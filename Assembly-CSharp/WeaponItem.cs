using System;
using UnityEngine;

[Serializable]
public class WeaponItem
{
	public string WeaponName = "WeaponName";

	public string Name = "WeaponName";

	public WeaponScript WeaponObject;

	public int WeaponLevel;

	public int AmmoKillGrant = 1;

	public bool CanUse = true;

	public void SetMyID()
	{
		WeaponObject.SetWeaponID(GameManager.GM.Player.GetComponent<InventoryScript>().Weapons.IndexOf(this));
	}

	public void SetWeaponName()
	{
		WeaponName = GameObject.Find("Player").GetComponent<InventoryScript>().Weapons.IndexOf(this) + ". " + Name.ToString();
	}
}
