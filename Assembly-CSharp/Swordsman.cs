using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Swordsman : MonoBehaviour
{
	[Header("Agent Set-Up")]
	public bool ShowSetUp;

	[ConditionalField("ShowSetUp", null)]
	public Animator Body;

	[ConditionalField("ShowSetUp", null)]
	public GameObject DeathObject;

	[ConditionalField("ShowSetUp", null)]
	public Vector2 AgentMaxHealth;

	[Header("Private Variables")]
	private Vector3 LookPosition;

	private Quaternion LookRotation;

	private Transform Target;

	private NavMeshAgent Agent;

	private CrosshairScript Crosshair;

	private Respawner MyRespawner;

	private AudioSource Audio;

	private GameManager GM;

	private bool Frozen;

	private bool Dead;

	private bool Attacking;

	private bool Active = true;

	private bool SetToDie;

	private float Health;

	private float SpeedSave;

	private int StateID;

	private void Start()
	{
		GM = GameManager.GM;
		Target = GM.Player.transform;
		Crosshair = Target.GetComponent<CrosshairScript>();
		MyRespawner = GetComponent<Respawner>();
		Audio = GetComponent<AudioSource>();
		Agent = GetComponent<NavMeshAgent>();
		Agent.updateRotation = true;
		Health = Random.Range(AgentMaxHealth.x, AgentMaxHealth.y);
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, Target.position) > 100f)
		{
			MyRespawner.RemoveEnemy();
			Object.Destroy(base.gameObject);
		}
		Body.SetInteger("MovementState", StateID);
		if (Active && !Frozen)
		{
			if (Vector3.Distance(base.transform.position, Target.position) > 2f && !Attacking)
			{
				StateID = 0;
				Advance();
			}
			else
			{
				StateID = 1;
				Melee();
			}
		}
		if (SetToDie)
		{
			Die();
		}
	}

	private void Advance()
	{
		Agent.SetDestination(Target.position);
	}

	private void Melee()
	{
		if (!Attacking)
		{
			Attacking = true;
			StartCoroutine(AttackCooldown());
		}
	}

	private IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(2f);
		Attacking = false;
		StateID = 0;
	}

	public void Damage(float damage)
	{
		if (!GM.AntiLife)
		{
			Health -= damage;
			Flinch();
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

	public void Headshot(float damage)
	{
		if (!GM.AntiLife)
		{
			Health -= damage;
			Flinch();
			Crosshair.Headshot();
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

	private void FacePlayer()
	{
		LookPosition = Target.position - base.transform.position;
		LookPosition.y = 0f;
		LookRotation = Quaternion.LookRotation(LookPosition);
		base.transform.rotation = LookRotation;
	}

	public void Stagger()
	{
		StartCoroutine(StaggerAgent());
	}

	public IEnumerator StaggerAgent()
	{
		Active = false;
		Agent.enabled = false;
		StateID = 3;
		FacePlayer();
		yield return new WaitForSeconds(1f);
		Active = true;
		Agent.enabled = true;
	}

	private void Flinch()
	{
		Body.SetTrigger("Flinch");
	}

	public void Freeze()
	{
		Body.speed = 0f;
		SpeedSave = Agent.speed;
		Agent.speed = 0f;
		Frozen = true;
		Active = false;
	}

	public void UnFreeze()
	{
		Body.speed = 1f;
		Agent.speed = SpeedSave;
		Frozen = false;
		Active = true;
	}
}
