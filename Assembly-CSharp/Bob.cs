using System;
using UnityEngine;

public class Bob : MonoBehaviour
{
	public float degreesPerSecond = 15f;

	public float amplitude = 0.5f;

	public float frequency = 1f;

	private Vector3 posOffset;

	private Vector3 tempPos;

	private void Start()
	{
		posOffset = base.transform.position;
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(Time.deltaTime * degreesPerSecond, Time.deltaTime * degreesPerSecond, Time.deltaTime * degreesPerSecond), Space.World);
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * (float)Math.PI * frequency) * amplitude;
		base.transform.position = tempPos;
	}
}
