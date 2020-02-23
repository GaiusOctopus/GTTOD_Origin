using UnityEngine;

public class RigidbodyForceDirection : MonoBehaviour
{
	public float ForwardForce;

	public float UpForce;

	public float SideForce;

	public bool AddPlayerVelocity;

	public bool AddTorque;

	private Rigidbody Rigidbody;

	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();
		if (AddPlayerVelocity)
		{
			Rigidbody.velocity = GameManager.GM.Player.GetComponent<Rigidbody>().velocity + (base.transform.forward * ForwardForce + base.transform.up * UpForce + base.transform.right * SideForce);
		}
		else
		{
			Rigidbody.velocity = base.transform.forward * ForwardForce + base.transform.up * UpForce + base.transform.right * SideForce;
		}
		if (AddTorque)
		{
			float d = Random.Range(-10, 10) * 100;
			Rigidbody.AddTorque(base.transform.forward * d, ForceMode.VelocityChange);
			Rigidbody.AddTorque(base.transform.right * d, ForceMode.VelocityChange);
		}
	}
}
