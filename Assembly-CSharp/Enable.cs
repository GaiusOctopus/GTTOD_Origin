using System.Collections.Generic;
using UnityEngine;

public class Enable : MonoBehaviour
{
	public bool DisableOnAwake;

	public List<GameObject> EnableObjects;

	public List<GameObject> DisableObjects;

	private Transform Player;

	private bool HasEnabled;

	private void Start()
	{
		if (DisableOnAwake)
		{
			ResetFuntionality();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Activate();
		}
	}

	public void Activate()
	{
		if (EnableObjects.Count > 0)
		{
			foreach (GameObject enableObject in EnableObjects)
			{
				enableObject.SetActive(value: true);
			}
		}
		if (DisableObjects.Count > 0)
		{
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: false);
			}
		}
	}

	public void Deactivate()
	{
		if (EnableObjects.Count > 0)
		{
			foreach (GameObject enableObject in EnableObjects)
			{
				enableObject.SetActive(value: false);
			}
		}
		if (DisableObjects.Count > 0)
		{
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: true);
			}
		}
	}

	public void ResetFuntionality()
	{
		HasEnabled = false;
		if (EnableObjects.Count > 0)
		{
			foreach (GameObject enableObject in EnableObjects)
			{
				enableObject.SetActive(value: false);
			}
		}
		if (DisableObjects.Count > 0)
		{
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: true);
			}
		}
	}
}
