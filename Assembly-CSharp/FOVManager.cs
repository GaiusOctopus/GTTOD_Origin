using EZCameraShake;
using UnityEngine;

public class FOVManager : MonoBehaviour
{
	public float NormalFOV;

	private Camera MyCam;

	private CameraShaker CamShake;

	private float CamShakeMagnitude;

	private float CamShakeMagnitudeZoom;

	private bool Zooming;

	private bool SafeZooming;

	private bool Bumping;

	public void Start()
	{
		MyCam = GetComponent<Camera>();
		CamShake = GetComponent<CameraShaker>();
		CamShakeMagnitude = CamShake.CameraShakeModifier;
		CamShakeMagnitudeZoom = CamShakeMagnitude / 2f;
		NormalFOV = PlayerPrefs.GetFloat("FieldOfView", 100f);
	}

	public void Update()
	{
		if (Bumping)
		{
			if (MyCam.fieldOfView > NormalFOV)
			{
				MyCam.fieldOfView -= Time.deltaTime * 20f;
				return;
			}
			MyCam.fieldOfView = NormalFOV;
			Bumping = false;
		}
	}

	public void Zoom(float ZoomFOV)
	{
		MyCam.fieldOfView = Mathf.Lerp(MyCam.fieldOfView, ZoomFOV, 0.15f);
		CamShake.CameraShakeModifier = CamShakeMagnitudeZoom;
		Zooming = true;
	}

	public void UnZoom()
	{
		MyCam.fieldOfView = Mathf.Lerp(MyCam.fieldOfView, NormalFOV, 0.15f);
		CamShake.CameraShakeModifier = CamShakeMagnitude;
		Zooming = false;
	}

	public void BumpFOV(float BumpAmount)
	{
		if (!Zooming && !SafeZooming)
		{
			MyCam.fieldOfView = NormalFOV + BumpAmount;
			Bumping = true;
		}
	}

	public void SafeZoom(float ZoomAddition)
	{
		if (!Zooming)
		{
			MyCam.fieldOfView = Mathf.Lerp(MyCam.fieldOfView, NormalFOV + ZoomAddition, 0.015f);
			CamShake.CameraShakeModifier = CamShakeMagnitudeZoom;
			SafeZooming = true;
		}
	}

	public void SafeUnZoom()
	{
		if (!Zooming && MyCam != null)
		{
			MyCam.fieldOfView = Mathf.Lerp(MyCam.fieldOfView, NormalFOV, 0.15f);
			CamShake.CameraShakeModifier = CamShakeMagnitude;
			SafeZooming = false;
		}
	}

	public void QuickUnzoom()
	{
		MyCam.fieldOfView = NormalFOV;
	}

	public void CheckFOV()
	{
		if (CamShake == null)
		{
			CamShake = GetComponent<CameraShaker>();
		}
		NormalFOV = PlayerPrefs.GetFloat("FieldOfView", 100f);
		CamShakeMagnitude = CamShake.CameraShakeModifier;
		CamShakeMagnitudeZoom = CamShakeMagnitude / 2f;
	}
}
