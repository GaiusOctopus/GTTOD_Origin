using EZCameraShake;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
	private float TimeToShake = 0.1f;

	private void Update()
	{
		TimeToShake -= Time.deltaTime;
		if (TimeToShake <= 0f)
		{
			Shake();
		}
	}

	private void Shake()
	{
		TimeToShake = Random.Range(0.1f, 0.35f);
		float magnitude = Random.Range(2f, 6f);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.5f, 0.1f, 0.5f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(1f, 1f, 1f);
		CameraShaker.Instance.ShakeOnce(magnitude, 1f, TimeToShake, TimeToShake);
		CameraShaker.Instance.ResetCamera();
	}
}
