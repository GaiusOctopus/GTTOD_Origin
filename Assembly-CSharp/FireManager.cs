using UnityEngine;

public class FireManager : MonoBehaviour
{
	public GameObject muzzleFlash;

	public GameObject bulletShellEffect;

	public GameObject muzzleFlashLight;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			muzzleFlash.SetActive(value: false);
			muzzleFlash.SetActive(value: true);
			bulletShellEffect.SetActive(value: false);
			bulletShellEffect.SetActive(value: true);
			muzzleFlashLight.SetActive(value: true);
		}
		if (Input.GetMouseButtonUp(0))
		{
			muzzleFlashLight.SetActive(value: false);
		}
	}
}
