using EZCameraShake;
using System.Collections;
using UnityEngine;

public class FanTheHammer : MonoBehaviour
{
	private WeaponScript Weapon;

	private bool hasReset;

	private float timetoreset = 0.5f;

	private void Start()
	{
		Weapon = GetComponent<WeaponScript>();
	}

	private void Update()
	{
		if (timetoreset > 0f)
		{
			timetoreset -= Time.deltaTime;
		}
		else if (!hasReset)
		{
			hasReset = true;
			Weapon.ChangeFireAnimation("Fire");
		}
	}

	public void CustomFire()
	{
		if (Weapon.PrimaryOn)
		{
			Weapon.ChangeFireAnimation("Fire");
			StartCoroutine(HammerPull());
		}
		else
		{
			Weapon.ChangeFireAnimation("FanTheHammer");
			hasReset = false;
			timetoreset = 0.5f;
		}
	}

	private IEnumerator HammerPull()
	{
		yield return new WaitForSeconds(0.55f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(10f, 2f, 0.4f, 0.65f);
		CameraShaker.Instance.ResetCamera();
	}
}
