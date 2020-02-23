using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public bool PlayerBullet = true;

	public bool HasBalistics;

	[Header("Bullet Stats")]
	public float damage = 16f;

	public float speed = 1000f;

	public float length = 3.5f;

	public int DamageChance = 1;

	public LayerMask PotentialLayers;

	[Header("Bullet Spawns")]
	public string hitEffectTag = "HitBox";

	public GameObject hitEffect;

	public string InteractEffectTag = "Object";

	public GameObject missEffect;

	[Header("Bullet Abilities")]
	public bool ShouldParent;

	public bool SlowBullet = true;

	public bool DamageFalloff;

	[ConditionalField("DamageFalloff", null)]
	public float EffectiveRange;

	public bool TimedDetonation;

	[ConditionalField("TimedDetonation", null)]
	public float TimeToDetonate;

	[HideInInspector]
	public float BulletAssist = 0.1f;

	private Transform Target;

	private RaycastHit hit;

	private Vector3 StartingPosition;

	private SphereCollider AimAssist;

	private Rigidbody MyPhysics;

	private float Distance;

	private float StableSpeed;

	private bool HasHit;

	private bool Frozen;

	private float SpeedSave;

	private void Awake()
	{
		StartingPosition = base.transform.position;
		if (Physics.Raycast(base.transform.position, base.transform.forward, out hit, length, PotentialLayers.value))
		{
			Collide();
		}
		StableSpeed = speed;
		if (PlayerBullet)
		{
			AimAssist = base.gameObject.AddComponent<SphereCollider>();
			AimAssist.isTrigger = true;
			AimAssist.radius = BulletAssist;
		}
		if (TimedDetonation)
		{
			StartCoroutine(Detonate());
		}
		if (HasBalistics)
		{
			MyPhysics = GetComponent<Rigidbody>();
		}
	}

	private void Update()
	{
		if (Frozen)
		{
			if (speed > 0f)
			{
				speed -= Time.deltaTime * 85f;
			}
			else
			{
				speed = 0f;
			}
		}
		base.transform.Translate(Vector3.forward * Time.deltaTime * speed);
		if (Physics.Raycast(base.transform.position, base.transform.forward, out hit, length, PotentialLayers.value))
		{
			Collide();
		}
		if (DamageFalloff && !Frozen)
		{
			if (Vector3.Distance(StartingPosition, base.transform.position) >= EffectiveRange)
			{
				damage -= Time.deltaTime * 10f;
			}
			if (damage <= 1f)
			{
				Object.Destroy(base.gameObject);
			}
		}
		if (SlowBullet)
		{
			speed = StableSpeed * Time.timeScale;
		}
		if (PlayerBullet)
		{
			AimAssist.radius = BulletAssist;
		}
	}

	private void Collide()
	{
		if (Frozen)
		{
			return;
		}
		if (hit.transform.tag == hitEffectTag)
		{
			hit.collider.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
			GameObject gameObject = Object.Instantiate(hitEffect, hit.point, base.transform.rotation);
			if (ShouldParent)
			{
				gameObject.transform.parent = hit.collider.transform;
			}
			DamageChance--;
			if (DamageChance <= 0)
			{
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			Object.Instantiate(missEffect, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
			if (hit.transform.tag == InteractEffectTag || hit.transform.tag == "Interactable")
			{
				hit.collider.SendMessage("Interact");
			}
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (Frozen || ((!(other.tag == "Enemy") || !PlayerBullet) && (!(other.tag == "HitBox") || !PlayerBullet)))
		{
			return;
		}
		if (!HasHit)
		{
			other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
			GameObject gameObject = Object.Instantiate(hitEffect, other.transform.position, base.transform.rotation);
			if (ShouldParent)
			{
				gameObject.transform.parent = other.gameObject.transform;
			}
			DamageChance--;
			if (DamageChance <= 0)
			{
				HasHit = true;
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private IEnumerator Detonate()
	{
		yield return new WaitForSeconds(TimeToDetonate);
		if (!Frozen)
		{
			Object.Instantiate(missEffect, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
		}
	}

	public void Freeze()
	{
		SpeedSave = speed;
		speed = 15f;
		if (HasBalistics)
		{
			MyPhysics.isKinematic = true;
		}
		Frozen = true;
	}

	public void UnFreeze()
	{
		speed = SpeedSave;
		if (HasBalistics)
		{
			MyPhysics.isKinematic = false;
		}
		Frozen = false;
	}
}
