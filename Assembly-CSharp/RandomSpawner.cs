using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
	public GameObject[] myObjects;

	private void Start()
	{
		int num = Random.Range(0, myObjects.Length);
		Object.Instantiate(myObjects[num], base.transform.position, Quaternion.identity);
		Object.Destroy(base.gameObject);
	}
}
