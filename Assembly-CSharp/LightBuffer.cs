using UnityEngine;

[DisallowMultipleComponent]
public class LightBuffer : MonoBehaviour
{
	public float Distance = 30f;

	private Light MyLight;

	private Transform Player;

	private float TimeToCheckDistance;

	private float MyTime;

	private float LightIntensity;

	private bool InRange;

	private bool FullLight;

	private void Start()
	{
		MyLight = GetComponent<Light>();
		Player = GameManager.GM.Player.transform;
		MyTime = Random.Range(0.5f, 1.5f);
		LightIntensity = MyLight.intensity;
	}

	private void Update()
	{
		LightsUpdate();
	}

	private void LightsUpdate()
	{
		TimeToCheckDistance -= Time.deltaTime;
		if (TimeToCheckDistance <= 0f)
		{
			TimeToCheckDistance = MyTime;
			if (Vector3.Distance(base.transform.position, Player.position) < Distance)
			{
				MyLight.enabled = true;
				InRange = true;
			}
			else
			{
				MyLight.enabled = false;
				MyLight.intensity = 0f;
				InRange = false;
				FullLight = false;
			}
		}
		if (InRange && !FullLight)
		{
			if (MyLight.intensity < LightIntensity)
			{
				MyLight.intensity += Time.deltaTime * LightIntensity;
				FullLight = false;
			}
			else
			{
				MyLight.intensity = LightIntensity;
				FullLight = true;
			}
		}
	}
}
