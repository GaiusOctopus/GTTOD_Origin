using System.Collections;
using UnityEngine;

public class ScenarioSpawner : MonoBehaviour
{
	public GameObject MyEnemy;

	public GameObject SpawnEffect;

	public void Spawn()
	{
		StartCoroutine(SpawnWait());
	}

	public IEnumerator SpawnWait()
	{
		float seconds = Random.Range(0f, 3f);
		yield return new WaitForSeconds(seconds);
		Object.Instantiate(MyEnemy, base.transform.position, base.transform.rotation);
		Object.Instantiate(SpawnEffect, base.transform.position, base.transform.rotation);
	}
}
