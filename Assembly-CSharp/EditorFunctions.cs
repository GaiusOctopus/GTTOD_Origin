using System.Collections.Generic;
using UnityEngine;

public class EditorFunctions : MonoBehaviour
{
	public List<GameObject> EnableObjects;

	public List<GameObject> DisableObjects;

	public void Start()
	{
	}

	public void PrepWeaponEditing()
	{
		foreach (GameObject enableObject in EnableObjects)
		{
			enableObject.SetActive(value: true);
		}
		foreach (GameObject disableObject in DisableObjects)
		{
			disableObject.SetActive(value: false);
		}
	}

	public void EndWeaponEditing()
	{
		foreach (GameObject enableObject in EnableObjects)
		{
			enableObject.SetActive(value: false);
		}
		foreach (GameObject disableObject in DisableObjects)
		{
			disableObject.SetActive(value: true);
		}
	}

	public void DeleteSaves()
	{
		PlayerPrefs.DeleteAll();
	}
}
