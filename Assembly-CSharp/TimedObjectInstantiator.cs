using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObjectInstantiator : MonoBehaviour
{
	public float Time;

	public List<GameObject> Objects;

	public bool ShouldDestroy;

	private void Start()
	{
		StartCoroutine(Instantiate());
	}

	private IEnumerator Instantiate()
	{
		yield return new WaitForSeconds(Time);
		foreach (GameObject @object in Objects)
		{
			Object.Instantiate(@object, base.transform.position, base.transform.rotation);
		}
		if (ShouldDestroy)
		{
			Object.Destroy(base.gameObject, 0.5f);
		}
	}
}
