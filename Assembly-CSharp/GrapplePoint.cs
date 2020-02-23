using UnityEngine;

public class GrapplePoint : MonoBehaviour
{
	public GameObject Hook;

	private bool IsConnected;

	private ac_CharacterController Player;

	private Transform RopePoint;

	private FixedJoint CurrentGrapplePoint;

	private LineRenderer LR;

	private bool Pulling;

	private void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<ac_CharacterController>();
	}

	public void GrapplePlayer()
	{
		CurrentGrapplePoint = Object.Instantiate(Hook, base.transform.position, base.transform.rotation).GetComponent<FixedJoint>();
		LR = CurrentGrapplePoint.GetComponent<LineRenderer>();
		IsConnected = true;
	}

	public void DisconnectGrapple()
	{
		IsConnected = false;
		LR.SetPosition(0, base.transform.position);
		LR.SetPosition(1, base.transform.position);
	}

	private void Update()
	{
		if (IsConnected)
		{
			LR.SetPosition(0, CurrentGrapplePoint.transform.position);
			LR.SetPosition(1, RopePoint.transform.position);
		}
	}

	public void HoldConnection()
	{
		if (!Pulling)
		{
			CurrentGrapplePoint.connectedBody = null;
			Pulling = true;
		}
	}

	public void ResetConnection()
	{
		Pulling = false;
	}
}
