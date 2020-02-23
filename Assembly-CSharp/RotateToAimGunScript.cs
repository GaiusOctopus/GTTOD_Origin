using UnityEngine;

public class RotateToAimGunScript : MonoBehaviour
{
	public Transform spineBone;

	public Transform bulletSpawnTransform;

	[HideInInspector]
	public Transform targetTransform;

	public bool shouldDebug;

	public Vector3 maximumRotationAngles = new Vector3(90f, 90f, 90f);

	public float rotationSpeed = 5f;

	private Quaternion spineRotationLastFrame;

	private Vector3 tempSpineLocalEulerAngles;

	public bool isEnabled;

	public float minDistToAim;

	private Quaternion targetRot;

	public int maxIterations = 2;

	public float minAngle = 5f;

	public bool useHighQualityAiming;

	public bool stopForCover;

	private void Awake()
	{
		if ((bool)spineBone)
		{
			spineRotationLastFrame = spineBone.rotation;
		}
		else
		{
			base.enabled = false;
		}
		targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void LateUpdate()
	{
		if (isEnabled && (bool)targetTransform && minDistToAim < Vector3.Distance(spineBone.position, targetTransform.position) && !stopForCover)
		{
			if (!useHighQualityAiming)
			{
				spineBone.rotation = Quaternion.FromToRotation(bulletSpawnTransform.forward, targetTransform.position - bulletSpawnTransform.position) * spineBone.rotation;
				tempSpineLocalEulerAngles = spineBone.localEulerAngles;
				tempSpineLocalEulerAngles = new Vector3(ResetIfTooHigh(tempSpineLocalEulerAngles.x, maximumRotationAngles.x), ResetIfTooHigh(tempSpineLocalEulerAngles.y, maximumRotationAngles.y), ResetIfTooHigh(tempSpineLocalEulerAngles.z, maximumRotationAngles.z));
				spineBone.localEulerAngles = tempSpineLocalEulerAngles;
				targetRot = spineBone.rotation;
				spineBone.rotation = Quaternion.Slerp(spineRotationLastFrame, targetRot, Time.deltaTime * rotationSpeed);
				spineRotationLastFrame = spineBone.rotation;
			}
			else
			{
				float num = 129601f;
				int num2 = maxIterations;
				while (num2 > 0 && num > minAngle)
				{
					Vector3 b = bulletSpawnTransform.TransformPoint(bulletSpawnTransform.position);
					if (new Plane(spineBone.forward, spineBone.position).Raycast(new Ray(bulletSpawnTransform.position, -bulletSpawnTransform.forward), out float enter))
					{
						b = bulletSpawnTransform.position + bulletSpawnTransform.forward * enter;
					}
					Quaternion lhs = Quaternion.FromToRotation(bulletSpawnTransform.forward, targetTransform.position - b);
					num = lhs.eulerAngles.sqrMagnitude;
					num2--;
					spineBone.rotation = lhs * spineBone.rotation;
				}
				spineBone.rotation = Quaternion.Slerp(spineRotationLastFrame, spineBone.rotation, Time.deltaTime * rotationSpeed);
				spineRotationLastFrame = spineBone.rotation;
			}
			if (shouldDebug)
			{
				Debug.DrawRay(bulletSpawnTransform.position, bulletSpawnTransform.forward * 1000f, Color.red);
				Debug.DrawLine(bulletSpawnTransform.position, targetTransform.position, Color.blue);
			}
		}
		else
		{
			targetRot = spineBone.rotation;
			spineBone.rotation = Quaternion.Slerp(spineRotationLastFrame, targetRot, Time.deltaTime * rotationSpeed * 2f);
			spineRotationLastFrame = spineBone.rotation;
		}
	}

	public void Activate()
	{
		spineRotationLastFrame = spineBone.rotation;
		isEnabled = true;
	}

	public void Deactivate()
	{
		isEnabled = false;
	}

	public void SetTargetTransform(Transform x)
	{
		targetTransform = x;
	}

	private float ClampEulerAngles(float r, float lim)
	{
		if (r > 180f)
		{
			r -= 360f;
		}
		r = Mathf.Clamp(r, 0f - lim, lim);
		return r;
	}

	private float ResetIfTooHigh(float r, float lim)
	{
		if (r > 180f)
		{
			r -= 360f;
		}
		if (r < 0f - lim || r > lim)
		{
			return 0f;
		}
		return r;
	}
}
