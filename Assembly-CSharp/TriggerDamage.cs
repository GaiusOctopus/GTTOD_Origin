using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
	public float Damage;

	public string damageMethodName = "Damage";

	public GameObject hitEffect;

	public GameObject missEffect;

	private GameObject spawnedHit;

	public bool Parent;

	private bool canDamage = true;

	private void OnCollisionEnter(Collision other)
	{
		if (canDamage && other.collider.tag == "HitBox")
		{
			other.collider.SendMessage(damageMethodName, Damage, SendMessageOptions.DontRequireReceiver);
			spawnedHit = Object.Instantiate(hitEffect, base.transform.position, base.transform.rotation);
			canDamage = false;
			if (Parent)
			{
				spawnedHit.transform.parent = other.transform;
			}
			Object.Destroy(base.gameObject);
		}
		else if (canDamage && other.collider.tag != "HitBox" && other.collider.tag != "Trigger" && other.collider.tag != "Player")
		{
			spawnedHit = Object.Instantiate(missEffect, base.transform.position, base.transform.rotation);
			canDamage = false;
			if (Parent)
			{
				spawnedHit.transform.parent = other.transform;
			}
			Object.Destroy(base.gameObject);
		}
	}
}
