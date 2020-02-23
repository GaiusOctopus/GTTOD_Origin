using UnityEngine;

public class GrapplePlayer : MonoBehaviour
{
	public AnimationCurve forceCurve;

	private Transform CamParent;

	private ac_CharacterController Player;

	private GameManager GM;

	private Rigidbody PlayerPhysics;

	private GrapplePoint Grapple;

	private Vector3 PreviousPosition;

	private float Distance;

	private float DistanceModifier;

	private bool movingAway;

	private Transform Direction;

	private void Start()
	{
		CamParent = GameObject.FindGameObjectWithTag("MainCamera").transform;
		Player = GameObject.FindGameObjectWithTag("Player").GetComponent<ac_CharacterController>();
		GM = Player.GM;
		PlayerPhysics = GetComponent<Rigidbody>();
		if (PlayerPhysics.velocity.y <= -2.5f)
		{
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 0f, PlayerPhysics.velocity.z);
		}
		Direction = new GameObject("LookDirection").transform;
		Direction.transform.position = base.transform.position;
		Direction.LookAt(Grapple.transform.position);
		PlayerPhysics.AddForce(Direction.transform.forward * 750f);
		PlayerPhysics.AddForce(base.transform.up * 350f);
		Object.Destroy(Direction.gameObject);
	}

	private void Update()
	{
		if (KeyBindingManager.GetKey(KeyAction.Forward) || Input.GetAxis("Vertical") < 0f)
		{
			PlayerPhysics.AddForce(CamParent.transform.forward * 3f);
		}
		if (movingAway)
		{
			Grapple.ResetConnection();
			PlayerPhysics.useGravity = true;
		}
		if (KeyBindingManager.GetKeyDown(KeyAction.Jump) || KeyBindingManager.GetKeyDown(KeyAction.Equipment) || Input.GetButtonDown("A") || Input.GetAxis("LeftTrigger") >= 0.5f)
		{
			Grapple.DisconnectGrapple();
			Object.Destroy(base.gameObject);
		}
	}

	private void FixedUpdate()
	{
		Distance = Vector3.Distance(base.transform.position, Grapple.transform.position);
		if (Distance <= 2f)
		{
			Grapple.DisconnectGrapple();
			Object.Destroy(base.gameObject);
		}
		if (Vector3.Distance(PreviousPosition, Grapple.transform.position) < Vector3.Distance(base.transform.position, Grapple.transform.position))
		{
			movingAway = true;
		}
		else
		{
			movingAway = false;
		}
		PreviousPosition = base.transform.position;
	}

	public void AssignGrapplePoint(GrapplePoint Point)
	{
		Grapple = Point;
	}
}
