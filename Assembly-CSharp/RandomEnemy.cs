using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
	public GameObject[] Enemies;

	[Header("Spawn Effects")]
	public GameObject SpawnEffect;

	private void Start()
	{
		Spawn();
		Object.Destroy(base.gameObject, 1f);
	}

	private void Spawn()
	{
		int num = Random.Range(0, Enemies.Length);
		Object.Instantiate(Enemies[num], base.transform.position, Quaternion.identity);
		Object.Instantiate(SpawnEffect, base.transform.position, Quaternion.identity);
	}
}
