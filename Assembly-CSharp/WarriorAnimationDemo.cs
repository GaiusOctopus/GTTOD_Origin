using System.Collections;
using UnityEngine;

public class WarriorAnimationDemo : MonoBehaviour
{
	[HideInInspector]
	public Animator animator;

	public Warrior warrior;

	private IKHands ikhands;

	private Rigidbody rigidBody;

	public GameObject target;

	public GameObject weaponModel;

	public GameObject secondaryWeaponModel;

	private float rotationSpeed = 15f;

	public float gravity = -9.83f;

	public float runSpeed = 8f;

	public float walkSpeed = 3f;

	public float strafeSpeed = 3f;

	private bool canMove = true;

	public float jumpSpeed = 8f;

	private bool jumpHold;

	[HideInInspector]
	public bool canJump = true;

	private float fallingVelocity = -2f;

	private bool isFalling;

	public float inAirSpeed = 8f;

	private float maxVelocity = 2f;

	private float minVelocity = -2f;

	[HideInInspector]
	public Vector3 newVelocity;

	private Vector3 inputVec;

	private Vector3 dashInputVec;

	private Vector3 targetDirection;

	private bool isDashing;

	[HideInInspector]
	public bool isGrounded = true;

	[HideInInspector]
	public bool dead;

	private bool isStrafing;

	[HideInInspector]
	public bool isAiming;

	private bool aimingGui;

	[HideInInspector]
	public bool isBlocking;

	[HideInInspector]
	public bool isStunned;

	[HideInInspector]
	public bool isSitting;

	[HideInInspector]
	public bool inBlock;

	[HideInInspector]
	public bool blockGui;

	[HideInInspector]
	public bool weaponSheathed;

	[HideInInspector]
	public bool weaponSheathed2;

	private bool isInAir;

	[HideInInspector]
	public bool isStealth;

	public float stealthSpeed;

	[HideInInspector]
	public bool isWall;

	[HideInInspector]
	public bool ledgeGui;

	[HideInInspector]
	public bool ledge;

	public float ledgeSpeed;

	[HideInInspector]
	public int attack;

	private bool canChain;

