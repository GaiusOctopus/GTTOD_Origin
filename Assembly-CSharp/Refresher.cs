using System.Collections;
using UnityEngine;

public class Refresher : MonoBehaviour
{
	public GameObject Replacement;

	private void Start()
	{
		StartCoroutine(Refresh());
	}

	private IEnumerator Refresh()
	{
		yield return new WaitForSeconds(15f);
		Object.Instantiate(Replacement, base.transform.position, base.transform.rotation);
		Object.Destroy(base.gameObject);
	}
}
