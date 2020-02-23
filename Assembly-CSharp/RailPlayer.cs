using Pixelplacement;
using UnityEngine;

public class RailPlayer : MonoBehaviour
{
	public Transform Player;

	public Spline RailSpline;

	public float Speed = 0.01f;

	public float RailPercentage;

	public SplineFollower Follower;

	private bool OnRail;

	private void Start()
	{
		Follower = RailSpline.followers[0];
		RailPercentage = RailSpline.ClosestPoint(new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z));
	}

	private void FixedUpdate()
	{
		Follower.percentage = RailPercentage;
		if (!OnRail)
		{
			RailPercentage = RailSpline.ClosestPoint(new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z));
		}
		else
		{
			RailPercentage += Time.deltaTime * Speed;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			Player.parent = base.transform;
			Player.position = new Vector3(base.transform.position.x, base.transform.position.y + 1.5f, base.transform.position.z);
			Player.GetComponent<ac_CharacterController>().BeginRail();
			OnRail = true;
		}
	}
}