	[HideInInspector]
	public bool specialAttack2Bool;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		if (warrior == Warrior.Archer)
		{
			secondaryWeaponModel.gameObject.SetActive(value: false);
		}
		if ((warrior == Warrior.Archer || warrior == Warrior.Crossbow) && animator.layerCount >= 1)
		{
			animator.SetLayerWeight(1, 0f);
		}
		if (warrior == Warrior.TwoHanded)
		{
			secondaryWeaponModel.GetComponent<Renderer>().enabled = false;
			ikhands = base.gameObject.GetComponent<IKHands>();
		}
	}

	private void FixedUpdate()
	{
		CheckForGrounded();
		rigidBody.AddForce(0f, gravity, 0f, ForceMode.Acceleration);
		if (!isGrounded)
		{
			AirControl();
		}
		if (canMove)
		{
			UpdateMovement();
		}
		if (rigidBody.velocity.y < fallingVelocity)
		{
			isFalling = true;
		}
		else
		{
			isFalling = false;
		}
	}

	private void LateUpdate()
	{
		float x = base.transform.InverseTransformDirection(rigidBody.velocity).x;
		float z = base.transform.InverseTransformDirection(rigidBody.velocity).z;
		animator.SetFloat("Input X", x / runSpeed);
		animator.SetFloat("Input Z", z / runSpeed);
	}

	private void Update()
	{
		CameraRelativeInput();
		InAir();
		JumpingUpdate();
		if (Input.GetKeyDown(KeyCode.P))
		{
			Debug.Break();
		}
		if (!dead || !blockGui || !isBlocking)
		{
			if (!weaponSheathed)
			{
				if (!blockGui)
				{
					if ((double)Input.GetAxis("TargetBlock") < -0.1 && !inBlock && !isInAir && attack == 0)
					{
						animator.SetBool("Block", value: true);
						isBlocking = true;
						animator.SetBool("Running", value: false);
						animator.SetBool("Moving", value: false);
						newVelocity = new Vector3(0f, 0f, 0f);
					}
					if (Input.GetAxis("TargetBlock") == 0f)
					{
						inBlock = false;
						isBlocking = false;
						animator.SetBool("Block", value: false);
					}
				}
				if (!isBlocking)
				{
					if (Input.GetButtonDown("Fire1") && attack <= 3)
					{
						AttackChain();
					}
					if (!isInAir)
					{
						if (Input.GetButtonDown("Jump") && canJump)
						{
							StartCoroutine(_Jump(0.8f));
						}
						if (attack == 0)
						{
							if (Input.GetButtonDown("Fire0"))
							{
								RangedAttack();
							}
							if (Input.GetButtonDown("Fire2"))
							{
								MoveAttack();
							}
							if (Input.GetButtonDown("Fire3"))
							{
								SpecialAttack();
							}
						}
						if (Input.GetButtonDown("LightHit"))
						{
							StartCoroutine(_GetHit());
						}
					}
				}
				else
				{
					if (Input.GetButtonDown("Jump"))
					{
						StartCoroutine(_BlockHitReact());
					}
					if (Input.GetButtonDown("Fire0"))
					{
						StartCoroutine(_BlockHitReact());
					}
					if (Input.GetButtonDown("Fire1"))
					{
						StartCoroutine(_BlockHitReact());
					}
					if (Input.GetButtonDown("Fire2"))
					{
						StartCoroutine(_BlockHitReact());
					}
					if (Input.GetButtonDown("Fire3"))
					{
						StartCoroutine(_BlockHitReact());
					}
					if (Input.GetButtonDown("LightHit"))
					{
						StartCoroutine(_BlockBreak());
					}
					if (Input.GetButtonDown("Death"))
					{
						StartCoroutine(_BlockBreak());
					}
				}
				if (((double)Input.GetAxis("DashVertical") > 0.5 || (double)Input.GetAxis("DashVertical") < -0.5 || (double)Input.GetAxis("DashHorizontal") > 0.5 || (double)Input.GetAxis("DashHorizontal") < -0.5) && !isDashing && !isInAir)
				{
					StartCoroutine(_DirectionalDash());
				}
			}
		}
		else
		{
			newVelocity = new Vector3(0f, 0f, 0f);
			inputVec = new Vector3(0f, 0f, 0f);
		}
		if (!dead)
		{
			if (!isBlocking && Input.GetButtonDown("Death"))
			{
				Dead();
			}
		}
		else if (Input.GetButtonDown("Death"))
		{
			StartCoroutine(_Revive());
		}
		if (Input.GetButtonDown("Special") && warrior == Warrior.Ninja)
		{
			if (!isStealth)
			{
				isStealth = true;
				animator.SetBool("Stealth", value: true);
			}
			else
			{
				isStealth = false;
				animator.SetBool("Stealth", value: false);
			}
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (Time.timeScale == 1f)
			{
				Time.timeScale = 0.25f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}

	private void CameraRelativeInput()
	{
		if (isStunned)
		{
			return;
		}
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		float axisRaw3 = Input.GetAxisRaw("DashHorizontal");
		float axisRaw4 = Input.GetAxisRaw("DashVertical");
		Vector3 a = Camera.main.transform.TransformDirection(Vector3.forward);
		a.y = 0f;
		a = a.normalized;
		Vector3 a2 = new Vector3(a.z, 0f, 0f - a.x);
		inputVec = axisRaw * a2 + axisRaw2 * a;
		dashInputVec = axisRaw3 * a2 + axisRaw4 * a;
		if (isBlocking)
		{
			return;
		}
		if ((double)axisRaw2 > 0.1 || (double)axisRaw2 < -0.1 || (double)axisRaw > 0.1 || (double)axisRaw < -0.1)
		{
			animator.SetBool("Moving", value: true);
			animator.SetBool("Running", value: true);
			if (Input.GetKey(KeyCode.LeftShift) || (double)Input.GetAxisRaw("TargetBlock") > 0.1)
			{
				if (!weaponSheathed)
				{
					isStrafing = true;
					animator.SetBool("Running", value: false);
				}
			}
			else
			{
				isStrafing = false;
				animator.SetBool("Running", value: true);
			}
		}
		else
		{
			animator.SetBool("Moving", value: false);
			animator.SetBool("Running", value: false);
		}
	}

	private float UpdateMovement()
	{
		Vector3 vector = inputVec;
		if (isGrounded)
		{
			if (!dead && !isBlocking && !blockGui && !isStunned)
			{
				if (!isStrafing)
				{
					newVelocity = vector.normalized * runSpeed;
				}
				else
				{
					newVelocity = vector.normalized * strafeSpeed;
				}
				if (ledge)
				{
					newVelocity = vector.normalized * ledgeSpeed;
				}
				if (isStealth)
				{
					newVelocity = vector.normalized * stealthSpeed;
				}
			}
			else
			{
				newVelocity = new Vector3(0f, 0f, 0f);
				inputVec = new Vector3(0f, 0f, 0f);
			}
		}
		else
		{
			newVelocity = rigidBody.velocity;
		}
		newVelocity.y = rigidBody.velocity.y;
		rigidBody.velocity = newVelocity;
		if (!isStrafing && !isWall && (!ledgeGui || !ledge))
		{
			RotateTowardMovementDirection();
		}
		if (isStrafing)
		{
			float num = 40f;
			Quaternion quaternion = Quaternion.LookRotation(target.transform.position - new Vector3(base.transform.position.x, 0f, base.transform.position.z));
			base.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(base.transform.eulerAngles.y, quaternion.eulerAngles.y, num * Time.deltaTime * num);
		}
		return inputVec.magnitude;
	}

	private void RotateTowardMovementDirection()
	{
		if (!dead && !blockGui && !isBlocking && !isStunned && inputVec != Vector3.zero)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	private void CheckForGrounded()
	{
		float num = 0.45f;
		Vector3 b = new Vector3(0f, 0.4f, 0f);
		if (Physics.Raycast(base.transform.position + b, -Vector3.up, out RaycastHit hitInfo, 100f))
		{
			if (hitInfo.distance < num)
			{
				isGrounded = true;
				isInAir = false;
			}
			else
			{
				isGrounded = false;
				isInAir = true;
			}
		}
	}

	private void JumpingUpdate()
	{
		if (!jumpHold)
		{
			if (isGrounded)
			{
				animator.SetInteger("Jumping", 0);
				canJump = true;
			}
			else if (!ledge && isFalling)
			{
				animator.SetInteger("Jumping", 2);
				canJump = false;
			}
		}
	}

	public IEnumerator _Jump(float jumpTime)
	{
		animator.SetTrigger("JumpTrigger");
		canJump = false;
		rigidBody.velocity += jumpSpeed * Vector3.up;
		animator.SetInteger("Jumping", 1);
		yield return new WaitForSeconds(jumpTime);
	}

	private void AirControl()
	{
		float axisRaw = Input.GetAxisRaw("Horizontal");
		float axisRaw2 = Input.GetAxisRaw("Vertical");
		Vector3 vector = new Vector3(axisRaw, 0f, axisRaw2);
		Vector3 a = vector;
		a *= ((Mathf.Abs(vector.x) == 1f && Mathf.Abs(vector.z) == 1f) ? 0.7f : 1f);
		rigidBody.AddForce(a * inAirSpeed, ForceMode.Acceleration);
		float num = 0f;
		float num2 = 0f;
		if (rigidBody.velocity.x > maxVelocity)
		{
			num = rigidBody.velocity.x - maxVelocity;
			if (num < 0f)
			{
				num = 0f;
			}
			rigidBody.AddForce(new Vector3(0f - num, 0f, 0f), ForceMode.Acceleration);
		}
		if (rigidBody.velocity.x < minVelocity)
		{
			num = rigidBody.velocity.x - minVelocity;
			if (num > 0f)
			{
				num = 0f;
			}
			rigidBody.AddForce(new Vector3(0f - num, 0f, 0f), ForceMode.Acceleration);
		}
		if (rigidBody.velocity.z > maxVelocity)
		{
			num2 = rigidBody.velocity.z - maxVelocity;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			rigidBody.AddForce(new Vector3(0f, 0f, 0f - num2), ForceMode.Acceleration);
		}
		if (rigidBody.velocity.z < minVelocity)
		{
			num2 = rigidBody.velocity.z - minVelocity;
			if (num2 > 0f)
			{
				num2 = 0f;
			}
			rigidBody.AddForce(new Vector3(0f, 0f, 0f - num2), ForceMode.Acceleration);
		}
	}

	private void InAir()
	{
		if (isInAir && ledgeGui)
		{
			animator.SetTrigger("Ledge-Catch");
			ledge = true;
		}
	}

	public void AttackChain()
	{
		if (isInAir)
		{
			StartCoroutine(_JumpAttack1());
		}
		else if (attack == 0)
		{
			StartCoroutine(_Attack1());
		}
		else if (canChain && warrior != Warrior.Archer)
		{
			if (attack == 1)
			{
				StartCoroutine(_Attack2());
			}
			else if (attack == 2)
			{
				StartCoroutine(_Attack3());
			}
		}
	}

	private IEnumerator _Attack1()
	{
		StopAllCoroutines();
		canChain = false;
		animator.SetInteger("Attack", 1);
		attack = 1;
		if (warrior == Warrior.Knight)
		{
			StartCoroutine(_ChainWindow(0.1f, 0.8f));
			StartCoroutine(_LockMovementAndAttack(0.6f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_ChainWindow(0.6f, 1f));
			StartCoroutine(_LockMovementAndAttack(0.85f));
		}
		else if (warrior == Warrior.Brute)
		{
			StartCoroutine(_ChainWindow(0.5f, 0.5f));
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Sorceress)
		{
			StartCoroutine(_ChainWindow(0.3f, 1.4f));
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.1f));
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_ChainWindow(0.2f, 0.8f));
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.2f));
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(0.7f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_ChainWindow(0.4f, 1.2f));
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Archer)
		{
			StartCoroutine(_LockMovementAndAttack(0.7f));
		}
		else
		{
			StartCoroutine(_ChainWindow(0.2f, 0.7f));
			StartCoroutine(_LockMovementAndAttack(0.6f));
		}
		yield return null;
	}

	private IEnumerator _Attack2()
	{
		StopAllCoroutines();
		canChain = false;
		animator.SetInteger("Attack", 2);
		attack = 2;
		if (warrior == Warrior.Knight)
		{
			StartCoroutine(_ChainWindow(0.4f, 0.9f));
			StartCoroutine(_LockMovementAndAttack(0.5f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_ChainWindow(0.5f, 0.8f));
			StartCoroutine(_LockMovementAndAttack(0.75f));
		}
		else if (warrior == Warrior.Brute)
		{
			StartCoroutine(_ChainWindow(0.3f, 0.7f));
			StartCoroutine(_LockMovementAndAttack(1.4f));
		}
		else if (warrior == Warrior.Sorceress)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.2f));
		}
		else if (warrior == Warrior.Karate)
		{
			StartCoroutine(_ChainWindow(0.3f, 0.6f));
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.1f));
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.1f));
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_ChainWindow(0.6f, 1.2f));
			StartCoroutine(_LockMovementAndAttack(1.4f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_ChainWindow(0.4f, 1.2f));
			StartCoroutine(_LockMovementAndAttack(1.3f));
		}
		else if (warrior == Warrior.Ninja)
		{
			StartCoroutine(_ChainWindow(0.2f, 0.8f));
			StartCoroutine(_LockMovementAndAttack(0.8f));
		}
		else
		{
			StartCoroutine(_ChainWindow(0.1f, 2f));
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		yield return null;
	}

	private IEnumerator _Attack3()
	{
		StopAllCoroutines();
		animator.SetInteger("Attack", 3);
		attack = 3;
		if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(0.8f));
		}
		if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(1.5f));
		}
		else if (warrior == Warrior.Karate)
		{
			StartCoroutine(_LockMovementAndAttack(0.8f));
		}
		else if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(1.7f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		canChain = false;
		yield return null;
	}

	public void RangedAttack()
	{
		StopAllCoroutines();
		animator.SetTrigger("RangeAttack1Trigger");
		attack = 4;
		if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(2.4f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(1.7f));
		}
		else if (warrior == Warrior.Ninja)
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Archer)
		{
			if (!isAiming)
			{
				StartCoroutine(_SetLayerWeightForTime(0.6f));
				StartCoroutine(_ArcherArrowOn(0.2f));
			}
			else
			{
				StartCoroutine(_ArcherArrowOff(0.2f));
			}
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_SetLayerWeightForTime(0.6f));
			StartCoroutine(_ArcherArrowOn(0.2f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_LockMovementAndAttack(2.4f));
			StartCoroutine(_SecondaryWeaponVisibility(0.7f, weaponVisiblity: true));
			StartCoroutine(_WeaponVisibility(0.7f, weaponVisiblity: false));
			StartCoroutine(_SecondaryWeaponVisibility(2f, weaponVisiblity: false));
			StartCoroutine(_WeaponVisibility(2f, weaponVisiblity: true));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(1.7f));
		}
		else if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
	}

	public void MoveAttack()
	{
		StopAllCoroutines();
		attack = 5;
		animator.SetTrigger("MoveAttack1Trigger");
		if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(1.4f));
		}
		else if (warrior == Warrior.Sorceress)
		{
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(1.5f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(2.5f));
		}
		else if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
	}

	public void SpecialAttack()
	{
		StopAllCoroutines();
		attack = 6;
		animator.SetTrigger("SpecialAttack1Trigger");
		if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(2f));
		}
		else if (warrior == Warrior.Sorceress)
		{
			StartCoroutine(_LockMovementAndAttack(1.5f));
		}
		else if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(1.95f));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(1.6f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1.7f));
		}
	}

	public IEnumerator _JumpAttack1()
	{
		yield return new WaitForFixedUpdate();
		jumpHold = true;
		rigidBody.velocity += jumpSpeed * -Vector3.up;
		animator.SetTrigger("JumpAttack1Trigger");
		if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
			yield return new WaitForSeconds(0.5f);
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
			yield return new WaitForSeconds(0.7f);
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
			yield return new WaitForSeconds(0.7f);
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1f));
			yield return new WaitForSeconds(0.8f);
		}
		jumpHold = false;
	}

	public IEnumerator _DirectionalDash()
	{
		float num = Vector3.Angle(dashInputVec, base.transform.forward);
		float num2 = Mathf.Sign(Vector3.Dot(base.transform.up, Vector3.Cross(dashInputVec, base.transform.forward)));
		float num3 = (num * num2 + 180f) % 360f;
		if (num3 > 315f || num3 < 45f)
		{
			StartCoroutine(_Dash(1));
		}
		if (num3 > 45f && num3 < 135f)
		{
			StartCoroutine(_Dash(2));
		}
		if (num3 > 135f && num3 < 225f)
		{
			StartCoroutine(_Dash(3));
		}
		if (num3 > 225f && num3 < 315f)
		{
			StartCoroutine(_Dash(4));
		}
		yield return null;
	}

	public IEnumerator _Dash(int dashDirection)
	{
		isDashing = true;
		animator.SetInteger("Dash", dashDirection);
		if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else if (warrior == Warrior.Karate)
		{
			StartCoroutine(_LockMovementAndAttack(0.7f));
		}
		else if (warrior == Warrior.Knight)
		{
			StartCoroutine(_LockMovementAndAttack(1.15f));
		}
		else if (warrior == Warrior.Archer)
		{
			StartCoroutine(_LockMovementAndAttack(0.6f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(0.8f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(0.55f));
		}
		else if (warrior == Warrior.Hammer)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(0.65f));
		}
		yield return new WaitForSeconds(0.1f);
		animator.SetInteger("Dash", 0);
		isDashing = false;
	}

	public IEnumerator _Dash2(int dashDirection)
	{
		isDashing = true;
		animator.SetInteger("Dash2", dashDirection);
		yield return new WaitForEndOfFrame();
		animator.SetInteger("Dash2", dashDirection);
		yield return new WaitForEndOfFrame();
		StartCoroutine(_LockMovementAndAttack(0.45f));
		animator.SetInteger("Dash2", 0);
		yield return new WaitForSeconds(0.95f);
		isDashing = false;
	}

	private IEnumerator _SetInAir(float timeToStart, float lenthOfTime)
	{
		yield return new WaitForSeconds(timeToStart);
		isInAir = true;
		yield return new WaitForSeconds(lenthOfTime);
		isInAir = false;
	}

	public IEnumerator _ChainWindow(float timeToWindow, float chainLength)
	{
		yield return new WaitForSeconds(timeToWindow);
		canChain = true;
		animator.SetInteger("Attack", 0);
		yield return new WaitForSeconds(chainLength);
		canChain = false;
	}

	public IEnumerator _LockMovementAndAttack(float pauseTime)
	{
		isStunned = true;
		animator.applyRootMotion = true;
		inputVec = new Vector3(0f, 0f, 0f);
		newVelocity = new Vector3(0f, 0f, 0f);
		animator.SetFloat("Input X", 0f);
		animator.SetFloat("Input Z", 0f);
		animator.SetBool("Moving", value: false);
		yield return new WaitForSeconds(pauseTime);
		animator.SetInteger("Attack", 0);
		canChain = false;
		isStunned = false;
		animator.applyRootMotion = false;
		yield return new WaitForSeconds(0.2f);
		attack = 0;
	}

	public void SheathWeapon()
	{
		animator.SetTrigger("WeaponSheathTrigger");
		if (warrior == Warrior.Archer)
		{
			StartCoroutine(_WeaponVisibility(0.4f, weaponVisiblity: false));
		}
		else if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_WeaponVisibility(0.4f, weaponVisiblity: false));
			StartCoroutine(_SecondaryWeaponVisibility(0.4f, weaponVisiblity: false));
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_WeaponVisibility(0.26f, weaponVisiblity: false));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_WeaponVisibility(0.5f, weaponVisiblity: false));
			StartCoroutine(_BlendIKHandLeftRot(0f, 0.3f, 0f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(1.1f));
		}
		else if (warrior == Warrior.Ninja)
		{
			StartCoroutine(_LockMovementAndAttack(1.4f));
			StartCoroutine(_WeaponVisibility(0.5f, weaponVisiblity: false));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1.4f));
			StartCoroutine(_WeaponVisibility(0.5f, weaponVisiblity: false));
		}
		weaponSheathed = true;
	}

	public void UnSheathWeapon()
	{
		animator.SetTrigger("WeaponUnsheathTrigger");
		if (warrior == Warrior.Archer)
		{
			StartCoroutine(_WeaponVisibility(0.4f, weaponVisiblity: true));
		}
		else if (warrior == Warrior.TwoHanded)
		{
			StartCoroutine(_WeaponVisibility(0.35f, weaponVisiblity: true));
		}
		else if (warrior == Warrior.Swordsman)
		{
			StartCoroutine(_WeaponVisibility(0.35f, weaponVisiblity: true));
			StartCoroutine(_SecondaryWeaponVisibility(0.35f, weaponVisiblity: true));
		}
		else if (warrior == Warrior.Spearman)
		{
			StartCoroutine(_WeaponVisibility(0.45f, weaponVisiblity: true));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_WeaponVisibility(0.6f, weaponVisiblity: true));
			StartCoroutine(_LockMovementAndAttack(1f));
		}
		else
		{
			StartCoroutine(_WeaponVisibility(0.6f, weaponVisiblity: true));
			StartCoroutine(_LockMovementAndAttack(1.4f));
		}
		weaponSheathed = false;
	}

	public IEnumerator _WeaponVisibility(float waitTime, bool weaponVisiblity)
	{
		yield return new WaitForSeconds(waitTime);
		weaponModel.SetActive(weaponVisiblity);
		if (secondaryWeaponModel != null)
		{
			secondaryWeaponModel.SetActive(weaponVisiblity);
		}
	}

	private IEnumerator _SecondaryWeaponVisibility(float waitTime, bool weaponVisiblity)
	{
		yield return new WaitForSeconds(waitTime);
		secondaryWeaponModel.GetComponent<Renderer>().enabled = weaponVisiblity;
	}

	public IEnumerator _ArcherArrowOn(float waitTime)
	{
		if (warrior != Warrior.Crossbow)
		{
			secondaryWeaponModel.gameObject.SetActive(value: true);
		}
		yield return new WaitForSeconds(waitTime);
		if (warrior != Warrior.Crossbow)
		{
			secondaryWeaponModel.gameObject.SetActive(value: false);
		}
		yield return new WaitForSeconds(0.2f);
		animator.SetInteger("Attack", 0);
		attack = 0;
	}

	public IEnumerator _ArcherArrowOff(float waitTime)
	{
		if (warrior != Warrior.Crossbow)
		{
			secondaryWeaponModel.gameObject.SetActive(value: false);
		}
		yield return new WaitForSeconds(waitTime);
		if (warrior != Warrior.Crossbow)
		{
			secondaryWeaponModel.gameObject.SetActive(value: true);
		}
		yield return new WaitForSeconds(0.2f);
		animator.SetInteger("Attack", 0);
		attack = 0;
	}

	public IEnumerator _SetLayerWeightForTime(float time)
	{
		animator.SetLayerWeight(1, 1f);
		yield return new WaitForSeconds(time);
		float a = 1f;
		for (int i = 0; i < 20; i++)
		{
			a -= 0.05f;
			animator.SetLayerWeight(1, a);
			yield return new WaitForEndOfFrame();
		}
		animator.SetLayerWeight(1, 0f);
	}

	public IEnumerator _SetLayerWeight(float amount)
	{
		animator.SetLayerWeight(1, amount);
		yield return null;
	}

	public IEnumerator _BlockHitReact()
	{
		StartCoroutine(_LockMovementAndAttack(0.5f));
		animator.SetTrigger("BlockHitReactTrigger");
		yield return null;
	}

	public IEnumerator _BlockBreak()
	{
		StartCoroutine(_LockMovementAndAttack(1f));
		animator.SetTrigger("BlockBreakTrigger");
		yield return null;
	}

	public IEnumerator _GetHit()
	{
		animator.SetTrigger("LightHitTrigger");
		if (warrior == Warrior.Ninja)
		{
			StartCoroutine(_LockMovementAndAttack(2.4f));
		}
		else if (warrior == Warrior.Archer)
		{
			StartCoroutine(_LockMovementAndAttack(2.7f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(2.5f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(2.8f));
		}
		yield return null;
	}

	private void Dead()
	{
		animator.applyRootMotion = true;
		animator.SetTrigger("DeathTrigger");
		dead = true;
	}

	public IEnumerator _Revive()
	{
		animator.SetTrigger("ReviveTrigger");
		if (warrior == Warrior.Brute)
		{
			StartCoroutine(_LockMovementAndAttack(1.75f));
		}
		else if (warrior == Warrior.Mage)
		{
			StartCoroutine(_LockMovementAndAttack(1.2f));
		}
		else if (warrior == Warrior.Sorceress)
		{
			StartCoroutine(_LockMovementAndAttack(0.7f));
		}
		else if (warrior == Warrior.Ninja)
		{
			StartCoroutine(_LockMovementAndAttack(0.9f));
		}
		else if (warrior == Warrior.Crossbow)
		{
			StartCoroutine(_LockMovementAndAttack(1.3f));
		}
		else
		{
			StartCoroutine(_LockMovementAndAttack(1.1f));
			yield return null;
		}
		dead = false;
	}

	public void Hit()
	{
	}

	public void Shoot()
	{
	}

	public void FootR()
	{
	}

	public void FootL()
	{
	}

	public void Land()
	{
	}

	public void WeaponSwitch()
	{
	}

	private IEnumerator _BlendIKHandLeftPos(float wait, float timeToBlend, float amount)
	{
		yield return new WaitForSeconds(wait);
		float currentLeftPos = ikhands.leftHandPositionWeight;
		float diffOverTime = (Mathf.Abs(currentLeftPos) - amount) / timeToBlend;
		float time = 0f;
		if (currentLeftPos > amount)
		{
			while (time < timeToBlend)
			{
				time += Time.deltaTime;
				ikhands.leftHandPositionWeight -= diffOverTime;
				yield return null;
			}
		}
		if (currentLeftPos < amount)
		{
			while (time < timeToBlend)
			{
				time += Time.deltaTime;
				ikhands.leftHandPositionWeight += diffOverTime;
				yield return null;
			}
		}
	}

	private IEnumerator _BlendIKHandRightPos(float wait, float timeToBlend, float amount)
	{
		yield return new WaitForSeconds(wait);
		float currentRightPos = ikhands.rightHandPositionWeight;
		float diffOverTime = (Mathf.Abs(currentRightPos) - amount) / timeToBlend;
		float time = 0f;
		if (currentRightPos > amount)
		{
			while (time < timeToBlend)
			{
				time += Time.deltaTime;
				ikhands.rightHandPositionWeight -= diffOverTime;
				yield return null;
			}
		}
		if (currentRightPos < amount)
		{
			while (time < timeToBlend)
			{
				time += Time.deltaTime;
				ikhands.rightHandPositionWeight += diffOverTime;
				yield return null;
			}
		}
	}

	private IEnumerator _BlendIKHandLeftRot(float wait, float timeToBlend, float amount)
	{
		yield return new WaitForSeconds(wait);
		float currentLeftRot = ikhands.leftHandRotationWeight;
		float diffOverTime = (Mathf.Abs(currentLeftRot) - amount) / timeToBlend;
		float time = 0f;
		while (time < timeToBlend)
		{
			if (currentLeftRot > amount)
			{
				ikhands.leftHandRotationWeight -= diffOverTime * Time.deltaTime;
				time += Time.deltaTime;
				yield return null;
			}
			if (currentLeftRot < amount)
			{
				ikhands.leftHandRotationWeight += diffOverTime * Time.deltaTime;
				time += Time.deltaTime;
				yield return null;
			}
		}
	}

	private IEnumerator _BlendIKHandRightRot(float wait, float timeToBlend, float amount)
	{
		yield return new WaitForSeconds(wait);
		float currentRightRot = ikhands.rightHandRotationWeight;
		float diffOverTime = (Mathf.Abs(currentRightRot) - amount) / timeToBlend;
		float time = 0f;
		while (time < timeToBlend)
		{
			if (currentRightRot > amount)
			{
				ikhands.rightHandRotationWeight -= diffOverTime * Time.deltaTime;
				time += Time.deltaTime;
				yield return null;
			}
			if (currentRightRot < amount)
			{
				ikhands.rightHandRotationWeight += diffOverTime * Time.deltaTime;
				time += Time.deltaTime;
				yield return null;
			}
		}
	}

	private IEnumerator _BlendIKHandRight(float time, float amount)
	{
		ikhands.rightHandPositionWeight = amount;
		ikhands.rightHandRotationWeight = amount;
		yield return new WaitForSeconds(time);
	}
}
