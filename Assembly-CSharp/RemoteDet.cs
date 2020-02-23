using System.Collections;
using UnityEngine;

public class RemoteDet : MonoBehaviour
{
	public GameObject Detonation;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			StartCoroutine(Detonate());
		}
	}

	private IEnumerator Detonate()
	{
		float seconds = Random.Range(0f, 1f);
		yield return new WaitForSeconds(seconds);
		Object.Instantiate(Detonation, base.transform.position, base.transform.rotation);
		Object.Destroy(base.gameObject);
	}
}
