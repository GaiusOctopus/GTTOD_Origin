using System.Collections;
using UnityEngine;

public class Drone : MonoBehaviour
{
	[Header("Agent Settings")]
	public float Health = 100f;

	public GameObject DeathObject;

	public Transform LeftMuzzle;

	public Transform RightMuzzle;

	[Header("Weapon Settings")]
	public bool ShowWeapon;

	[ConditionalField("ShowWeapon", null)]
	public GameObject Bullet;

	[ConditionalField("ShowWeapon", null)]
	public float FireRate = 0.1f;

	[ConditionalField("ShowWeapon", null)]
	public float WeaponRange = 100f;

	[ConditionalField("ShowWeapon", null)]
	public Vector2 BurstAmount;

	[ConditionalField("ShowWeapon", null)]
	public int RoundsInMag = 15;

	[Header("Movement Settings")]
	public float MovementRange;

	public float Velocity;

	public float MaxVelocity = 10f;

	public float movementSpeed = 10f;

	[Header("IDK")]
	public float rotationalDamp = 0.5f;

	public float detectionDistance = 20f;

	public float rayCastOffset = 0.3f;

	private AudioSource Audio;

	private Transform target;

	private Transform FixedRotation;

	private Vector3 Direction;

	private Vector3 PreviousPosition;

	private Vector3 VelocitySave;

	private Rigidbody MyPhysics;

	private Respawner MyRespawner;

	private RaycastHit AimRay;

	private CrosshairScript Crosshair;

	private GameManager GM;

	private bool canFire = true;

	private bool isDead;

	private bool Stable = true;

	private bool Reloading;

	private bool Stabilizing;

	private bool SetToDie;

	private bool Frozen;

	private float TimeBetweenBursts;

	private int RoundsInBurst;

	private int MaxAmmo;

	private void Start()
	{
		GM = GameManager.GM;
		Audio = GetComponent<AudioSource>();
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		MaxAmmo = RoundsInMag;
		StartCoroutine(Fire());
		target = GameManager.GM.Player.transform;
		FixedRotation = new GameObject("FixedRotation").transform;
		Crosshair = target.GetComponent<CrosshairScript>();
		MyPhysics = GetComponent<Rigidbody>();
		MyRespawner = GetComponent<Respawner>();
	}

