using UnityEngine;

public class Paper : MonoBehaviour
{
	private void Awake()
	{
		int num = Random.Range(0, 360);
		base.transform.eulerAngles = new Vector3(0f, num, 0f);
	}
}
