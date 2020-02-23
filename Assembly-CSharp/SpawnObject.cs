using UnityEngine;

public class SpawnObject : MonoBehaviour
{
	private StartLevel LevelManager;

	public GameObject myObject;

	private void Start()
	{
		LevelManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<StartLevel>();
		LevelManager.Add(base.gameObject);
	}

	public void SpawnMyObject()
	{
		Object.Instantiate(myObject, base.transform.position, base.transform.rotation);
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
}
