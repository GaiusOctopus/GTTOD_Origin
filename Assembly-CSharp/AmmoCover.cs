using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCover : MonoBehaviour
{
	public WeaponScript MyWeapon;

	public List<Image> Sliders;

	private Image Slider;

	private bool ReloadFill;

	private float CurrentAmmo;

	private float MaxAmmo;

	private float AmmoDividend;

	private void Start()
	{
		Slider = Sliders[0];
	}

	private void Update()
	{
		CurrentAmmo = MyWeapon.CurrentAmmo;
		MaxAmmo = MyWeapon.MaxAmmo;
		AmmoDividend = 1f - CurrentAmmo / MaxAmmo;
		if (MyWeapon.isReloading)
		{
			foreach (Image slider in Sliders)
			{
				slider.fillAmount = 1f;
			}
			ReloadFill = true;
		}
		else if (ReloadFill)
		{
			foreach (Image slider2 in Sliders)
			{
				slider2.fillAmount -= Time.deltaTime * 4f;
			}
			if (Slider.fillAmount <= 0f)
			{
				ReloadFill = false;
			}
		}
		else
		{
			foreach (Image slider3 in Sliders)
			{
				slider3.fillAmount = AmmoDividend / 2f;
			}
		}
	}
}
