using UnityEngine;

public class FallCheckpoint : MonoBehaviour
{
	public Transform Checkpoint;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.transform.position = Checkpoint.position;
			other.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}
