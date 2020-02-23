using UnityEngine;

public class TriggerDamageSimple : MonoBehaviour
{
	public float Damage;

	public int HitBudget = 1;

	public GameObject HitEffect;

	[ConditionalField("DestroyOnDamage", null)]
	public GameObject ObjectToDestroy;

	[ConditionalField("KillAI", null)]
	public Respawner AI;

	public bool Stagger;

	public bool Ragdoll;

	public bool Parent;

	public bool KillAI;

	public bool DestroyOnDamage = true;

	public bool PlayerDamage;

	private Transform Hit;

	private bool hasHit;

	private void OnTriggerEnter(Collider other)
	{
		if (HitBudget > 0 && !PlayerDamage)
		{
			HitBudget--;
			if (other.tag == "HitBox" && !hasHit && !PlayerDamage)
			{
				other.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
				if (Stagger)
				{
					other.SendMessage("Stagger", SendMessageOptions.DontRequireReceiver);
				}
				if (Ragdoll)
				{
					other.SendMessage("RagdollAgent", SendMessageOptions.DontRequireReceiver);
				}
				Hit = Object.Instantiate(HitEffect, other.transform.position, base.transform.rotation).transform;
				if (Parent)
				{
					Hit.parent = other.transform;
				}
				if (KillAI)
				{
					AI.RemoveEnemy();
				}
				if (DestroyOnDamage)
				{
					hasHit = true;
					if (ObjectToDestroy != null)
					{
						Object.Destroy(ObjectToDestroy);
					}
					else
					{
						Object.Destroy(base.gameObject);
					}
				}
			}
		}
		if (!PlayerDamage)
		{
			return;
		}
		if (other.tag == "Player" && !hasHit)
		{
			other.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
			Hit = Object.Instantiate(HitEffect, base.transform.position, base.transform.rotation).transform;
			if (Parent)
			{
				Hit.parent = other.transform;
			}
			if (KillAI)
			{
				AI.RemoveEnemy();
			}
			if (DestroyOnDamage)
			{
				hasHit = true;
				if (ObjectToDestroy != null)
				{
					Object.Destroy(ObjectToDestroy);
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
		}
		if (other.tag == "Interactable")
		{
			other.SendMessage("Interact");
		}
	}
}
