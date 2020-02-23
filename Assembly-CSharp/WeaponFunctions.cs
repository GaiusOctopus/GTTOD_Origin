using System.Collections;
using UnityEngine;

public class WeaponFunctions : MonoBehaviour
{
	private WeaponScript MyWeapon;

	public bool PhysicsMag;

	[ConditionalField("PhysicsMag", null)]
	public float MagDropTime;

	[ConditionalField("PhysicsMag", null)]
	public GameObject MagObject;

	[ConditionalField("PhysicsMag", null)]
	public Transform MagDropPoint;

	public void Reload()
	{
		StartCoroutine(MagDrop());
	}

	public IEnumerator MagDrop()
	{
		yield return new WaitForSeconds(MagDropTime);
		Object.Instantiate(MagObject, MagDropPoint.position, MagDropPoint.rotation);
	}

	public void SetBaseView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.SetBaseView();
	}

	public void SetCrouchView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.SetCrouchView();
	}

	public void SetDualView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.SetDualView();
	}

	public void SetConsumeView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.SetConsumeView();
	}

	public void ApplyBaseView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.ApplyBaseView();
	}

	public void ApplyConsumeView()
	{
		MyWeapon = GetComponent<WeaponScript>();
		MyWeapon.ApplyConsumeView();
	}
}
