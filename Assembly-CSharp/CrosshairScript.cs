using UnityEngine;
using UnityEngine.UI;

public class CrosshairScript : MonoBehaviour
{
	public RectTransform Reticle;

	public RaycastAim AimPoint;

	public Image GunCooldown;

	public Animation HitmarkerAnim;

	public Animation CrosshairAnim;

	[HideInInspector]
	public float RecoilMultiplier = 1f;

	private float RandomX;

	private float RandomY;

	private float ResetTime = 0.2f;

	private void Update()
	{
		if (ResetTime > 0f)
		{
			ResetTime -= Time.deltaTime;
			Reticle.localPosition = Vector3.Lerp(Reticle.localPosition, new Vector3(RandomX * 10f, RandomY * 10f, 0f), 0.1f);
		}
		else
		{
			Reticle.localPosition = Vector3.Lerp(Reticle.localPosition, new Vector3(0f, 0f, 0f), 0.1f);
		}
	}

	public void Fire(float Inaccuracy)
	{
		RandomX = Random.Range(0f - Inaccuracy, Inaccuracy) * RecoilMultiplier;
		RandomY = Random.Range(0f - Inaccuracy, Inaccuracy) * RecoilMultiplier;
		ResetTime = 0.2f;
		AimPoint.Fire(RandomX, RandomY);
		CrosshairAnim.Stop();
		CrosshairAnim.Play("ReticleFire");
	}

	public void RotateCrosshair()
	{
		Reticle.rotation = Quaternion.Euler(0f, 0f, 90f);
	}

	public void ResetCrosshair()
	{
		Reticle.rotation = Quaternion.Euler(0f, 0f, 0f);
	}

	public void Hitmarker()
	{
		HitmarkerAnim.Stop();
		HitmarkerAnim.Play("Hitmarker");
	}

	public void Headshot()
	{
		HitmarkerAnim.Stop();
		HitmarkerAnim.Play("Headshot");
	}

	public void Kill()
	{
		HitmarkerAnim.Stop();
		HitmarkerAnim.Play("Kill");
	}
}
