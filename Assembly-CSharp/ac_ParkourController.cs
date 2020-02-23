using System.Collections;
using UnityEngine;

public class ac_ParkourController : MonoBehaviour
{
	public LayerMask AvailableLayers;

	public float MaxCornerAngle = 50f;

	public float MaxElbowAngle = 135f;

	private GameManager GM;

	private ac_CharacterController CharacterController;

	[HideInInspector]
	public Rigidbody PlayerPhysics;

	private bool HorizontalWall;

	private bool RightWall;

	private bool WallRunning;

	private bool hasBumped;

	private bool hasJumped;

	private bool hasReleased;

	private bool Stable;

	private bool VerticalStable;

	private bool Bumping;

	private float StartingSpeed;

	private float ForwardSpeed;

	private float VerticalSpeed = 6f;

	private float TimeToDip = 0.5f;

	private float MaxSpeed = 40f;

	private RaycastHit HitCheck;

	private RaycastHit HitWall;

	private Quaternion WallRotation;

	private Transform DirectionObject;

	private Vector3 PhysicsSave;

	public void StartWallRun(bool Horizontal, bool Right)
	{
		GM = GameManager.GM;
		CharacterController = GM.Player.GetComponent<ac_CharacterController>();
		PlayerPhysics = GetComponent<Rigidbody>();
		DirectionObject = base.gameObject.transform.GetChild(0).transform;
		StartingSpeed = CharacterController.TransferSpeedAmount;
		ForwardSpeed = StartingSpeed;
		if (Horizontal)
		{
			HorizontalWall = true;
			if (Right)
			{
				RightWall = true;
				base.transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
				PlayerPhysics.velocity = base.transform.forward * StartingSpeed;
			}
			else
			{
				RightWall = false;
				base.transform.rotation *= Quaternion.Euler(0f, -90f, 0f);
				PlayerPhysics.velocity = base.transform.forward * StartingSpeed;
			}
		}
		else
		{
			base.transform.rotation *= Quaternion.Euler(0f, 180f, 0f);
			HorizontalWall = false;
		}
		WallRunning = true;
		StartCoroutine(WallStability());
	}

