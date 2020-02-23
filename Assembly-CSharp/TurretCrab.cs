using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TurretCrab : MonoBehaviour
{
	public AudioClip FireSFX;

	public AudioClip JumpSFX;

	public float Health = 100f;

	public GameObject DeathObject;

	public GameObject TriggerDamage;

	public LayerMask AvailableLayers;

	[Header("Weapon Settings")]
	public bool ShowWeapon;

	[ConditionalField("ShowWeapon", null)]
	public GameObject Bullet;

	[ConditionalField("ShowWeapon", null)]
	public Transform Muzzle;

	[ConditionalField("ShowWeapon", null)]
	public float FireRate = 0.1f;

	[ConditionalField("ShowWeapon", null)]
	public Vector2 BurstAmount;

	private GameManager GM;

	private Animator Anim;

	private NavMeshAgent Agent;

	private Rigidbody MyPhysics;

	private AudioSource Audio;

	private Respawner MyRespawner;

	private CrosshairScript Crosshair;

	private RaycastHit FloorHit;

	private RaycastHit GroundCheck;

	private RaycastHit AimRay;

	private Transform BreadCrumb;

	private Transform Target;

	private Vector3 LookPosition;

	private Vector3 VelocitySave;

	private Quaternion LookRotation;

	private float CrumbTimer;

	private float DistanceToJump;

	private float TurretTimer;

	private float TimeBetweenBursts;

	private float SpeedSave;

	private int RoundsInBurst;

	private int MaxAmmo;

	private bool CanJump = true;

	private bool Grounded = true;

	private bool TurretMode;

	private bool Jumping;

	private bool canFire = true;

	private bool Frozen;

	private bool Dead;

	private bool SetToDie;

	private void Start()
	{
		GM = GameManager.GM;
		Target = GameManager.GM.Player.transform;
		BreadCrumb = new GameObject("MyBreadCrumb").transform;
		Anim = GetComponent<Animator>();
		Agent = GetComponent<NavMeshAgent>();
		MyPhysics = GetComponent<Rigidbody>();
		Audio = GetComponent<AudioSource>();
		MyRespawner = GetComponent<Respawner>();
		Crosshair = GM.Player.GetComponent<CrosshairScript>();
		Agent.updateRotation = false;
		DistanceToJump = Random.Range(6f, 12f);
	}

	private void Update()
	{
		if (!Frozen)
		{
			CheckGround();
			if (Vector3.Distance(base.transform.position, Target.position) > 1f && Grounded && !TurretMode)
			{
				FacePlayer();
				Anim.SetInteger("MoveState", 1);
				if (CrumbTimer <= 0f && Agent.isOnNavMesh)
				{
					Physics.Raycast(Target.position, base.transform.up * -1f, out FloorHit, 100f);
					BreadCrumb.position = new Vector3(Target.position.x, FloorHit.point.y, Target.position.z);
					Agent.SetDestination(BreadCrumb.position);
					CrumbTimer = 0.5f;
				}
				else
				{
					CrumbTimer -= Time.deltaTime;
				}
				if (CanJump)
				{
					if (Vector3.Distance(base.transform.position, Target.position) < DistanceToJump)
					{
						StartCoroutine(Jump());
					}
				}
				else if (TurretTimer <= 0f)
				{
					AttemptTurret();
					TurretTimer = 2.5f;
				}
				else
				{
					TurretTimer -= Time.deltaTime;
				}
			}
			else if (TurretMode)
			{
				FacePlayer();
				Muzzle.LookAt(new Vector3(Target.position.x, Target.position.y - 0.25f, Target.position.z));
				WeaponBehavior();
				Anim.SetInteger("MoveState", 2);
			}
			else
			{
				Anim.SetInteger("MoveState", 0);
			}
		}
		if (SetToDie)
		{
			Die();
		}
	}

	private void WeaponBehavior()
	{
		if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out AimRay, 25f) && AimRay.collider.tag == "Player")
		{
			if (RoundsInBurst >= 1 && canFire)
			{
				StartCoroutine(Fire());
			}
			if (RoundsInBurst <= 0 && canFire)
			{
				StartCoroutine(FireWait());
			}
		}
	}

	private IEnumerator Fire()
	{
		Object.Instantiate(Bullet, Muzzle.position, Muzzle.rotation);
		Audio.clip = FireSFX;
		Audio.Play();
		canFire = false;
		RoundsInBurst--;
		yield return new WaitForSeconds(FireRate);
		canFire = true;
	}

	private IEnumerator FireWait()
	{
		canFire = false;
		TimeBetweenBursts = Random.Range(0.25f, 1.5f);
		yield return new WaitForSeconds(TimeBetweenBursts);
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		canFire = true;
	}

	private void FacePlayer()
	{
		LookPosition = Target.position - base.transform.position;
		LookPosition.y = 0f;
		LookRotation = Quaternion.LookRotation(LookPosition);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, LookRotation, 0.1f);
	}

	private void CheckGround()
	{
		if (!Jumping)
		{
			if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z + 1f), base.transform.up * -1f, out GroundCheck, 2f, AvailableLayers))
			{
				Grounded = true;
				Agent.enabled = true;
				MyPhysics.isKinematic = true;
			}
			else
			{
				Grounded = false;
				Agent.enabled = false;
				MyPhysics.isKinematic = false;
			}
		}
	}

	private IEnumerator Jump()
	{
		CanJump = false;
		Jumping = true;
		Anim.SetTrigger("Jump");
		Agent.enabled = false;
		MyPhysics.isKinematic = false;
		MyPhysics.velocity = Vector3.zero;
		MyPhysics.velocity = base.transform.forward * 20f + base.transform.up * 8f;
		DistanceToJump = Random.Range(6f, 12f);
		Audio.clip = JumpSFX;
		Audio.Play();
		TriggerDamage.SetActive(value: true);
		yield return new WaitForSeconds(1f);
		Jumping = false;
		yield return new WaitForSeconds(2f);
		CanJump = true;
		TriggerDamage.SetActive(value: false);
	}

	private void AttemptTurret()
	{
		if (Random.Range(-1f, 1f) > 0.5f)
		{
			StartCoroutine(TriggerTurretMode());
		}
	}

	private IEnumerator TriggerTurretMode()
	{
		TurretMode = true;
		BreadCrumb.position = base.transform.position;
		Agent.SetDestination(BreadCrumb.position);
		yield return new WaitForSeconds(8f);
		TurretMode = false;
	}

	public void Damage(float damage)
	{
		if (!GM.AntiLife)
		{
			Health -= damage;
			Crosshair.Hitmarker();
			if (Health <= 0f && !Dead)
			{
				Die();
			}
		}
		else
		{
			Die();
		}
	}

	public void Die()
	{
		if (!Frozen)
		{
			SetToDie = false;
			Object.Instantiate(DeathObject, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
			Dead = true;
			Crosshair.Kill();
			if (MyRespawner != null)
			{
				MyRespawner.RemoveEnemy();
			}
			GameManager.GM.PlayerKillsStat++;
			GameManager.GM.UpdateStats();
		}
		else
		{
			SetToDie = true;
		}
	}

	public void Stagger()
	{
		Die();
	}

	public void Freeze()
	{
		Anim.speed = 0f;
		SpeedSave = Agent.speed;
		Agent.speed = 0f;
		VelocitySave = MyPhysics.velocity;
		MyPhysics.velocity = Vector3.zero;
		MyPhysics.isKinematic = true;
		Frozen = true;
	}

	public void UnFreeze()
	{
		Anim.speed = 1f;
		Agent.speed = SpeedSave;
		MyPhysics.isKinematic = false;
		MyPhysics.velocity = VelocitySave;
		Frozen = false;
	}
}
