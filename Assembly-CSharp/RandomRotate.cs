using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
	public List<GameObject> Bullets;

	private void Start()
	{
		float z = Random.Range(-360, 360);
		base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		EnableObjects();
	}

	private void EnableObjects()
	{
		foreach (GameObject bullet in Bullets)
		{
			bullet.SetActive(value: true);
		}
	}
}
