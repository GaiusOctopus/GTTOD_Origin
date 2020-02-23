using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Infantry : MonoBehaviour
{
	[Header("Set-Up")]
	public bool ShowSetUp;

	[ConditionalField("ShowSetUp", null)]
	public float Health;

	[ConditionalField("ShowSetUp", null)]
	public Transform ForcePoint;

	[ConditionalField("ShowSetUp", null)]
	public Rigidbody Core;

	[ConditionalField("ShowSetUp", null)]
	public Transform RightFoot;

	[ConditionalField("ShowSetUp", null)]
	public Transform LeftFoot;

	[ConditionalField("ShowSetUp", null)]
	public GameObject Dirt;

	[ConditionalField("ShowSetUp", null)]
	public GameObject DashObject;

	[ConditionalField("ShowSetUp", null)]
	public GameObject DeathObject;

	[ConditionalField("ShowSetUp", null)]
	public GameObject WeaponHolder;

	[ConditionalField("ShowSetUp", null)]
	public bool CanStagger = true;

	[ConditionalField("ShowSetUp", null)]
	public bool HasEquipment;

	[Header("AI Type")]
	public bool ShowAIType;

	[ConditionalField("ShowAIType", null)]
	public int MovementState = 1;

	[ConditionalField("ShowAIType", null)]
	public bool Gunner;

	[ConditionalField("ShowAIType", null)]
	public bool Brawler;

	[ConditionalField("ShowAIType", null)]
	public bool Rusher;

	[Header("Weapon Settings")]
	public bool ShowWeapon;

	[ConditionalField("ShowWeapon", null)]
	public bool HasGun = true;

	[ConditionalField("ShowWeapon", null)]
	public GameObject Bullet;

	[ConditionalField("ShowWeapon", null)]
	public Transform Muzzle;

	[ConditionalField("ShowWeapon", null)]
	public float FireRate = 0.1f;

	[ConditionalField("ShowWeapon", null)]
	public float Range = 100f;

	[ConditionalField("ShowWeapon", null)]
	public Vector2 BurstAmount;

	[ConditionalField("ShowWeapon", null)]
	public int RoundsInMag = 15;

	[ConditionalField("ShowWeapon", null)]
	public GameObject MeleeObject;

	[Header("Equipment Settings")]
	public bool ShowEquipmentSettings;

	[ConditionalField("ShowEquipmentSettings", null)]
	public GameObject Equipment;

	[ConditionalField("ShowEquipmentSettings", null)]
	public GameObject EquipmentObject;

	[ConditionalField("ShowEquipmentSettings", null)]
	public Transform EquipmentTransform;

	[Header("Infantry Settings")]
	public bool ShowInfantrySettings;

	[ConditionalField("ShowInfantrySettings", null)]
	public float MeleeTime = 0.5f;

	public GameObject[] WeaponsToDrop;

	private GameManager GM;

	private CrosshairScript Crosshair;

	private Respawner MyRespawner;

	private NavMeshAgent Agent;

	private Animator Anim;

	private RotateToAimGunScript Aimer;

	private RagdollInfantry Ragdoller;

	private AITalker Talker;

	private AudioSource Audio;

	private Transform Target;

	private Transform BreadCrumb;

	private Transform LastDashEffect;

	private Vector3 RelativePosition;

	private Vector3 LookPosition;

	private Quaternion LookRotation;

	private RaycastHit FloorHit;

	private RaycastHit AimRay;

	private RaycastHit DodgeRay;

	private float ZMovement;

	private float XMovement;

	private float CrumbTimer;

	private float AgentUpdateSpeed;

	private float StoppingDistance;

	private float IdleTime;

	private float TimeToStand = 5f;

	private float TimeBetweenBursts;

	private float DashChance;

	private float SpeedSave;

	private float DamageModifier;

	private bool Active = true;

	private bool Ragdolled;

	private bool Dead;

	private bool Aggressive;

	private bool canFire = true;

	private bool canDodge = true;

	private bool CanMelee = true;

	private bool Reloading;

	private bool SetToDie;

	private bool Frozen;

	private bool Animating;

	private bool CanBeInterupted;

	private bool StandingUp;

	private bool HasSpawnedDeathObject;

	private bool HasRemoved;

	private int RoundsInBurst;

	private int MaxAmmo;

	private Rigidbody[] Rigidbodies;

	public void Start()
	{
		GM = GameManager.GM;
		Target = GM.Player.transform;
		Crosshair = Target.GetComponent<CrosshairScript>();
		MyRespawner = GetComponent<Respawner>();
		Anim = GetComponent<Animator>();
		Agent = GetComponent<NavMeshAgent>();
		Aimer = GetComponent<RotateToAimGunScript>();
		Ragdoller = GetComponent<RagdollInfantry>();
		Talker = GetComponent<AITalker>();
		Audio = GetComponent<AudioSource>();
		DamageModifier = GM.EnemyDamageModifier;
		if (!Brawler && !Rusher)
		{
			StoppingDistance = Random.Range(4f, 12f);
			AgentUpdateSpeed = Random.Range(4f, 10f);
			IdleTime = Random.Range(2f, 6f);
			DashChance = Random.Range(0.05f, 0.15f);
		}
		else
		{
			StoppingDistance = 2.5f;
			AgentUpdateSpeed = 2f;
			IdleTime = 0.5f;
			DashChance = 0f;
		}
		BreadCrumb = new GameObject("MyBreadCrumb").transform;
		Anim.speed = 1.25f;
		Agent.updateRotation = false;
		MaxAmmo = RoundsInMag;
		Rigidbodies = GetComponentsInChildren<Rigidbody>();
		Rigidbody[] rigidbodies = Rigidbodies;
		for (int i = 0; i < rigidbodies.Length; i++)
		{
			rigidbodies[i].GetComponent<Rigidbody>().isKinematic = true;
		}
		if (HasEquipment)
		{
			StartCoroutine(EquipmentChance());
		}
	}

	public void Update()
	{
		if (Active && !Ragdolled && !Frozen && Ragdoller.FinishedRagdoll)
		{
			FacePlayer();
			if (HasGun)
			{
				WeaponBehavior();
			}
			Anim.SetFloat("Z", ZMovement);
			Anim.SetFloat("X", XMovement);
			Anim.SetInteger("MovementState", MovementState);
			if (Vector3.Distance(base.transform.position, Target.position) > StoppingDistance && Agent.isOnNavMesh)
			{
				RelativePosition = base.transform.InverseTransformPoint(BreadCrumb.position);
				ZMovement = Mathf.Lerp(ZMovement, RelativePosition.z, 0.1f);
				XMovement = Mathf.Lerp(XMovement, RelativePosition.x, 0.1f);
				if (CrumbTimer <= 0f)
				{
					Physics.Raycast(Target.position, base.transform.up * -1f, out FloorHit, 100f);
					BreadCrumb.position = new Vector3(Target.position.x, FloorHit.point.y, Target.position.z);
					if (Agent.isOnNavMesh)
					{
						Agent.SetDestination(BreadCrumb.position);
					}
					if (!Brawler)
					{
						CrumbTimer = AgentUpdateSpeed;
					}
					else
					{
						CrumbTimer = 0.25f;
					}
				}
				else
				{
					CrumbTimer -= Time.deltaTime;
				}
			}
			else
			{
				ZMovement = Mathf.Lerp(ZMovement, 0f, 0.1f);
				XMovement = Mathf.Lerp(XMovement, 0f, 0.1f);
				CrumbTimer = IdleTime;
				if (Agent.isOnNavMesh)
				{
					Agent.SetDestination(base.transform.position);
				}
				BreadCrumb.position = base.transform.position;
				if (Brawler && CanMelee)
				{
					StartCoroutine(AnimateAgent("Melee 1", MeleeTime, CanInterupt: true));
				}
			}
		}
		else if (Ragdolled && Core.velocity.magnitude < 2.5f)
		{
			if (TimeToStand <= 0f)
			{
				StartCoroutine(UnRagdoll());
				Ragdolled = false;
			}
			else
			{
				TimeToStand -= Time.deltaTime;
			}
		}
		if (SetToDie)
		{
			Die();
		}
		if (Frozen || Animating || !HasGun || StandingUp || !Ragdoller.FinishedRagdoll)
		{
			Aimer.isEnabled = false;
		}
		else
		{
			Aimer.isEnabled = true;
		}
	}

	public void FootR()
	{
		if (ZMovement > 0.5f || ZMovement < -0.5f || XMovement > 0.5f || XMovement < -0.5f)
		{
			Object.Instantiate(Dirt, RightFoot.transform.position, Quaternion.identity);
		}
	}

	public void FootL()
	{
		if (ZMovement > 0.5f || ZMovement < -0.5f || XMovement > 0.5f || XMovement < -0.5f)
		{
			Object.Instantiate(Dirt, LeftFoot.transform.position, Quaternion.identity);
		}
	}

	public void HitPlayer()
	{
		Object.Instantiate(MeleeObject, new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z), base.transform.rotation);
	}

	private void FacePlayer()
	{
		LookPosition = Target.position - base.transform.position;
		LookPosition.y = 0f;
		LookRotation = Quaternion.LookRotation(LookPosition);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, LookRotation, 0.1f);
	}

	private void WeaponBehavior()
	{
		Muzzle.LookAt(new Vector3(Target.position.x, Target.position.y - 0.5f, Target.position.z));
		if (Physics.Raycast(Muzzle.transform.position, Muzzle.transform.forward, out AimRay, Range) && AimRay.collider.tag == "Player" && !Reloading)
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
		Object.Instantiate(Bullet, Muzzle.position, Muzzle.rotation);
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
		TimeBetweenBursts = Random.Range(1f, 4f);
		yield return new WaitForSeconds(TimeBetweenBursts);
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		canFire = true;
	}

	private IEnumerator Reload()
	{
		StartCoroutine(AnimateAgent("Reload", 3.5f, CanInterupt: false));
		if (Random.Range(-1f, 1f) >= 0.75f)
		{
			Talker.SayRandomReload();
		}
		yield return new WaitForSeconds(3.5f);
		RoundsInMag = MaxAmmo;
		RoundsInBurst = Random.Range(Mathf.RoundToInt(BurstAmount.x), Mathf.RoundToInt(BurstAmount.y));
		Reloading = false;
	}

	private void Dodge()
	{
		if (Active && !Ragdolled && !Animating)
		{
			StartCoroutine(DodgeCooldown());
			float d = (!(Random.Range(-100f, 100f) < 0f)) ? 1f : (-1f);
			LastDashEffect = Object.Instantiate(DashObject, new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z), base.transform.rotation).transform;
			if (Physics.Raycast(new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z), base.transform.right * d, out DodgeRay, 5f))
			{
				base.transform.position = new Vector3(DodgeRay.point.x, DodgeRay.point.y - 0.5f, DodgeRay.point.z);
			}
			else
			{
				base.transform.position = base.transform.position + base.transform.right * 5f * d;
			}
			LastDashEffect.LookAt(base.transform.position);
		}
	}

	private IEnumerator DodgeCooldown()
	{
		canDodge = false;
		yield return new WaitForSeconds(1.5f);
		canDodge = true;
	}

	public void Damage(float damage)
	{
		if (!GM.AntiLife)
		{
			Health -= damage * DamageModifier;
			Flinch();
			Crosshair.Hitmarker();
			if (Health <= 0f)
			{
				Die();
			}
			if (Random.Range(0f, 1f) <= DashChance && !Dead && canDodge)
			{
				Dodge();
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
			Object.Destroy(BreadCrumb.gameObject);
			Object.Destroy(base.gameObject);
			SetToDie = false;
			if (Dead)
			{
				return;
			}
			if (!Ragdolled)
			{
				if (!HasSpawnedDeathObject)
				{
					Object.Instantiate(DeathObject, base.transform.position, base.transform.rotation);
					HasSpawnedDeathObject = true;
				}
			}
			else if (!HasSpawnedDeathObject)
			{
				Object.Instantiate(DeathObject, Core.transform.position, Core.transform.rotation);
				HasSpawnedDeathObject = true;
			}
			DropWeapon();
			if (MyRespawner != null && !HasRemoved)
			{
				MyRespawner.RemoveEnemy();
				HasRemoved = true;
			}
			GameManager.GM.PlayerKillsStat++;
			GameManager.GM.UpdateStats();
			Dead = true;
		}
		else
		{
			SetToDie = true;
		}
	}

	public void DropWeapon()
	{
		if (HasGun)
		{
			int num = Random.Range(0, WeaponsToDrop.Length);
			Object.Instantiate(WeaponsToDrop[num], new Vector3(base.transform.position.x, base.transform.position.y + 1f, base.transform.position.z), base.transform.rotation);
		}
	}

	public void Flinch()
	{
		if (CanStagger)
		{
			Anim.SetTrigger("Flinch");
		}
	}

	public void Stagger()
	{
		if (CanStagger)
		{
			LookPosition = Target.position - base.transform.position;
			LookPosition.y = 0f;
			base.transform.rotation = Quaternion.LookRotation(LookPosition);
			StartCoroutine(AnimateAgent("Stagger", 2f, CanInterupt: false));
		}
	}

	public void KnockDown()
	{
		MovementState = 2;
		CanStagger = false;
		CanMelee = false;
	}

	public void DodgeRoll()
	{
		if (CanStagger && !Brawler && !Rusher && (double)(float)Random.Range(-1, 1) > -0.25)
		{
			StartCoroutine(AnimateAgent("DodgeRoll", 1f, CanInterupt: false));
		}
	}

	public void PrepGrenade()
	{
		WeaponHolder.SetActive(value: false);
		EquipmentObject.SetActive(value: true);
	}

	public void ThrowGrenade()
	{
		Object.Instantiate(Equipment, EquipmentTransform.position, EquipmentTransform.rotation);
	}

	public IEnumerator EquipmentChance()
	{
		if (HasEquipment)
		{
			float seconds = Random.Range(5, 20);
			yield return new WaitForSeconds(seconds);
			if (Random.Range(-1f, 1f) > 0f)
			{
				UseEquipment();
			}
			StartCoroutine(EquipmentChance());
		}
	}

	public void UseEquipment()
	{
		StartCoroutine(AnimateAgent("UseEquipment", 1f, CanInterupt: false));
	}

	public void SoundOff()
	{
		HasSpawnedDeathObject = true;
		Audio.Play();
	}

	public IEnumerator AnimateAgent(string Animation, float AnimationTime, bool CanInterupt)
	{
		if (Animating && Agent.isOnNavMesh)
		{
			if (CanBeInterupted)
			{
				Anim.SetTrigger(Animation);
				Animating = true;
				Active = false;
				Agent.isStopped = true;
				if (CanInterupt)
				{
					CanBeInterupted = true;
				}
				else
				{
					CanBeInterupted = false;
				}
				yield return new WaitForSeconds(AnimationTime);
				Active = true;
				Animating = false;
				Agent.isStopped = false;
			}
			yield break;
		}
		Anim.SetTrigger(Animation);
		Animating = true;
		Active = false;
		if (Agent.isOnNavMesh)
		{
			Agent.isStopped = true;
		}
		if (CanInterupt)
		{
			CanBeInterupted = true;
		}
		else
		{
			CanBeInterupted = false;
		}
		yield return new WaitForSeconds(AnimationTime);
		Active = true;
		Animating = false;
		if (Agent.isOnNavMesh)
		{
			Agent.isStopped = false;
		}
	}

	public void Ragdoll()
	{
		if (!Ragdolled && !Frozen && CanStagger)
		{
			Disarm();
			Ragdoller.RagdollIn();
			if (Agent.isOnNavMesh)
			{
				Agent.isStopped = true;
			}
			Active = false;
			Ragdolled = true;
			TimeToStand = 5f;
			Talker.Typer.gameObject.SetActive(value: false);
			Rigidbody[] rigidbodies = Rigidbodies;
			foreach (Rigidbody obj in rigidbodies)
			{
				obj.GetComponent<Rigidbody>().isKinematic = false;
				obj.AddExplosionForce(20f * GM.Player.GetComponent<InventoryScript>().KickStrength, ForcePoint.position, 10f, 0.35f, ForceMode.Impulse);
			}
		}
	}

	public IEnumerator UnRagdoll()
	{
		Ragdoller.RagdollOut();
		Ragdolled = false;
		StandingUp = true;
		yield return new WaitForSeconds(2f);
		Talker.Typer.gameObject.SetActive(value: true);
		if (Agent.isOnNavMesh)
		{
			Agent.isStopped = false;
		}
		Anim.enabled = true;
		Active = true;
		yield return new WaitForSeconds(3f);
		StandingUp = false;
	}

	public void Disarm()
	{
		MovementState = 0;
		HasGun = false;
		Brawler = true;
		Gunner = false;
		HasEquipment = false;
		StoppingDistance = 2.5f;
		AgentUpdateSpeed = 1f;
		IdleTime = 0.25f;
		DashChance = 0f;
		WeaponHolder.SetActive(value: false);
		Agent.speed = 3f;
		Agent.acceleration = 25f;
		Talker.Angry = true;
	}

	public void Freeze()
	{
		Anim.speed = 0f;
		SpeedSave = Agent.speed;
		Agent.speed = 0f;
		Frozen = true;
		Active = false;
	}

	public void UnFreeze()
	{
		Anim.speed = 1f;
		Agent.speed = SpeedSave;
		Frozen = false;
		Active = true;
	}
}
