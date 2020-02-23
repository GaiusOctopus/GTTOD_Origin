using UnityEngine;

public class Infinifall : MonoBehaviour
{
	public float Height;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.transform.position = new Vector3(other.transform.position.x, Height, other.transform.position.z);
		}
	}
}
