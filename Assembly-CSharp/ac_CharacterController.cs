using EZCameraShake;
using System.Collections;
using UnityEngine;

public class ac_CharacterController : MonoBehaviour
{
	[Header("Controller and Management")]
	public bool ShowManagement;

	[ConditionalField("ShowManagement", null)]
	public LayerMask AvailableLayers;

	[ConditionalField("ShowManagement", null)]
	public GameManager GM;

	[ConditionalField("ShowManagement", null)]
	public GameObject Player;

	[ConditionalField("ShowManagement", null)]
	public GameObject WallSpawn;

	[ConditionalField("ShowManagement", null)]
	public GameObject CamParent;

	[ConditionalField("ShowManagement", null)]
	public GameObject MainCamera;

	[ConditionalField("ShowManagement", null)]
	public GameObject WeaponCamera;

	[ConditionalField("ShowManagement", null)]
	public RaycastAim AimScript;

	[ConditionalField("ShowManagement", null)]
	public Animator WeaponParent;

	[ConditionalField("ShowManagement", null)]
	public float Sensitivity = 2f;

	[ConditionalField("ShowManagement", null)]
	public bool InvertControls;

	[ConditionalField("ShowManagement", null)]
	public float ControllerAcceleration = 0.1f;

	[ConditionalField("ShowManagement", null)]
	public bool KeyboardMouse = true;

	[ConditionalField("ShowManagement", null)]
	public bool ToggleCrouch = true;

	[Header("Abilities and Set-Up")]
	public bool ShowAbilities;

	[ConditionalField("ShowAbilities", null)]
	public bool CanWallRun;

	[ConditionalField("ShowAbilities", null)]
	public bool CanVault;

	[ConditionalField("ShowAbilities", null)]
	public bool CanDoubleJump;

	[ConditionalField("ShowAbilities", null)]
	public bool CanWallKick;

	[ConditionalField("ShowAbilities", null)]
	public bool CanSlide;

	[ConditionalField("ShowAbilities", null)]
	public bool CanDash;

	[ConditionalField("ShowAbilities", null)]
	public bool CanSwim;

	[Header("Footsteps and Audio")]
	public bool ShowAudio;

	[ConditionalField("ShowAudio", null)]
	public GameObject Footstep;

	[ConditionalField("ShowAudio", null)]
	public GameObject Crouch;

	[ConditionalField("ShowAudio", null)]
	public GameObject Jump;

	[ConditionalField("ShowAudio", null)]
	public GameObject DoubleJump;

	[ConditionalField("ShowAudio", null)]
	public GameObject LandSound;

	[ConditionalField("ShowAudio", null)]
	public GameObject Dash;

	[ConditionalField("ShowAudio", null)]
	public GameObject VaultSound;

	[ConditionalField("ShowAudio", null)]
	public GameObject InWater;

	[ConditionalField("ShowAudio", null)]
	public GameObject OutWater;

	[Header("Physics")]
	public bool ShowPhysics;

	[ConditionalField("ShowPhysics", null)]
	public float Speed = 5f;

	[ConditionalField("ShowPhysics", null)]
	public float CrouchSpeed = 0.75f;

	[ConditionalField("ShowPhysics", null)]
	public float SwimSpeed = 5f;

	[ConditionalField("ShowPhysics", null)]
	public float Gravity;

	[ConditionalField("ShowPhysics", null)]
	public float MotorAcceleration = 10f;

	[ConditionalField("ShowPhysics", null)]
	public float MotorDeceleration = 0.8f;

	[ConditionalField("ShowPhysics", null)]
	public float SwimAcceleration = 10f;

	[ConditionalField("ShowPhysics", null)]
	public float SwimDeceleration = 0.8f;

	[ConditionalField("ShowPhysics", null)]
	public float DashDistance = 35f;

	[ConditionalField("ShowPhysics", null)]
	public Transform PhysicsHook;

	[Header("End Visible Variables")]
	[HideInInspector]
	public float TransferSpeedAmount;

	[HideInInspector]
	public ac_ParkourController ParkourPlayer;

	[HideInInspector]
	public bool isFalling;

	[HideInInspector]
	public bool inAir;

	[HideInInspector]
	public bool isCrouching;

	[HideInInspector]
	public bool isSliding;

	[HideInInspector]
	public bool CrouchGround;

	[HideInInspector]
	public bool AimAssist = true;

	[HideInInspector]
	public CrosshairScript Crosshair;

	[HideInInspector]
	public Transform RevertPoint;

	[HideInInspector]
	public Transform Parent;

	[HideInInspector]
	public bool OnWall;

	[HideInInspector]
	public bool isFrozen;

	[HideInInspector]
	public bool TreadingWater;

	[HideInInspector]
	public bool Swimming;

	private bool CanJump = true;

	private bool canDoubleJump;

	private bool OutOfJumps;

	private bool DashFixOn;

	private InventoryScript Inventory;

	private GTTODManager PointsManager;

	private MenuScript PauseMenu;

	private Rigidbody PlayerPhysics;

	private CapsuleCollider PlayerCollider;

	private PhysicMaterial PlayerMaterial;

	private CameraBob CamBob;

	private bool LockedScreen;

	private bool grounded;

	private bool isWalking;

	private bool CanRePosition = true;

	private bool HasCheckedAngle;

	private bool isWallRunning;

	private bool canStartWallRun = true;

	private bool canVault = true;

	private bool canDash = true;

	private bool DashCool = true;

	private bool HasDashDirection;

	private bool Dashing;

	private bool canSlide = true;

	private bool WasCrouched;

	private bool isGrappling;

	private bool canWallKick = true;

	private bool ForwardsBackwardsKey;

	private bool LeftRightKey;

	private bool Both;

	private bool JumpProtection;

	private bool RightWall;

	private bool LeftWall;

	private bool NearGround;

	private bool SwimCooling = true;

	private int Invert = 1;

	private float XAccelerationTime;

	private float YAccelerationTime;

	private float SlideForce;

	private float DualCrouchDividend = 1f;

	private float GroundTime = 0.2f;

	private float CameraTilt;

	private float TiltSpeed = 90f;

	private float TimeToShake = 0.1f;

	private float m_StepCycle;

	private float m_NextStep;

	private float ForwardBack;

	private float LeftRight;

	private float ColliderRadius = 0.65f;

	private float ColliderHeight = 2f;

	private float rotationX;

	private float rotationY;

	private float MaxYRotation = 90f;

	private float PreviousSensitivity;

	private float AirTime = 0.15f;

	private float PlayerVelocity;

	private float BulletRadius;

	private float ForwardWallAngle = -25f;

	private float XAccel;

	private float YAccel;

	private float JumpCooldown = 0.15f;

	private float AngleSave;

	private float AngleResetTime = 1f;

	private float ForwardVelocity;

	private float DashForce = 500f;

	private float CurrentForwardSwimSpeed;

	private float CurrentHorizontalSwimSpeed;

	private float CurrentVerticalSwimSpeed;

	private float ForwardDirection;

	private float HorizontalDirection;

	private float VerticalDirection;

	private RaycastHit HitR;

	private RaycastHit HitL;

	private RaycastHit HitD;

	private RaycastHit HitU;

	private RaycastHit HitF;

	private RaycastHit HitClamber;

	private RaycastHit HitB;

	private RaycastHit HitAngle;

	private RaycastHit DashCheck;

	private Transform DashTransform;

	private Transform DashDestination;

	private Transform FinalDashPosition;

	private Transform FinalDirection;

	private Transform WaterEffect;

	private Vector3 relative;

	private Vector3 CrouchRelative;

	private Vector3 CrouchPosition;

	private Vector3 LastPosition;

	private Vector3 SavePosition;

	private Vector2 AngleRange;

	public void Awake()
	{
		Inventory = GetComponent<InventoryScript>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		PlayerCollider = GetComponent<CapsuleCollider>();
		PlayerPhysics = GetComponent<Rigidbody>();
		PlayerMaterial = PlayerCollider.material;
		Crosshair = GetComponent<CrosshairScript>();
		AimScript = MainCamera.GetComponent<RaycastAim>();
		CamBob = CamParent.GetComponent<CameraBob>();
		Swimming = false;
		m_StepCycle = 0f;
		m_NextStep = m_StepCycle / 2f;
		grounded = true;
		Parent = base.transform.parent;
		RevertPoint = new GameObject("RevertPoint").transform;
		RevertPoint.parent = Parent;
		FinalDirection = new GameObject("FinalDirection").transform;
		FinalDirection.transform.position = base.transform.position;
		FinalDirection.parent = base.transform;
		DashTransform = new GameObject("DashTransform").transform;
		DashTransform.transform.position = base.transform.position;
		DashTransform.parent = base.transform;
		SavePosition = DashTransform.localPosition;
		DashDestination = new GameObject("DashDestination").transform;
		DashDestination.transform.position = base.transform.position;
		DashDestination.parent = base.transform;
		FixDash();
		PointsManager = Parent.GetComponent<GTTODManager>();
		PauseMenu = GM.gameObject.GetComponent<MenuScript>();
		PauseMenu.CanPauseGame = true;
		PauseMenu.RefreshProcessingTimer();
	}

