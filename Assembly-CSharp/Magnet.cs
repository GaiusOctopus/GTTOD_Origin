using UnityEngine;

public class Magnet : MonoBehaviour
{
	public float MagnetStrength;

	private Rigidbody Player;

	private bool ShouldMagnetize;

	private float Distance;

	private Vector3 Direction;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Player = other.GetComponent<Rigidbody>();
			ShouldMagnetize = true;
		}
	}

	private void Update()
	{
		if (ShouldMagnetize)
		{
			Distance = Vector3.Distance(Player.position, base.transform.position);
			Direction = base.transform.position - Player.transform.position;
			Player.AddForce(Direction * MagnetStrength / Distance);
			if (Distance <= 1f)
			{
				ShouldMagnetize = false;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			ShouldMagnetize = false;
		}
	}
}
