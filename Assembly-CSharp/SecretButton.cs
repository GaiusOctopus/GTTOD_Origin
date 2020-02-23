using System.Collections.Generic;
using UnityEngine;

public class SecretButton : MonoBehaviour
{
	public GameObject OffButton;

	[Header("OBJECT EDITS")]
	public List<GameObject> EnableObjects;

	public List<GameObject> DisableObjects;

	private AudioSource Audio;

	private void Start()
	{
		Audio = GetComponent<AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name.Contains("SpecialBox"))
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
			OffButton.SetActive(value: false);
			Audio.Play();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name.Contains("SpecialBox"))
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
			OffButton.SetActive(value: true);
		}
	}
}
