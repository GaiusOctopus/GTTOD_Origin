using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheating : MonoBehaviour
{
	public List<GameObject> Children;

	public bool Manager;

	public float Range;

	public GameObject Switch;

	private void Start()
	{
		if (Manager)
		{
			foreach (Transform item in base.transform)
			{
				if (item.tag == "Cover")
				{
					Children.Add(item.gameObject);
					item.GetComponent<Cheating>().SendMessage("Disable");
				}
			}
		}
	}

	public IEnumerator Disable()
	{
		Range = Random.Range(1f, 3.5f);
		yield return new WaitForSeconds(Range);
		Object.Instantiate(Switch);
		base.gameObject.SetActive(value: false);
	}
}