	private void Update()
	{
		if (!WallRunning || Bumping)
		{
			return;
		}
		CharacterController.transform.position = base.transform.position;
		if ((CharacterController.KeyboardMouse && !KeyBindingManager.GetKey(KeyAction.Jump)) || (!CharacterController.KeyboardMouse && !Input.GetButton("A")))
		{
			hasReleased = true;
		}
		if (KeyBindingManager.GetKeyDown(KeyAction.Jump) || Input.GetButtonDown("A"))
		{
			if (HorizontalWall)
			{
				if (hasReleased && Stable && !hasJumped)
				{
					CharacterController.WallJump(HorizontalWall, RightWall);
					hasJumped = true;
				}
			}
			else if (hasReleased && VerticalStable && !hasJumped)
			{
				CharacterController.WallJump(HorizontalWall, RightWall);
				hasJumped = true;
			}
		}
		if ((KeyBindingManager.GetKeyDown(KeyAction.Crouch) || Input.GetButtonDown("B")) && hasReleased && Stable)
		{
			CharacterController.WallBump(AddForce: true);
		}
		if (HorizontalWall)
		{
			DirectionObject.rotation = WallRotation;
			PlayerPhysics.velocity = DirectionObject.forward * ForwardSpeed + base.transform.up * VerticalSpeed;
			if (KeyBindingManager.GetKey(KeyAction.Forward) || (Input.GetAxis("Vertical") < 0f && !CharacterController.KeyboardMouse))
			{
				if (ForwardSpeed <= MaxSpeed)
				{
					ForwardSpeed = Mathf.Lerp(ForwardSpeed, MaxSpeed, 0.005f);
				}
				VerticalSpeed -= Time.deltaTime * 6f;
			}
			else
			{
				ForwardSpeed = Mathf.Lerp(ForwardSpeed, 0f, 0.005f);
				VerticalSpeed -= Time.deltaTime * 8f;
			}
			if (KeyBindingManager.GetKey(KeyAction.Backward) || (Input.GetAxis("Vertical") > 0f && !CharacterController.KeyboardMouse))
			{
				TimeToDip -= Time.deltaTime;
				ForwardSpeed = Mathf.Lerp(ForwardSpeed, 0f, 0.05f);
				VerticalSpeed += Time.deltaTime * 6f;
				if (TimeToDip <= 0f)
				{
					CharacterController.WallBump(AddForce: true);
				}
			}
			else if (TimeToDip < 0.5f)
			{
				TimeToDip += Time.deltaTime;
			}
			else
			{
				TimeToDip = 0.5f;
			}
			if (Physics.Raycast(base.transform.position, base.transform.forward, out HitCheck, 6f, AvailableLayers) && ((RightWall && Vector3.Angle(HitCheck.normal, base.transform.right) <= MaxCornerAngle && Stable && !hasJumped) || (!RightWall && Vector3.Angle(HitCheck.normal, base.transform.right) >= MaxCornerAngle && Stable && !hasJumped)))
			{
				CharacterController.WallJump(HorizontalWall, RightWall);
				hasJumped = true;
			}
			if (Physics.Raycast(base.transform.position, base.transform.up * -1f, out HitCheck, 1.15f, AvailableLayers) && Stable && !hasJumped)
			{
				CharacterController.WallJump(HorizontalWall, RightWall);
				hasJumped = true;
			}
			if (!Physics.Raycast(base.transform.position, base.transform.right, out HitWall, 3f, AvailableLayers) && !Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitWall, 3f, AvailableLayers))
			{
				if (!Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 1f), base.transform.right, out HitWall, 3f, AvailableLayers) && !Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 1f), base.transform.right * -1f, out HitWall, 3f, AvailableLayers) && Stable && !hasJumped)
				{
					StartCoroutine(BumpPause());
				}
				return;
			}
			if ((RightWall && Vector3.Angle(HitWall.normal, base.transform.right) < MaxElbowAngle && Stable && !hasJumped) || (!RightWall && Vector3.Angle(HitWall.normal, base.transform.right * -1f) < MaxElbowAngle && Stable && !hasJumped))
			{
				CharacterController.WallJump(HorizontalWall, RightWall);
				hasJumped = true;
				return;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, HitWall.point + HitWall.normal * 0.5f, 0.05f);
			if (Physics.Raycast(DirectionObject.position, DirectionObject.forward, out HitCheck, 3f, AvailableLayers))
			{
				if ((RightWall && Vector3.Angle(HitCheck.normal, base.transform.right) > MaxElbowAngle) || (!RightWall && Vector3.Angle(HitCheck.normal, base.transform.right * -1f) > MaxElbowAngle))
				{
					WallRotation = Quaternion.LookRotation(HitCheck.normal);
				}
			}
			else
			{
				WallRotation = Quaternion.LookRotation(HitWall.normal);
			}
			if (RightWall)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, WallRotation *= Quaternion.Euler(0f, 90f, 0f), 0.175f);
			}
			else
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, WallRotation *= Quaternion.Euler(0f, -90f, 0f), 0.175f);
			}
			if ((!Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 0.75f, base.transform.position.z), base.transform.right, out HitWall, 2f, AvailableLayers) && !Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 0.75f, base.transform.position.z), base.transform.right * -1f, out HitWall, 2f, AvailableLayers)) || Physics.Raycast(base.transform.position, base.transform.up, out HitWall, 1f, AvailableLayers))
			{
				if (!hasBumped)
				{
					PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 0f, PlayerPhysics.velocity.z);
					VerticalSpeed = 0f;
					hasBumped = true;
				}
			}
			else
			{
				hasBumped = false;
			}
		}
		else
		{
			PlayerPhysics.velocity = base.transform.up * VerticalSpeed;
			VerticalSpeed -= Time.deltaTime * 6f;
			if (!Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 0.6f, base.transform.position.z - 0.15f), base.transform.forward, out HitCheck, 2f, AvailableLayers) && !Physics.Raycast(base.transform.position, base.transform.up, out HitCheck, 1.15f, AvailableLayers) && !Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 1.4f, base.transform.position.z), base.transform.forward, out HitCheck, 1.15f, AvailableLayers) && Stable)
			{
				CharacterController.Clamber();
				CharacterController.WallBump(AddForce: true);
			}
			if ((PlayerPhysics.velocity.y < -0.1f || Physics.Raycast(base.transform.position, base.transform.up * -1f, out HitCheck, 1.15f, AvailableLayers) || Physics.Raycast(base.transform.position, base.transform.up, out HitCheck, 1.15f, AvailableLayers)) && Stable && !hasJumped)
			{
				CharacterController.WallJump(HorizontalWall, RightWall);
				hasJumped = true;
			}
		}
	}

	private IEnumerator WallStability()
	{
		yield return new WaitForSeconds(0.15f);
		Stable = true;
		yield return new WaitForSeconds(0.5f);
		VerticalStable = true;
	}

	private IEnumerator BumpPause()
	{
		if (!Bumping)
		{
			Bumping = true;
			PhysicsSave = PlayerPhysics.velocity;
			PlayerPhysics.velocity = Vector3.zero;
			yield return new WaitForSeconds(0.05f);
			PlayerPhysics.velocity = PhysicsSave;
			yield return new WaitForSeconds(0.015f);
			CharacterController.WallBump(AddForce: true);
		}
	}
}