	private void Update()
	{
		if (!Frozen)
		{
			if (Stable)
			{
				base.transform.LookAt(target);
				LeftMuzzle.LookAt(target);
				RightMuzzle.LookAt(target);
				Pathfinding();
				WeaponBehavior();
				Move();
			}
			else if (Stabilizing)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, FixedRotation.rotation, 0.1f);
				FixedRotation.LookAt(target);
				FixedRotation.position = base.transform.position;
			}
		}
		if (SetToDie)
		{
			Die();
		}
	}

	private void Move()
	{
		Direction = base.transform.position - target.position;
		Velocity = MyPhysics.velocity.magnitude;
		if (Vector3.Distance(PreviousPosition, target.position) > Vector3.Distance(base.transform.position, target.position) || Vector3.Distance(base.transform.position, target.position) >= MovementRange)
		{
			float x = Mathf.Lerp(MyPhysics.velocity.x, 0f, 0.02f);
			float y = Mathf.Lerp(MyPhysics.velocity.y, 0f, 0.02f);
			float z = Mathf.Lerp(MyPhysics.velocity.z, 0f, 0.02f);
			MyPhysics.velocity = new Vector3(x, y, z);
			if (MyPhysics.velocity.magnitude < MaxVelocity)
			{
				MyPhysics.AddForce(Direction * (0f - movementSpeed));
				MyPhysics.AddForce(base.transform.forward * 25f);
			}
		}
		else if (MyPhysics.velocity.magnitude < MaxVelocity)
		{
			MyPhysics.AddForce(Direction * movementSpeed);
			MyPhysics.AddForce(base.transform.up * 10f);
		}
		PreviousPosition = base.transform.position;
	}

	private void Pathfinding()
	{
		Vector3 zero = Vector3.zero;
		Vector3 vector = base.transform.position - base.transform.right * rayCastOffset;
		Vector3 vector2 = base.transform.position + base.transform.right * rayCastOffset;
		Vector3 vector3 = base.transform.position + base.transform.up * rayCastOffset;
		Vector3 vector4 = base.transform.position - base.transform.up * rayCastOffset;
		Debug.DrawRay(vector, base.transform.forward * detectionDistance, Color.red);
		Debug.DrawRay(vector2, base.transform.forward * detectionDistance, Color.red);
		Debug.DrawRay(vector3, base.transform.forward * detectionDistance, Color.red);
		Debug.DrawRay(vector4, base.transform.forward * detectionDistance, Color.red);
		if (Physics.Raycast(vector, base.transform.forward, out RaycastHit hitInfo, detectionDistance))
		{
			zero += Vector3.right;
		}
		else if (Physics.Raycast(vector2, base.transform.forward, out hitInfo, detectionDistance))
		{
			zero -= Vector3.right;
		}
		if (Physics.Raycast(vector3, base.transform.forward, out hitInfo, detectionDistance))
		{
			zero -= Vector3.up;
		}
		else if (Physics.Raycast(vector4, base.transform.forward, out hitInfo, detectionDistance))
		{
			zero += Vector3.up;
		}
		if (zero != Vector3.zero)
		{
			base.transform.Rotate(zero * 5f * Time.deltaTime);
		}
		else
		{
			Turn();
		}
	}

	private void Turn()
	{
		base.transform.position += base.transform.forward * 5f * Time.deltaTime;
		Quaternion b = Quaternion.LookRotation(target.position - base.transform.position);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, rotationalDamp);
	}

	private void WeaponBehavior()
	{
		if (Physics.Raycast(RightMuzzle.transform.position, RightMuzzle.transform.forward, out AimRay, WeaponRange) && AimRay.collider.tag == "Player" && !Reloading)
		{
			if (RoundsInBurst >= 1 && canFire)
			{
				StartCoroutine(Fire());
			}
			if (RoundsInBurst <= 0 && canFire)
			{
				StartCoroutine(FireWait());
			}
			if (RoundsInMag <= 0)
			{
				Reloading = true;
				StartCoroutine(Reload());
			}
		}
	}

	private IEnumerator Fire()
	{
		Object.Instantiate(Bullet, RightMuzzle.position, RightMuzzle.rotation);
		Object.Instantiate(Bullet, LeftMuzzle.position, LeftMuzzle.rotation);
		Audio.Play();
		canFire = false;
		RoundsInBurst--;
		RoundsInMag--;
		yield return new WaitForSeconds(FireRate);
		canFire = true;
	}

	private IEnumerator FireWait()
	{
		canFire = false;
		TimeBetweenBursts = Random.Range(0.5f, 2f);
		yield return new WaitForSeconds(TimeBetweenBursts);
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		canFire = true;
	}

	private IEnumerator Reload()
	{
		yield return new WaitForSeconds(4f);
		RoundsInMag = MaxAmmo;
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		Reloading = false;
	}

	public void Damage(float damage)
	{
		if (!GM.AntiLife)
		{
			Health -= damage;
			Crosshair.Hitmarker();
			if (Stable)
			{
				Stable = false;
				StartCoroutine(DamageCooldown());
			}
			MyPhysics.velocity = Vector3.zero;
			MyPhysics.AddForce(base.transform.forward * -7500f);
			MyPhysics.AddTorque(LeftMuzzle.transform.right * 100000f);
			if (Health <= 0f && !isDead)
			{
				Die();
			}
		}
		else
		{
			Die();
		}
	}

	public IEnumerator DamageCooldown()
	{
		yield return new WaitForSeconds(2.5f);
		MyPhysics.velocity = Vector3.zero;
		Stabilizing = true;
		yield return new WaitForSeconds(1f);
		Stabilizing = false;
		Stable = true;
	}

	public void Die()
	{
		if (!Frozen)
		{
			Object.Destroy(base.gameObject);
			if (MyRespawner != null)
			{
				MyRespawner.RemoveEnemy();
			}
			isDead = true;
			Object.Instantiate(DeathObject, base.transform.position, base.transform.rotation);
		}
		else
		{
			SetToDie = true;
		}
	}

	public void Stagger()
	{
		MyPhysics.velocity = Vector3.zero;
		MyPhysics.AddForce(base.transform.forward * -7500f);
		MyPhysics.AddTorque(LeftMuzzle.transform.right * 100000f);
	}

	public void Ragdoll()
	{
		MyPhysics.velocity = Vector3.zero;
		MyPhysics.AddForce(base.transform.forward * -7500f);
		MyPhysics.AddTorque(LeftMuzzle.transform.right * 100000f);
	}

	public void Freeze()
	{
		VelocitySave = MyPhysics.velocity;
		MyPhysics.velocity = Vector3.zero;
		MyPhysics.isKinematic = true;
		Frozen = true;
	}

	public void UnFreeze()
	{
		MyPhysics.isKinematic = false;
		MyPhysics.velocity = VelocitySave;
		Frozen = false;
	}
}
