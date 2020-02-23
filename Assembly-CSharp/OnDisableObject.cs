using UnityEngine;

public class OnDisableObject : MonoBehaviour
{
	public GameObject MyObject;

	private void OnDisable()
	{
		Object.Instantiate(MyObject, base.transform.position, base.transform.rotation);
	}
}