	public void FixedUpdate()
	{
		if (!grounded && !Swimming)
		{
			PlayerPhysics.AddForce((0f - Gravity) * base.transform.up);
		}
	}

	public void Update()
	{
		if (!isFrozen)
		{
			if (!Swimming)
			{
				if (CanSlide)
				{
					UpdateCrouching();
					UpdateColliderSlide();
				}
				if (CanWallRun)
				{
					UpdateCameraTilt();
				}
				UpdateJumping();
				if (CanDash)
				{
					if (!DashFixOn)
					{
						if (!KeyBindingManager.GetKey(KeyAction.Forward) && !KeyBindingManager.GetKey(KeyAction.Backward) && !KeyBindingManager.GetKey(KeyAction.Left) && !KeyBindingManager.GetKey(KeyAction.Right))
						{
							DashFixOn = true;
						}
						if (KeyBindingManager.GetKeyDown(KeyAction.Dash))
						{
							Inventory.PrintTutorialMessage("Hey, let go of your movement keys for a second. There's a bug with it and this is the only way I can get it to stop.");
						}
					}
					else
					{
						UpdateDashing();
					}
				}
			}
			else if (CanSwim)
			{
				UpdateSwimming();
			}
			UpdatePhysics();
		}
		UpdateCommands();
		UpdateMouseLook();
		LastPosition = base.transform.position;
		if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z + 0.25f), base.transform.up * -1f, out HitD, 1.5f, AvailableLayers) && !isSliding && !isCrouching && !Swimming)
		{
			canDash = true;
			CanJump = true;
			GroundTime -= Time.deltaTime;
			AirTime = 0.15f;
			ForwardWallAngle = -25f;
			isFalling = false;
			base.transform.position = new Vector3(base.transform.position.x, HitD.point.y + 1.35f, base.transform.position.z);
			if (Vector3.Angle(HitD.normal, base.transform.up * -1f) <= 165f)
			{
				inAir = false;
				if ((!isWalking && !KeyBindingManager.GetKey(KeyAction.Jump)) || (!isWalking && Input.GetButton("A") && !KeyboardMouse))
				{
					PlayerPhysics.velocity = Vector3.zero;
				}
			}
			if (GroundTime <= 0f && !grounded)
			{
				grounded = true;
				isFalling = false;
				WasCrouched = false;
			}
			if (inAir)
			{
				inAir = false;
				if (PlayerPhysics.velocity.y < -5f && !isFrozen)
				{
					if (PlayerPhysics.velocity.y < -20f)
					{
						Object.Instantiate(LandSound, new Vector3(base.transform.position.x, base.transform.position.y - 1f, base.transform.position.z), base.transform.rotation);
						MovementShake();
						Inventory.BumpWeapons();
						if (!isCrouching)
						{
							CamBob.CameraBump(-1.2f, 0.5f);
						}
					}
					else
					{
						Object.Instantiate(LandSound, base.transform.position, base.transform.rotation);
						MovementShake();
						Inventory.BumpWeapons();
						if (!isCrouching)
						{
							CamBob.CameraBump(-0.6f, 0.25f);
						}
					}
				}
			}
		}
		else
		{
			if ((KeyBindingManager.GetKey(KeyAction.Forward) && KeyboardMouse) || (Input.GetAxis("Vertical") < 0f && !KeyboardMouse))
			{
				if (PlayerPhysics.velocity.magnitude >= 5f)
				{
					if (CanWallRun)
					{
						UpdateWallRun();
					}
					if (CanVault && !Swimming)
					{
						UpdateClamber();
					}
				}
			}
			else if (((Physics.Raycast(base.transform.position, base.transform.right, out HitR, 1f, AvailableLayers) && !isWallRunning && HitR.transform.tag == "Wall") || (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitL, 1f, AvailableLayers) && !isWallRunning && HitL.transform.tag == "Wall")) && (KeyBindingManager.GetKey(KeyAction.Jump) || Input.GetButton("A")) && canWallKick && CanWallKick && AirTime <= 0f)
			{
				StartCoroutine(WallKick());
			}
			if (!Swimming)
			{
				UpdateAirControl();
			}
			GroundTime = 0.2f;
			grounded = false;
			inAir = true;
			CanJump = false;
			if (!CrouchGround || isSliding)
			{
				WeaponParent.SetTrigger("Idle");
			}
		}
		if (!grounded || isCrouching || Swimming)
		{
			return;
		}
		relative = Player.transform.InverseTransformDirection(PlayerPhysics.velocity);
		if (KeyboardMouse)
		{
			if (grounded)
			{
				if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward))
				{
					bool flag = false;
					bool key = KeyBindingManager.GetKey(KeyAction.Forward);
					bool key2 = KeyBindingManager.GetKey(KeyAction.Backward);
					if (key2 && key)
					{
						flag = true;
					}
					if (!flag && key)
					{
						if (relative.z <= Speed)
						{
							PlayerPhysics.AddForce(Player.transform.forward * MotorAcceleration);
							isWalking = true;
							WeaponParent.SetTrigger("Walk");
						}
					}
					else if (key2 && relative.z > 0f - Speed)
					{
						PlayerPhysics.AddForce(-Player.transform.forward * MotorAcceleration);
						isWalking = true;
						WeaponParent.SetTrigger("Walk");
					}
				}
				else
				{
					relative = new Vector3(relative.x, relative.y, relative.z * MotorDeceleration);
					PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
					isWalking = false;
					WeaponParent.SetTrigger("Idle");
				}
			}
			if (KeyBindingManager.GetKey(KeyAction.Right) || KeyBindingManager.GetKey(KeyAction.Left))
			{
				bool flag2 = false;
				bool key3 = KeyBindingManager.GetKey(KeyAction.Right);
				bool key4 = KeyBindingManager.GetKey(KeyAction.Left);
				if (key3 && key4)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					if (key3)
					{
						if (relative.x <= Speed / 1.5f)
						{
							PlayerPhysics.AddForce(Player.transform.right * MotorAcceleration);
						}
					}
					else if (key4 && relative.x > (0f - Speed) / 1.5f)
					{
						PlayerPhysics.AddForce(-Player.transform.right * MotorAcceleration);
					}
				}
				else
				{
					relative = new Vector3(relative.x * MotorDeceleration, relative.y, relative.z);
					PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
				}
			}
			else
			{
				relative = new Vector3(relative.x * MotorDeceleration, relative.y, relative.z);
				PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
			}
			if (grounded)
			{
				if (relative.x > Speed)
				{
					relative.x = Speed;
				}
				else if (0f - relative.x > Speed)
				{
					relative.x = 0f - Speed;
				}
				if (relative.z > Speed)
				{
					relative.z = Speed;
				}
				else if (0f - relative.z > Speed)
				{
					relative.z = 0f - Speed;
				}
				PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
			}
			return;
		}
		ForwardBack = Input.GetAxis("Vertical") * (0f - MotorAcceleration);
		LeftRight = Input.GetAxis("Horizontal") * MotorAcceleration;
		if (!grounded)
		{
			return;
		}
		if (ForwardBack != 0f || LeftRight != 0f)
		{
			isWalking = true;
			WeaponParent.SetTrigger("Walk");
			if (ForwardBack != 0f)
			{
				if (relative.z <= Speed || relative.z > 0f - Speed)
				{
					PlayerPhysics.AddForce(Player.transform.forward * ForwardBack);
				}
			}
			else
			{
				relative = new Vector3(relative.x, relative.y, relative.z * MotorDeceleration);
				PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
			}
			if (LeftRight != 0f)
			{
				if (relative.x <= Speed / 1.5f || relative.x > (0f - Speed) / 1.5f)
				{
					PlayerPhysics.AddForce(Player.transform.right * LeftRight);
				}
			}
			else
			{
				relative = new Vector3(relative.x * MotorDeceleration, relative.y, relative.z);
				PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
			}
			if (relative.x > Speed)
			{
				relative.x = Speed;
			}
			else if (0f - relative.x > Speed)
			{
				relative.x = 0f - Speed;
			}
			if (relative.z > Speed)
			{
				relative.z = Speed;
			}
			else if (0f - relative.z > Speed)
			{
				relative.z = 0f - Speed;
			}
			PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
		}
		else
		{
			isWalking = false;
			WeaponParent.SetTrigger("Idle");
			relative = new Vector3(relative.x * MotorDeceleration, relative.y, relative.z * MotorDeceleration);
			PlayerPhysics.velocity = Player.transform.TransformDirection(relative);
		}
	}

	private void UpdateJumping()
	{
		if (JumpCooldown > 0f)
		{
			JumpCooldown -= Time.deltaTime;
		}
		if (KeyBindingManager.GetKey(KeyAction.Jump) || (Input.GetButton("A") && !KeyboardMouse))
		{
			if (!isWallRunning && !Swimming && JumpCooldown <= 0f)
			{
				if (CanJump && !JumpProtection && !isCrouching)
				{
					StartJump();
				}
				if (canDoubleJump && !NearGround && !CanJump)
				{
					StartDoubleJump();
				}
				if (Physics.Raycast(CrouchPosition, base.transform.up * -1f, out HitD, 1.15f, AvailableLayers) && isCrouching)
				{
					StartJump();
					OutOfJumps = false;
					canDash = true;
					AirTime = 0.15f;
				}
			}
		}
		else
		{
			if (!inAir)
			{
				JumpProtection = false;
				CanJump = true;
				canDoubleJump = false;
				OutOfJumps = false;
			}
			if (!grounded && !OutOfJumps && !CanJump && !isSliding && CanDoubleJump)
			{
				canDoubleJump = true;
			}
		}
	}

	private void StartJump()
	{
		JumpCooldown = 0.2f;
		grounded = false;
		CanJump = false;
		JumpProtection = true;
		Object.Instantiate(Jump, base.transform.position, base.transform.rotation);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.25f, base.transform.position.z);
		PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 10f, PlayerPhysics.velocity.z);
		Inventory.BumpWeapons();
		if (isCrouching)
		{
			PlayerUnCrouch();
			CanRePosition = true;
		}
	}

	private void StartDoubleJump()
	{
		JumpCooldown = 0.2f;
		canDoubleJump = false;
		OutOfJumps = true;
		Object.Instantiate(DoubleJump, base.transform.position, base.transform.rotation);
		MovementShake();
		Inventory.BumpWeapons();
		Vector3 vector = Vector3.zero;
		if (KeyboardMouse)
		{
			vector.z = (KeyBindingManager.GetKey(KeyAction.Forward) ? 1 : 0) - (KeyBindingManager.GetKey(KeyAction.Backward) ? 1 : 0);
			vector.x = (KeyBindingManager.GetKey(KeyAction.Right) ? 1 : 0) - (KeyBindingManager.GetKey(KeyAction.Left) ? 1 : 0);
		}
		else
		{
			ForwardBack = Input.GetAxis("Vertical") * (0f - MotorAcceleration);
			LeftRight = Input.GetAxis("Horizontal") * MotorAcceleration;
			vector.z = ((ForwardBack >= 0f) ? 1 : 0) - ((ForwardBack <= 0f) ? 1 : 0);
			vector.x = ((LeftRight >= 0f) ? 1 : 0) - ((LeftRight <= 0f) ? 1 : 0);
		}
		vector = base.transform.TransformDirection(vector);
		Vector3 velocity = PlayerPhysics.velocity;
		velocity.y = 0f;
		if (Vector3.Angle(velocity, vector) >= 90f)
		{
			PlayerPhysics.velocity = new Vector3(0f, PlayerPhysics.velocity.y, 0f);
		}
		if (PlayerPhysics.velocity.y > 10f)
		{
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, PlayerPhysics.velocity.y + 12.5f, PlayerPhysics.velocity.z);
		}
		else
		{
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 12.5f, PlayerPhysics.velocity.z);
		}
	}

	private IEnumerator WallKick()
	{
		JumpCooldown = 0.2f;
		PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 0f, PlayerPhysics.velocity.z);
		Object.Instantiate(Jump, base.transform.position, base.transform.rotation);
		canWallKick = false;
		PlayerPhysics.AddForce(base.transform.up * 350f);
		PlayerPhysics.AddForce(base.transform.forward * 350f);
		MovementShake();
		ResetAbilities();
		Inventory.BumpWeapons();
		if (Physics.Raycast(base.transform.position, base.transform.right, out HitR, 1f, AvailableLayers))
		{
			PlayerPhysics.AddForce(base.transform.right * -250f);
		}
		if (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitL, 1f, AvailableLayers))
		{
			PlayerPhysics.AddForce(base.transform.right * 250f);
		}
		yield return new WaitForSeconds(0.35f);
		canWallKick = true;
	}

	public void UpdateClamber()
	{
		if (isGrappling)
		{
			return;
		}
		if ((Physics.Raycast(base.transform.position, base.transform.forward, out HitF, 1f, AvailableLayers) && !isWallRunning && HitF.transform.tag == "Wall") || (Physics.Raycast(base.transform.position, base.transform.forward, out HitF, 1f, AvailableLayers) && !isWallRunning && HitF.transform.tag == "Wall"))
		{
			if (!HasCheckedAngle)
			{
				Transform transform = Object.Instantiate(new GameObject("WallAngle"), HitF.point, Quaternion.LookRotation(HitF.normal)).transform;
				Object.Destroy(transform.gameObject, 1f);
				HasCheckedAngle = true;
				if ((Vector3.SignedAngle(base.transform.forward, transform.transform.forward, Vector3.up) > 150f || Vector3.SignedAngle(base.transform.forward, transform.transform.forward, Vector3.up) < -150f) && (Vector3.Angle(HitF.normal, base.transform.parent.forward) < ForwardWallAngle - 10f || Vector3.Angle(HitF.normal, base.transform.parent.forward) > ForwardWallAngle + 10f))
				{
					ForwardWallAngle = Vector3.Angle(HitF.normal, base.transform.parent.forward);
					ParkourPlayer = Object.Instantiate(WallSpawn, HitF.point + HitF.normal * 0.35f, Quaternion.LookRotation(HitF.normal)).GetComponent<ac_ParkourController>();
					ParkourPlayer.gameObject.transform.parent = base.transform.parent;
					ParkourPlayer.StartWallRun(Horizontal: false, Right: false);
					WallRun(Horizontal: false, RightWall: false);
					isWallRunning = true;
				}
			}
		}
		else
		{
			HasCheckedAngle = false;
		}
		if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y - 1f, base.transform.position.z), base.transform.forward, out HitClamber, 1.5f, AvailableLayers) && !Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z), base.transform.forward, out HitF, 1.5f, AvailableLayers) && HitClamber.collider.gameObject.tag == "Wall" && !isWallRunning && !isSliding)
		{
			if (!HasCheckedAngle)
			{
				Transform transform2 = Object.Instantiate(new GameObject("WallAngle"), HitClamber.point, Quaternion.LookRotation(HitClamber.normal)).transform;
				Object.Destroy(transform2.gameObject, 1f);
				HasCheckedAngle = true;
				if (Vector3.SignedAngle(base.transform.forward, transform2.transform.forward, Vector3.up) > 150f || Vector3.SignedAngle(base.transform.forward, transform2.transform.forward, Vector3.up) < -150f)
				{
					StartCoroutine(BasicJumpObstacle(5f, new Vector3(0f, 2.5f, 0f)));
				}
			}
		}
		else
		{
			HasCheckedAngle = false;
		}
	}

	public void Clamber()
	{
		StartCoroutine(JumpObstacle(5f, new Vector3(0f, 2.5f, 0f)));
	}

	public IEnumerator JumpObstacle(float speed, Vector3 endPosition)
	{
		if (canVault)
		{
			canVault = false;
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 0f, PlayerPhysics.velocity.z);
			MovementShake();
			Object.Instantiate(VaultSound, base.transform.position, base.transform.rotation);
			Inventory.Vault();
			Vector3 start = base.transform.position;
			Vector3 end = base.transform.position + base.transform.up * endPosition.y + base.transform.forward * endPosition.z;
			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime * speed;
				base.transform.position = Vector3.Lerp(start, end, t);
				yield return null;
			}
			PlayerPhysics.velocity += base.transform.forward * 5f;
			yield return new WaitForSeconds(0.35f);
			canVault = true;
		}
	}

	public IEnumerator BasicJumpObstacle(float speed, Vector3 endPosition)
	{
		if (canVault)
		{
			canVault = false;
			PlayerPhysics.velocity = new Vector3(0f, 0f, 0f);
			MovementShake();
			Object.Instantiate(VaultSound, base.transform.position, base.transform.rotation);
			WeaponParent.SetTrigger("WallBump");
			Inventory.OnWall();
			Vector3 start = base.transform.position;
			Vector3 end = base.transform.position + base.transform.up * endPosition.y + base.transform.forward * endPosition.z;
			float t = 0f;
			while (t < 1f)
			{
				t += Time.deltaTime * speed;
				base.transform.position = Vector3.Lerp(start, end, t);
				yield return null;
			}
			Inventory.OffWall();
			PlayerPhysics.velocity += base.transform.forward * 5f;
			yield return new WaitForSeconds(0.35f);
			canVault = true;
		}
	}

	public IEnumerator ClamberWaitTime()
	{
		yield return new WaitForSeconds(1f);
		canVault = true;
	}

	private void UpdateDashing()
	{
		if ((KeyBindingManager.GetKey(KeyAction.Dash) || (Input.GetButton("LeftClick") && !KeyboardMouse)) && canDash && DashCool && HasDashDirection && !Dashing)
		{
			FinalDashPosition = new GameObject("FinalDashPosition").transform;
			FinalDashPosition.position = DashDestination.position;
			canDash = false;
			DashCool = false;
			Dashing = true;
			DashForce = ForwardVelocity;
			PlayerPhysics.velocity = Vector3.zero;
			Blur();
			WeaponParent.SetTrigger("Dash");
			WallRunShake();
			Object.Instantiate(Dash, base.transform.position, base.transform.rotation);
		}
		if (Dashing && FinalDashPosition != null)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, FinalDashPosition.position, 0.85f);
			if (Vector3.Distance(base.transform.position, FinalDashPosition.position) < 5f)
			{
				PlayerPhysics.isKinematic = false;
				StartCoroutine(DashCooldown());
				Object.Destroy(FinalDashPosition.gameObject);
				if (DashForce < 15f)
				{
					PlayerPhysics.velocity = FinalDirection.forward * 15f;
				}
				else
				{
					PlayerPhysics.velocity = FinalDirection.forward * DashForce;
				}
				Dashing = false;
			}
		}
		if (KeyboardMouse)
		{
			if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward) || KeyBindingManager.GetKey(KeyAction.Left) || KeyBindingManager.GetKey(KeyAction.Right))
			{
				HasDashDirection = true;
				if (KeyBindingManager.GetKeyDown(KeyAction.Forward) || KeyBindingManager.GetKeyUp(KeyAction.Backward))
				{
					DashTransform.position += base.transform.forward * DashDistance;
				}
				if (KeyBindingManager.GetKeyDown(KeyAction.Backward) || KeyBindingManager.GetKeyUp(KeyAction.Forward))
				{
					DashTransform.position += base.transform.forward * (0f - DashDistance);
				}
				if (KeyBindingManager.GetKeyDown(KeyAction.Right) || KeyBindingManager.GetKeyUp(KeyAction.Left))
				{
					DashTransform.position += base.transform.right * DashDistance;
				}
				if (KeyBindingManager.GetKeyDown(KeyAction.Left) || KeyBindingManager.GetKeyUp(KeyAction.Right))
				{
					DashTransform.position += base.transform.right * (0f - DashDistance);
				}
			}
			else
			{
				HasDashDirection = false;
				DashTransform.localPosition = SavePosition;
				DashDestination.position = SavePosition;
			}
			if (HasDashDirection)
			{
				FinalDirection.LookAt(DashTransform.position);
				if (Physics.Raycast(base.transform.position, FinalDirection.forward, out DashCheck, DashDistance + 5f, AvailableLayers))
				{
					DashDestination.position = DashCheck.point;
				}
				else
				{
					DashDestination.position = DashTransform.position;
				}
			}
			return;
		}
		ForwardBack = Input.GetAxis("Vertical");
		LeftRight = Input.GetAxis("Horizontal");
		if (ForwardBack != 0f || LeftRight != 0f)
		{
			HasDashDirection = true;
			DashTransform.position = base.transform.position + base.transform.right * DashDistance * LeftRight + base.transform.forward * DashDistance * (0f - ForwardBack);
		}
		else
		{
			HasDashDirection = false;
			DashTransform.localPosition = SavePosition;
			DashDestination.position = SavePosition;
		}
		if (HasDashDirection)
		{
			FinalDirection.LookAt(DashTransform.position);
			if (Physics.Raycast(base.transform.position, FinalDirection.forward, out DashCheck, DashDistance + 5f, AvailableLayers))
			{
				DashDestination.position = DashCheck.point;
			}
			else
			{
				DashDestination.position = DashTransform.position;
			}
		}
	}

	private IEnumerator DashCooldown()
	{
		OutOfJumps = false;
		yield return new WaitForSeconds(0.65f);
		DashCool = true;
	}

	public void StopDashing()
	{
		Dashing = false;
		DashCool = true;
	}

	public void FixDash()
	{
		HasDashDirection = false;
		DashTransform.localPosition = SavePosition;
		DashDestination.position = SavePosition;
	}

	public void PreventDashing()
	{
		canDash = false;
	}

	private void UpdateCrouching()
	{
		CrouchPosition = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
		SlideForce = base.transform.InverseTransformDirection(PlayerPhysics.velocity).z * 15f;
		if (ToggleCrouch)
		{
			if (KeyBindingManager.GetKey(KeyAction.Crouch) || (Input.GetButton("B") && !KeyboardMouse))
			{
				if (CanRePosition)
				{
					if (!isCrouching)
					{
						PlayerCrouch();
						if (grounded && canSlide && !isSliding && PlayerPhysics.velocity.magnitude >= 8f)
						{
							StartGroundSlide();
						}
					}
					else if (!Physics.Raycast(base.transform.position, base.transform.up, out HitD, 0.5f, AvailableLayers) && !Physics.Raycast(base.transform.position + base.transform.right * 0.25f, base.transform.up, out HitD, 0.5f, AvailableLayers) && !Physics.Raycast(base.transform.position - base.transform.right * 0.25f, base.transform.up, out HitD, 0.5f, AvailableLayers))
					{
						PlayerUnCrouch();
					}
				}
			}
			else if (!CanRePosition)
			{
				CanRePosition = true;
			}
		}
		else if (KeyBindingManager.GetKey(KeyAction.Crouch) || (Input.GetButton("B") && !KeyboardMouse))
		{
			if (!isCrouching)
			{
				PlayerCrouch();
				if (grounded && canSlide && !isSliding && PlayerPhysics.velocity.magnitude >= 8f)
				{
					StartGroundSlide();
				}
			}
		}
		else if (isCrouching)
		{
			PlayerUnCrouch();
		}
		if (Physics.Raycast(CrouchPosition, base.transform.up * -1f, out HitD, 0.75f, AvailableLayers) && isCrouching && inAir)
		{
			NearGround = true;
			if (!KeyBindingManager.GetKey(KeyAction.Jump) && PlayerPhysics.velocity.magnitude >= 9f && !isSliding)
			{
				StartGroundSlide();
				Inventory.BumpWeapons();
			}
		}
		else if (NearGround)
		{
			NearGround = false;
			ResetAbilities();
			canDoubleJump = true;
		}
		if (isSliding)
		{
			TimeToShake -= Time.deltaTime;
			if (TimeToShake <= 0f && PlayerPhysics.velocity.magnitude > 5f)
			{
				SlideShake();
			}
			if (PlayerPhysics.velocity.magnitude <= 5f)
			{
				isSliding = false;
			}
		}
		else if (Physics.Raycast(base.transform.position, base.transform.up * -1f, out HitD, 1f, AvailableLayers))
		{
			CrouchGround = true;
			UpdateCrouchWalking();
			WeaponParent.speed = 0.75f;
		}
		else
		{
			CrouchGround = false;
			WeaponParent.speed = 1f;
		}
	}

	private void UpdateCrouchWalking()
	{
		if (!CrouchGround)
		{
			return;
		}
		CrouchRelative = Player.transform.InverseTransformDirection(PlayerPhysics.velocity / DualCrouchDividend);
		if (!KeyboardMouse)
		{
			return;
		}
		if (CrouchGround)
		{
			if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward))
			{
				bool flag = false;
				bool key = KeyBindingManager.GetKey(KeyAction.Forward);
				bool key2 = KeyBindingManager.GetKey(KeyAction.Backward);
				if (key2 && key)
				{
					flag = true;
				}
				if (!flag && key)
				{
					if (CrouchRelative.z <= CrouchSpeed)
					{
						PlayerPhysics.AddForce(Player.transform.forward * MotorAcceleration);
						isWalking = true;
						WeaponParent.SetTrigger("Walk");
					}
				}
				else if (key2 && CrouchRelative.z > 0f - CrouchSpeed)
				{
					PlayerPhysics.AddForce(-Player.transform.forward * MotorAcceleration);
					isWalking = true;
					WeaponParent.SetTrigger("Walk");
				}
			}
			else
			{
				CrouchRelative = new Vector3(CrouchRelative.x, CrouchRelative.y, CrouchRelative.z * MotorDeceleration);
				PlayerPhysics.velocity = Player.transform.TransformDirection(CrouchRelative);
				isWalking = false;
				WeaponParent.SetTrigger("Idle");
			}
		}
		if (KeyBindingManager.GetKey(KeyAction.Right) || KeyBindingManager.GetKey(KeyAction.Left))
		{
			bool flag2 = false;
			bool key3 = KeyBindingManager.GetKey(KeyAction.Right);
			bool key4 = KeyBindingManager.GetKey(KeyAction.Left);
			if (key3 && key4)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				if (key3)
				{
					if (CrouchRelative.x <= CrouchSpeed)
					{
						PlayerPhysics.AddForce(Player.transform.right * MotorAcceleration);
					}
				}
				else if (key4 && CrouchRelative.x > 0f - CrouchSpeed)
				{
					PlayerPhysics.AddForce(-Player.transform.right * MotorAcceleration);
				}
			}
			else
			{
				CrouchRelative = new Vector3(CrouchRelative.x * MotorDeceleration, CrouchRelative.y, CrouchRelative.z);
				PlayerPhysics.velocity = Player.transform.TransformDirection(CrouchRelative);
			}
		}
		else
		{
			CrouchRelative = new Vector3(CrouchRelative.x * MotorDeceleration, CrouchRelative.y, CrouchRelative.z);
			PlayerPhysics.velocity = Player.transform.TransformDirection(CrouchRelative);
		}
		if (CrouchGround)
		{
			if (CrouchRelative.x > CrouchSpeed)
			{
				CrouchRelative.x = CrouchSpeed;
			}
			else if (0f - CrouchRelative.x > CrouchSpeed)
			{
				CrouchRelative.x = 0f - CrouchSpeed;
			}
			if (CrouchRelative.z > CrouchSpeed)
			{
				CrouchRelative.z = CrouchSpeed;
			}
			else if (0f - CrouchRelative.z > CrouchSpeed)
			{
				CrouchRelative.z = 0f - CrouchSpeed;
			}
			PlayerPhysics.velocity = Player.transform.TransformDirection(CrouchRelative);
		}
		if (KeyBindingManager.GetKey(KeyAction.Right) || (KeyBindingManager.GetKey(KeyAction.Left) && KeyBindingManager.GetKey(KeyAction.Forward)) || KeyBindingManager.GetKey(KeyAction.Backward))
		{
			DualCrouchDividend = 2f;
		}
		else
		{
			DualCrouchDividend = 1f;
		}
	}

	public void PlayerCrouch()
	{
		isCrouching = true;
		CanRePosition = false;
		Object.Instantiate(Crouch);
	}

	public void PlayerUnCrouch()
	{
		isCrouching = false;
		WasCrouched = true;
		isSliding = false;
		canSlide = true;
		CanRePosition = false;
		Object.Instantiate(Crouch);
	}

	public void PlayerRePosition()
	{
		CanRePosition = true;
	}

	public void StartGroundSlide()
	{
		isSliding = true;
		inAir = false;
		canSlide = false;
		grounded = false;
		if (PlayerPhysics.velocity.magnitude <= 35f)
		{
			PlayerPhysics.AddForce(base.transform.forward * SlideForce);
		}
		MovementShake();
	}

	private void UpdateColliderSlide()
	{
		PlayerCollider.radius = ColliderRadius;
		PlayerCollider.height = ColliderHeight;
		if (isCrouching)
		{
			ColliderRadius = Mathf.Lerp(ColliderRadius, 0.25f, 0.1f);
			ColliderHeight = Mathf.Lerp(ColliderHeight, 0.5f, 0.1f);
		}
		if (!isCrouching)
		{
			ColliderRadius = Mathf.Lerp(ColliderRadius, 0.6f, 0.1f);
			ColliderHeight = Mathf.Lerp(ColliderHeight, 2f, 0.1f);
		}
	}

	private void SlideShake()
	{
		TimeToShake = Random.Range(0.1f, 0.25f);
		float magnitude = Random.Range(2f, 6f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(3f, 1f, 3f);
		CameraShaker.Instance.ShakeOnce(magnitude, 1f, TimeToShake, 1f);
		CameraShaker.Instance.ResetCamera();
	}

	public void UpdateAirControl()
	{
		if (!isWallRunning && !isSliding && !Physics.Raycast(base.transform.position, base.transform.up * -1f, out HitD, 1f, AvailableLayers))
		{
			Vector3 vector = Vector3.zero;
			if (KeyboardMouse)
			{
				vector.z = (KeyBindingManager.GetKey(KeyAction.Forward) ? 1 : 0) - (KeyBindingManager.GetKey(KeyAction.Backward) ? 1 : 0);
				vector.x = (KeyBindingManager.GetKey(KeyAction.Right) ? 1 : 0) - (KeyBindingManager.GetKey(KeyAction.Left) ? 1 : 0);
			}
			else
			{
				ForwardBack = Input.GetAxis("Vertical") * (0f - MotorAcceleration);
				LeftRight = Input.GetAxis("Horizontal") * MotorAcceleration;
				vector.z = ((ForwardBack >= 0f) ? 1 : 0) - ((ForwardBack <= 0f) ? 1 : 0);
				vector.x = ((LeftRight >= 0f) ? 1 : 0) - ((LeftRight <= 0f) ? 1 : 0);
			}
			vector = base.transform.TransformDirection(vector);
			vector.Normalize();
			float num = vector.magnitude * Speed;
			float wishspeed = num;
			Accelerate(vector, num, 3f);
			AirControl(vector, wishspeed);
		}
	}

	private void AirControl(Vector3 wishdir, float wishspeed)
	{
		if (!((double)Mathf.Abs(wishspeed) < 0.001))
		{
			Vector3 velocity = PlayerPhysics.velocity;
			float y = velocity.y;
			velocity.y = 0f;
			float magnitude = velocity.magnitude;
			velocity.Normalize();
			float num = Vector3.Dot(velocity, wishdir);
			float num2 = 32f;
			num2 *= 1f * num * num * Time.deltaTime;
			if (num > 0f)
			{
				velocity.x = velocity.x * magnitude + wishdir.x * num2;
				velocity.y = velocity.y * magnitude + wishdir.y * num2;
				velocity.z = velocity.z * magnitude + wishdir.z * num2;
				velocity.Normalize();
			}
			velocity.x *= magnitude;
			velocity.y = y;
			velocity.z *= magnitude;
			PlayerPhysics.velocity = velocity;
		}
	}

	private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
	{
		float num = Vector3.Dot(PlayerPhysics.velocity, wishdir);
		float num2 = wishspeed - num;
		if (!(num2 <= 0f))
		{
			float num3 = accel * Time.deltaTime * wishspeed;
			if (num3 > num2)
			{
				num3 = num2;
			}
			PlayerPhysics.velocity += new Vector3(num3 * wishdir.x, 0f, num3 * wishdir.z);
		}
	}

	private void UpdateSwimming()
	{
		SetSwimSpeed();
		if (KeyBindingManager.GetKey(KeyAction.Forward) || KeyBindingManager.GetKey(KeyAction.Backward) || KeyBindingManager.GetKey(KeyAction.Left) || KeyBindingManager.GetKey(KeyAction.Right) || KeyBindingManager.GetKey(KeyAction.Jump) || KeyBindingManager.GetKey(KeyAction.Crouch))
		{
			PlayerPhysics.velocity = MainCamera.transform.forward * CurrentForwardSwimSpeed * ForwardDirection + MainCamera.transform.right * CurrentHorizontalSwimSpeed * HorizontalDirection + MainCamera.transform.up * CurrentVerticalSwimSpeed * VerticalDirection;
			if ((KeyBindingManager.GetKey(KeyAction.Forward) && KeyBindingManager.GetKey(KeyAction.Backward)) || (!KeyBindingManager.GetKey(KeyAction.Forward) && !KeyBindingManager.GetKey(KeyAction.Backward)))
			{
				ForwardDirection = 0f;
				CurrentForwardSwimSpeed = Mathf.Lerp(CurrentForwardSwimSpeed, 0f, 0.15f);
			}
			else
			{
				if (KeyBindingManager.GetKey(KeyAction.Forward))
				{
					ForwardDirection = 1f;
					CurrentForwardSwimSpeed = Mathf.Lerp(CurrentForwardSwimSpeed, SwimSpeed, SwimAcceleration);
					TreadingWater = true;
				}
				if (KeyBindingManager.GetKey(KeyAction.Backward))
				{
					ForwardDirection = -1f;
					CurrentForwardSwimSpeed = Mathf.Lerp(CurrentForwardSwimSpeed, SwimSpeed, SwimAcceleration);
				}
			}
			if ((KeyBindingManager.GetKey(KeyAction.Right) && KeyBindingManager.GetKey(KeyAction.Left)) || (!KeyBindingManager.GetKey(KeyAction.Right) && !KeyBindingManager.GetKey(KeyAction.Left)))
			{
				HorizontalDirection = 0f;
				CurrentHorizontalSwimSpeed = Mathf.Lerp(CurrentHorizontalSwimSpeed, 0f, 0.15f);
			}
			else
			{
				if (KeyBindingManager.GetKey(KeyAction.Right))
				{
					HorizontalDirection = 1f;
					CurrentHorizontalSwimSpeed = Mathf.Lerp(CurrentHorizontalSwimSpeed, SwimSpeed, SwimAcceleration);
				}
				if (KeyBindingManager.GetKey(KeyAction.Left))
				{
					HorizontalDirection = -1f;
					CurrentHorizontalSwimSpeed = Mathf.Lerp(CurrentHorizontalSwimSpeed, SwimSpeed, SwimAcceleration);
				}
			}
			if ((KeyBindingManager.GetKey(KeyAction.Jump) && KeyBindingManager.GetKey(KeyAction.Crouch)) || (!KeyBindingManager.GetKey(KeyAction.Jump) && !KeyBindingManager.GetKey(KeyAction.Crouch)))
			{
				VerticalDirection = 0f;
				CurrentVerticalSwimSpeed = Mathf.Lerp(CurrentVerticalSwimSpeed, 0f, 0.15f);
				return;
			}
			if (KeyBindingManager.GetKey(KeyAction.Jump))
			{
				VerticalDirection = 1f;
				CurrentVerticalSwimSpeed = Mathf.Lerp(CurrentVerticalSwimSpeed, SwimSpeed, SwimAcceleration);
			}
			if (KeyBindingManager.GetKey(KeyAction.Crouch))
			{
				VerticalDirection = -1f;
				CurrentVerticalSwimSpeed = Mathf.Lerp(CurrentVerticalSwimSpeed, SwimSpeed, SwimAcceleration);
			}
		}
		else
		{
			TreadingWater = false;
			CurrentForwardSwimSpeed = Mathf.Lerp(CurrentForwardSwimSpeed, 0f, 0.15f);
			CurrentHorizontalSwimSpeed = Mathf.Lerp(CurrentHorizontalSwimSpeed, 0f, 0.15f);
			CurrentVerticalSwimSpeed = Mathf.Lerp(CurrentVerticalSwimSpeed, 0f, 0.15f);
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x / SwimDeceleration, PlayerPhysics.velocity.y / SwimDeceleration, PlayerPhysics.velocity.z / SwimDeceleration);
		}
	}

	public void SetSwimSpeed()
	{
	}

	public void UpdatePhysics()
	{
		TransferSpeedAmount = base.transform.InverseTransformDirection(PlayerPhysics.velocity).z;
		if (!OnWall)
		{
			ForwardVelocity = base.transform.InverseTransformDirection(PlayerPhysics.velocity).z;
		}
		else
		{
			ForwardVelocity = base.transform.InverseTransformDirection(ParkourPlayer.PlayerPhysics.velocity).z;
		}
		if ((inAir && !isWallRunning) || (isCrouching && !isWallRunning))
		{
			PlayerMaterial.staticFriction = 0.125f;
			PlayerMaterial.dynamicFriction = 0.125f;
		}
		else
		{
			PlayerMaterial.staticFriction = 0.35f;
			PlayerMaterial.dynamicFriction = 0.35f;
		}
		if (KeyboardMouse)
		{
			if (AimAssist)
			{
				Inventory.SetBulletRadius(BulletRadius);
				if (!grounded && !isSliding)
				{
					BulletRadius = PlayerVelocity * 0.1f;
					if (!isWallRunning)
					{
						PlayerVelocity = PlayerPhysics.velocity.magnitude;
					}
					else if (ParkourPlayer != null)
					{
						PlayerVelocity = ParkourPlayer.PlayerPhysics.velocity.magnitude / 2f;
					}
				}
				else
				{
					BulletRadius = 0.25f;
				}
			}
		}
		else
		{
			Inventory.SetBulletRadius(BulletRadius);
			if (!inAir)
			{
				BulletRadius = 1f;
			}
			else
			{
				BulletRadius = 2.25f;
			}
		}
		if (PlayerPhysics.velocity.y < -0.1f)
		{
			isFalling = true;
		}
		else
		{
			isFalling = false;
		}
	}

	public void UpdateWallRun()
	{
		AirTime -= Time.deltaTime;
		if (AirTime <= 0f && TransferSpeedAmount >= 8f && !isGrappling && canStartWallRun && !isSliding && !Swimming)
		{
			if (Physics.Raycast(base.transform.position, base.transform.right, out HitR, 1f, AvailableLayers) && !isWallRunning && HitR.transform.tag == "Wall" && (Vector3.Angle(HitR.normal, base.transform.parent.right) > AngleRange.x || Vector3.Angle(HitR.normal, base.transform.parent.right) < AngleRange.y))
			{
				ParkourPlayer = Object.Instantiate(WallSpawn, HitR.point + HitR.normal * 0.35f, Quaternion.LookRotation(HitR.normal)).GetComponent<ac_ParkourController>();
				ParkourPlayer.transform.parent = base.transform.parent;
				ParkourPlayer.StartWallRun(Horizontal: true, Right: true);
				WallRun(Horizontal: true, RightWall: true);
				isWallRunning = true;
			}
			if (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitL, 1f, AvailableLayers) && !isWallRunning && HitL.transform.tag == "Wall" && (Vector3.Angle(HitR.normal, base.transform.parent.right * -1f) > AngleRange.x || Vector3.Angle(HitR.normal, base.transform.parent.right * -1f) < AngleRange.y))
			{
				ParkourPlayer = Object.Instantiate(WallSpawn, HitL.point + HitL.normal * 0.35f, Quaternion.LookRotation(HitL.normal)).GetComponent<ac_ParkourController>();
				ParkourPlayer.transform.parent = base.transform.parent;
				ParkourPlayer.StartWallRun(Horizontal: true, Right: false);
				WallRun(Horizontal: true, RightWall: false);
				isWallRunning = true;
			}
		}
	}

	public void WallRun(bool Horizontal, bool RightWall)
	{
		AirTime = 0f;
		grounded = false;
		PlayerPhysics.isKinematic = true;
		base.gameObject.GetComponent<CapsuleCollider>().enabled = false;
		Player.transform.position = ParkourPlayer.transform.position;
		Inventory.OnWall();
		WallRunShake();
		canStartWallRun = false;
		WeaponParent.SetTrigger("WallBump");
		OnWall = true;
		if (Horizontal)
		{
			if (RightWall)
			{
				ForwardWallAngle = -25f;
			}
			else
			{
				ForwardWallAngle = -25f;
			}
		}
		if (isCrouching)
		{
			PlayerUnCrouch();
			CanRePosition = true;
		}
	}

	public void WallJump(bool Horizontal, bool Right)
	{
		AirTime = 0.15f;
		canDash = true;
		OnWall = false;
		Object.Instantiate(Jump, base.transform.position, base.transform.rotation);
		WeaponParent.SetTrigger("WallBump");
		WallRunShake();
		PlayerPhysics.isKinematic = false;
		base.gameObject.GetComponent<CapsuleCollider>().enabled = true;
		Player.transform.parent = Parent;
		StartCoroutine(ResetParkour());
		if (Horizontal)
		{
			Inventory.OffWall();
			PlayerPhysics.AddForce(400f * base.transform.up);
			PlayerPhysics.velocity = new Vector3(ParkourPlayer.PlayerPhysics.velocity.x, 0f, ParkourPlayer.PlayerPhysics.velocity.z);
			if (Right)
			{
				PlayerPhysics.AddForce(-500f * base.transform.right);
				if (Physics.Raycast(base.transform.position, base.transform.right, out HitAngle, 4f, AvailableLayers))
				{
					AngleResetTime = 1.25f;
					AngleSave = Vector3.Angle(HitAngle.normal, base.transform.parent.right);
					AngleRange.x = AngleSave + 5f;
					AngleRange.y = AngleSave - 5f;
				}
			}
			else
			{
				PlayerPhysics.AddForce(500f * base.transform.right);
				if (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitAngle, 4f, AvailableLayers))
				{
					AngleResetTime = 1.25f;
					AngleSave = Vector3.Angle(HitAngle.normal, base.transform.parent.right);
					AngleRange.x = AngleSave + 5f;
					AngleRange.y = AngleSave - 5f;
				}
			}
		}
		else
		{
			PlayerPhysics.AddForce(-350f * base.transform.forward);
			PlayerPhysics.AddForce(125f * base.transform.up);
			Inventory.UnFreezeWeapons();
			Inventory.OffWall();
		}
		if (ParkourPlayer != null)
		{
			Object.Destroy(ParkourPlayer.gameObject);
		}
	}

	public void WallBump(bool AddForce)
	{
		if (isWallRunning)
		{
			AirTime = 0.15f;
			canDash = true;
			OnWall = false;
			Object.Instantiate(Jump, base.transform.position, base.transform.rotation);
			WeaponParent.SetTrigger("WallBump");
			WallRunShake();
			PlayerPhysics.isKinematic = false;
			base.gameObject.GetComponent<CapsuleCollider>().enabled = true;
			Player.transform.parent = Parent;
			StartCoroutine(ResetParkour());
			Inventory.OffWall();
			Inventory.UnFreezeWeapons();
			if (AddForce)
			{
				PlayerPhysics.velocity = new Vector3(ParkourPlayer.PlayerPhysics.velocity.x, 0f, ParkourPlayer.PlayerPhysics.velocity.z);
			}
			if (ParkourPlayer != null)
			{
				Object.Destroy(ParkourPlayer.gameObject);
			}
			if (Physics.Raycast(base.transform.position, base.transform.right, out HitAngle, 4f, AvailableLayers))
			{
				AngleResetTime = 2f;
				AngleSave = Vector3.Angle(HitAngle.normal, base.transform.parent.right);
				AngleRange.x = AngleSave + 5f;
				AngleRange.y = AngleSave - 5f;
			}
			else if (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitAngle, 4f, AvailableLayers))
			{
				AngleResetTime = 2f;
				AngleSave = Vector3.Angle(HitAngle.normal, base.transform.parent.right);
				AngleRange.x = AngleSave + 5f;
				AngleRange.y = AngleSave - 5f;
			}
		}
	}

	public void WallRunShake()
	{
		OnWall = false;
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.35f, 0.1f, 0.1f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(4f, 1f, 2f);
		CameraShaker.Instance.ShakeOnce(13.5f, 1.25f, 0.1f, 0.75f);
		CameraShaker.Instance.ResetCamera();
	}

	private void UpdateCameraTilt()
	{
		if ((KeyBindingManager.GetKey(KeyAction.Forward) && inAir && !Swimming && !isSliding && AirTime <= 0f && KeyboardMouse && ForwardVelocity >= 8f) || (Input.GetAxis("Vertical") < 0f && inAir && !Swimming && !isSliding && AirTime <= 0f && !KeyboardMouse && ForwardVelocity >= 8f))
		{
			if (Physics.Raycast(base.transform.position, base.transform.right, out HitR, 3.25f, AvailableLayers) || Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitL, 3.25f, AvailableLayers))
			{
				if (Physics.Raycast(base.transform.position, base.transform.right, out HitR, 3.25f, AvailableLayers) && HitR.transform.tag == "Wall")
				{
					RightWall = true;
				}
				else
				{
					RightWall = false;
				}
				if (Physics.Raycast(base.transform.position, base.transform.right * -1f, out HitL, 3.25f, AvailableLayers) && HitL.transform.tag == "Wall")
				{
					LeftWall = true;
				}
				else
				{
					LeftWall = false;
				}
				if (CameraTilt <= 20f && CameraTilt >= -20f)
				{
					if (LeftWall && RightWall)
					{
						if (Vector3.Distance(base.transform.position, HitR.point) < Vector3.Distance(base.transform.position, HitL.point))
						{
							if ((CameraTilt < 20f && Vector3.Angle(HitR.normal, base.transform.parent.right) > AngleRange.x) || (CameraTilt < 20f && Vector3.Angle(HitL.normal, base.transform.parent.right) < AngleRange.y))
							{
								CameraTilt += Time.deltaTime * TiltSpeed;
								if (!isWallRunning)
								{
									base.transform.position = Vector3.MoveTowards(base.transform.position, HitR.point, 0.1f);
								}
							}
						}
						else if ((CameraTilt > -20f && Vector3.Angle(HitR.normal, base.transform.parent.right * -1f) > AngleRange.x) || (CameraTilt > -20f && Vector3.Angle(HitL.normal, base.transform.parent.right * -1f) < AngleRange.y))
						{
							CameraTilt -= Time.deltaTime * TiltSpeed;
							if (!isWallRunning)
							{
								base.transform.position = Vector3.MoveTowards(base.transform.position, HitR.point, -0.1f);
							}
						}
					}
					else
					{
						if (RightWall && ((CameraTilt < 20f && Vector3.Angle(HitR.normal, base.transform.parent.right) > AngleRange.x) || (CameraTilt < 20f && Vector3.Angle(HitR.normal, base.transform.parent.right) < AngleRange.y)))
						{
							CameraTilt += Time.deltaTime * TiltSpeed;
							if (!isWallRunning)
							{
								base.transform.position = Vector3.MoveTowards(base.transform.position, HitR.point, 0.1f);
							}
						}
						if (LeftWall && ((CameraTilt > -20f && Vector3.Angle(HitL.normal, base.transform.parent.right * -1f) > AngleRange.x) || (CameraTilt > -20f && Vector3.Angle(HitL.normal, base.transform.parent.right * -1f) < AngleRange.y)))
						{
							CameraTilt -= Time.deltaTime * TiltSpeed;
							if (!isWallRunning)
							{
								base.transform.position = Vector3.MoveTowards(base.transform.position, HitL.point, 0.1f);
							}
						}
					}
				}
			}
			else if (!OnWall && CameraTilt != 0f)
			{
				if (CameraTilt > 0f)
				{
					CameraTilt -= Time.deltaTime * TiltSpeed;
				}
				else
				{
					CameraTilt += Time.deltaTime * TiltSpeed;
				}
				if (CameraTilt <= 1f && CameraTilt >= -1f)
				{
					CameraTilt = 0f;
				}
			}
		}
		else if (!OnWall && CameraTilt != 0f)
		{
			if (CameraTilt > 0f)
			{
				CameraTilt -= Time.deltaTime * TiltSpeed;
			}
			else
			{
				CameraTilt += Time.deltaTime * TiltSpeed;
			}
			if (CameraTilt <= 1f && CameraTilt >= -1f)
			{
				CameraTilt = 0f;
			}
		}
		if (AngleResetTime > 0f)
		{
			AngleResetTime -= Time.deltaTime;
		}
		else
		{
			AngleSave = -25f;
			AngleRange.x = AngleSave;
			AngleRange.y = AngleSave;
		}
		if (OnWall)
		{
			TiltSpeed = 125f;
		}
		else
		{
			TiltSpeed = 75f;
		}
	}

	public void PlayFootStepAudio()
	{
		if (isWalking && !isFrozen)
		{
			if (grounded)
			{
				CameraShaker.Instance.DefaultPosInfluence = Vector3.zero;
				CameraShaker.Instance.DefaultRotInfluence = new Vector3(1f, 0.35f, 0.35f);
				CameraShaker.Instance.ShakeOnce(8f, 0.75f, 0.25f, 0.25f);
				CameraShaker.Instance.ResetCamera();
			}
			if (CrouchGround)
			{
				CameraShaker.Instance.DefaultPosInfluence = Vector3.zero;
				CameraShaker.Instance.DefaultRotInfluence = new Vector3(1f, 0.35f, 0.35f);
				CameraShaker.Instance.ShakeOnce(18f, 0.5f, 0.35f, 0.75f);
				CameraShaker.Instance.ResetCamera();
			}
			RevertPoint.position = Object.Instantiate(Footstep, new Vector3(base.transform.position.x, base.transform.position.y - 0.75f, base.transform.position.z), base.transform.rotation).transform.position;
		}
	}

	private void UpdateMouseLook()
	{
		if (!isFrozen)
		{
			if (KeyboardMouse)
			{
				rotationX += Input.GetAxis("Mouse X") * Sensitivity;
				rotationY += (0f - Input.GetAxis("Mouse Y")) * Sensitivity * (float)Invert;
				XAccel = 0f;
				YAccel = 0f;
			}
			else if (!KeyboardMouse)
			{
				rotationX += Input.GetAxis("Controller X") * XAccel;
				rotationY += Input.GetAxis("Controller Y") * YAccel * (float)Invert;
				if (Input.GetAxis("Controller X") >= 0.35f || Input.GetAxis("Controller X") <= -0.35f)
				{
					XAccel = Mathf.Lerp(XAccel, Sensitivity, XAccelerationTime);
					XAccelerationTime += Time.unscaledDeltaTime * ControllerAcceleration;
				}
				else
				{
					XAccel = Sensitivity * 0.35f;
					XAccelerationTime = 0f;
				}
				if (Input.GetAxis("Controller Y") >= 0.35f || Input.GetAxis("Controller Y") <= -0.35f)
				{
					YAccel = Mathf.Lerp(YAccel, Sensitivity, YAccelerationTime);
					YAccelerationTime += Time.unscaledDeltaTime * ControllerAcceleration;
				}
				else
				{
					YAccel = Sensitivity * 0.35f;
					YAccelerationTime = 0f;
				}
			}
			if (!inAir)
			{
				if (rotationY >= MaxYRotation)
				{
					rotationY = MaxYRotation;
				}
				if (0f - MaxYRotation >= rotationY)
				{
					rotationY = 0f - MaxYRotation;
				}
			}
			else if (!isCrouching)
			{
				if (rotationY > 270f)
				{
					rotationY *= -0.275f;
				}
				if (rotationY < -270f)
				{
					rotationY *= -0.275f;
				}
			}
			if (InvertControls)
			{
				Invert = -1;
			}
			else
			{
				Invert = 1;
			}
		}
		Player.transform.rotation = Quaternion.Euler(Player.transform.rotation.x, rotationX, Player.transform.rotation.z);
		CamParent.transform.localRotation = Quaternion.Euler(rotationY, CamParent.transform.localRotation.y, CameraTilt);
	}

	public void SetXRotation(float NewRotation)
	{
		rotationX = NewRotation;
	}

	public void SetYRotation(float NewRotation)
	{
		rotationY = NewRotation;
	}

	private void UpdateCommands()
	{
		if (Input.GetKeyDown(KeyCode.Return) && !LockedScreen && !isFrozen)
		{
			LockedScreen = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			PreviousSensitivity = Sensitivity;
			Sensitivity = 0f;
		}
		if (Input.GetKeyDown(KeyCode.Mouse0) && LockedScreen)
		{
			LockedScreen = false;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Sensitivity = PreviousSensitivity;
		}
	}

	public void LockScreen()
	{
		LockedScreen = true;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PreviousSensitivity = Sensitivity;
		Sensitivity = 0f;
	}

	public void UnlockScreen()
	{
		LockedScreen = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Sensitivity = PreviousSensitivity;
	}

	public void WheelFreezePlayer()
	{
		isFrozen = true;
		Inventory.FreezeWeapons();
	}

	public void WheelUnFreezePlayer()
	{
		isFrozen = false;
		Inventory.UnFreezeWeapons();
		ReParent();
	}

	public void FreezePlayer()
	{
		isFrozen = true;
		PlayerPhysics.isKinematic = true;
		Inventory.FreezeWeapons();
	}

	public void UnFreezePlayer()
	{
		isFrozen = false;
		PlayerPhysics.isKinematic = false;
		Inventory.UnFreezeWeapons();
		ReParent();
	}

	public void SoftFreeze()
	{
		isFrozen = true;
		Crosshair.gameObject.SetActive(value: false);
		PlayerPhysics.isKinematic = true;
	}

	public void Unground()
	{
		grounded = false;
		GroundTime = 0.5f;
	}

	public void StationaryFreeze()
	{
		isFrozen = true;
		PlayerPhysics.isKinematic = true;
		Inventory.FreezeWeapons();
		Crosshair.gameObject.SetActive(value: false);
	}

	public void ReParent()
	{
		base.transform.parent = Parent;
		PlayerPhysics.isKinematic = false;
	}

	public void Revert()
	{
		base.transform.position = new Vector3(RevertPoint.position.x, RevertPoint.position.y + 0.75f, RevertPoint.position.z);
		PlayerPhysics.velocity = Vector3.zero;
	}

	public void ResetAbilities()
	{
		OutOfJumps = false;
		canDash = true;
	}

	private IEnumerator SlowReset()
	{
		yield return new WaitForSeconds(0.25f);
		OutOfJumps = false;
		canDoubleJump = true;
		CanJump = false;
		canDash = true;
	}

	public void MovementShake()
	{
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.05f, 0.1f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(5f, 1f, 2f);
		CameraShaker.Instance.ShakeOnce(12f, 1f, 0.2f, 1.5f);
		CameraShaker.Instance.ResetCamera();
	}

	public void ResetCharacter()
	{
		SetXRotation(0f);
		SetYRotation(0f);
		base.transform.localPosition = new Vector3(0f, 0.35f, 0f);
	}

	public void Blur()
	{
		MainCamera.GetComponent<Animation>().Stop();
		MainCamera.GetComponent<Animation>().Play("RadialBlur");
	}

	public void HeavyBlur()
	{
		MainCamera.GetComponent<Animation>().Play("HeavyBlur");
	}

	public void SetSwimming(bool SwimmingState)
	{
		Swimming = SwimmingState;
		if (SwimmingState)
		{
			CurrentForwardSwimSpeed = SwimSpeed;
			ForwardDirection = 1f;
			WaterEffect = Object.Instantiate(InWater, base.transform.position, base.transform.rotation).transform;
			WaterEffect.LookAt(base.transform.position);
			Inventory.Bubbles.SetActive(value: true);
			PlayerUnCrouch();
		}
		else
		{
			ResetAbilities();
			WaterEffect = Object.Instantiate(OutWater, base.transform.position, base.transform.rotation).transform;
			WaterEffect.LookAt(base.transform.position);
			PlayerPhysics.velocity += MainCamera.transform.forward * 10f;
			Inventory.Bubbles.SetActive(value: false);
		}
	}

	public IEnumerator ResetParkour()
	{
		OutOfJumps = false;
		yield return new WaitForSeconds(0.45f);
		isWallRunning = false;
		canStartWallRun = true;
	}

	public void CutsceneStart()
	{
		FreezePlayer();
		SetXRotation(0f);
		SetYRotation(0f);
		PauseMenu.CanPauseGame = false;
		MainCamera.GetComponent<Camera>().enabled = false;
		WeaponCamera.GetComponent<Camera>().enabled = false;
		MainCamera.gameObject.GetComponent<Camera>().enabled = false;
	}

	public void CutsceneEnd(Transform EndPosition)
	{
		base.transform.position = EndPosition.position;
		UnFreezePlayer();
		GM.gameObject.GetComponent<MenuScript>().CanPauseGame = true;
		MainCamera.GetComponent<Camera>().enabled = true;
		WeaponCamera.GetComponent<Camera>().enabled = true;
		MainCamera.gameObject.GetComponent<Camera>().enabled = true;
		Inventory.HasMelee = true;
	}

	public void RemoveAbilities()
	{
		CanWallRun = false;
		CanVault = false;
		CanDoubleJump = false;
		CanSlide = false;
		CanDash = false;
		Inventory.HasMelee = false;
		GetComponent<HealthScript>().CanRegen = false;
		GetComponent<HealthScript>().Shield = 0f;
	}

	public void BeginRail()
	{
		grounded = false;
		PlayerPhysics.isKinematic = true;
	}
}
