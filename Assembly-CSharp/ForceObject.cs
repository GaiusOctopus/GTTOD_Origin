using UnityEngine;

public class ForceObject : MonoBehaviour
{
	public float RequiredForce = 1f;

	public GameObject ObjectToSwap;

	public GameObject ObjectToDelete;

	public bool Parent;

	private GameObject SpawnedObject;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > RequiredForce)
		{
			if (Parent)
			{
				SpawnedObject = Object.Instantiate(ObjectToSwap, base.transform.position, base.transform.rotation);
				SpawnedObject.transform.parent = collision.transform;
				Object.Destroy(ObjectToDelete);
			}
			else
			{
				Object.Instantiate(ObjectToSwap, base.transform.position, base.transform.rotation);
				Object.Destroy(ObjectToDelete);
			}
		}
	}
}
