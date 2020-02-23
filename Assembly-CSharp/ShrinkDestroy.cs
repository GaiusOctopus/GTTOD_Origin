using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkDestroy : MonoBehaviour
{
	public float TimeToShrink = 1f;

	public float ShrinkSpeed = 1f;

	public float DestroyTime = 5f;

	public List<Transform> Objects;

	private bool Shrinking;

	private void Start()
	{
		StartCoroutine(StartShrink());
		Object.Destroy(base.gameObject, DestroyTime);
	}

	private void Update()
	{
		if (Shrinking)
		{
			foreach (Transform @object in Objects)
			{
				@object.localScale = new Vector3(@object.localScale.x - Time.deltaTime * ShrinkSpeed, @object.localScale.y - Time.deltaTime * ShrinkSpeed, @object.localScale.z - Time.deltaTime * ShrinkSpeed);
				if (@object.localScale.x <= 0f)
				{
					Shrinking = false;
					@object.gameObject.SetActive(value: false);
				}
			}
		}
	}

	private IEnumerator StartShrink()
	{
		yield return new WaitForSeconds(TimeToShrink);
		Shrinking = true;
	}
}
