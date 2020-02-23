using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
	public List<GameObject> SpawnObjects;

	private void Start()
	{
		StartCoroutine(SpawnMyObjects());
	}

	public void Add(GameObject NewItem)
	{
		SpawnObjects.Add(NewItem);
	}

	private IEnumerator SpawnMyObjects()
	{
		yield return new WaitForSeconds(0.1f);
		foreach (GameObject spawnObject in SpawnObjects)
		{
			spawnObject.GetComponent<SpawnObject>().SpawnMyObject();
		}
	}
}
