using UnityEngine;

public class LockScale : MonoBehaviour
{
	private void Update()
	{
		base.transform.localScale = new Vector3(1000f, 1000f, 3500f);
	}
}
