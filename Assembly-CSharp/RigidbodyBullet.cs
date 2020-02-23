using UnityEngine;

public class RigidbodyBullet : MonoBehaviour
{
	public float Speed;

	private Rigidbody RB;

	private void Start()
	{
		RB = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		RB.AddForce(base.transform.forward * Speed);
		if (RB.velocity.magnitude >= Speed)
		{
			RB.AddForce(base.transform.forward * (0f - Speed));
		}
	}
}
