using System.Collections.Generic;
using UnityEngine;

public class ExplosionShake : MonoBehaviour
{
	public float radius = 5f;

	public float power = 10f;

	public float Damage;

	public int HitBudget = 1;

	public GameObject hitEffect;

	public bool Ragdoll;

	public bool Stagger;

	public bool PlayerDamage;

	private bool canDamage = true;

	private SphereCollider RadiusCollider;

	[HideInInspector]
	public List<GameObject> HitObjects;

	private GameObject Player;

	private void Start()
	{
		RadiusCollider = GetComponent<SphereCollider>();
		RadiusCollider.radius = radius;
		Player = GameManager.GM.Player;
		Collider[] array = Physics.OverlapSphere(base.transform.position, radius);
		if (power > 0f)
		{
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Rigidbody component = collider.GetComponent<Rigidbody>();
				if (component != null)
				{
					if (collider.tag == "Player" && component.velocity.y <= 0f)
					{
						component.velocity = new Vector3(component.velocity.x, 0f, component.velocity.z);
					}
					component.AddExplosionForce(power, base.transform.position, radius, 1f);
				}
			}
		}
		if (Vector3.Distance(Player.transform.position, base.transform.position) <= radius && PlayerDamage)
		{
			Player.SendMessage("TrueDamage", Damage);
		}
		Object.Destroy(base.gameObject, 0.1f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(other.tag == "WeakPoint") || !canDamage || HitObjects.Contains(other.gameObject))
		{
			return;
		}
		if (HitBudget > 0)
		{
			HitBudget--;
			Object.Instantiate(hitEffect, other.gameObject.transform.position, base.transform.rotation);
			other.SendMessage("Damage", Damage / (Vector3.Distance(base.transform.position, other.transform.position) / 2f), SendMessageOptions.DontRequireReceiver);
			if (Ragdoll)
			{
				other.SendMessage("RagdollAgent", SendMessageOptions.DontRequireReceiver);
			}
			if (Stagger)
			{
				other.SendMessage("Stagger", SendMessageOptions.DontRequireReceiver);
			}
			HitObjects.Add(other.gameObject);
		}
		else
		{
			canDamage = false;
		}
	}
}
