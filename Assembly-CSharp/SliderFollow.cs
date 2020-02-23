using UnityEngine;
using UnityEngine.UI;

public class SliderFollow : MonoBehaviour
{
	public WeaponScript MyWeapon;

	private Slider slider;

	private bool ReloadFill;

	private void Start()
	{
		slider = base.gameObject.GetComponent<Slider>();
	}

	private void FixedUpdate()
	{
		if (MyWeapon.isReloading)
		{
			slider.value += Time.deltaTime;
			if (!ReloadFill)
			{
				ReloadFill = true;
				Reload();
			}
		}
		else
		{
			ReloadFill = false;
			slider.maxValue = MyWeapon.MaxAmmo;
			slider.value = MyWeapon.CurrentAmmo;
		}
	}

	private void Reload()
	{
		slider.value = 0f;
		slider.maxValue = MyWeapon.ReloadDuration;
	}
}
