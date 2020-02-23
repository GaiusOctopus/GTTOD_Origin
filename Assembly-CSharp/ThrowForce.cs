using UnityEngine;

public class ThrowForce : MonoBehaviour
{
	public float thrust;

	public bool ThrowAtPlayer;

	private Rigidbody Parent;

	private Transform Direction;

	private Rigidbody Rigidbody;

	private void Start()
	{
		Parent = GameManager.GM.Player.GetComponent<Rigidbody>();
		Direction = GameObject.FindGameObjectWithTag("MainCamera").transform;
		Rigidbody = GetComponent<Rigidbody>();
		if (ThrowAtPlayer)
		{
			base.transform.LookAt(Parent.gameObject.transform.position);
			Rigidbody.velocity = Direction.forward * (thrust * -1f) + Parent.transform.up * thrust / 2f;
		}
		else
		{
			Rigidbody.velocity = Direction.forward * base.transform.InverseTransformDirection(Parent.velocity).z + Direction.forward * thrust + Parent.transform.up * thrust / 4f;
		}
	}
}
