using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
	public float TimeToBlink = 1f;

	private float TimeSave;

	private Light MyLight;

	private void Start()
	{
		MyLight = GetComponent<Light>();
		MyLight.enabled = false;
		TimeSave = TimeToBlink;
	}

	private void Update()
	{
		TimeToBlink -= Time.deltaTime;
		if (TimeToBlink <= 0f)
		{
			TimeToBlink = TimeSave;
			StartCoroutine(BlinkLight());
		}
	}

	public IEnumerator BlinkLight()
	{
		MyLight.enabled = true;
		yield return new WaitForSeconds(TimeToBlink / 2f);
		MyLight.enabled = false;
	}
}
