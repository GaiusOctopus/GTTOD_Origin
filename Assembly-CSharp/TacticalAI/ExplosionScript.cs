using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TacticalAI
{
	public class ExplosionScript : MonoBehaviour
	{
		public string damageMethodName = "Damage";

		public float explosionRadius = 5f;

		public float explosionPower = 10f;

		public float upwardsPower = 10f;

		public float damage = 200f;

		public LayerMask layerMask;

		public float explosionTime = 5f;

		public bool scaleDamageByDistance = true;

		public bool showBlastRadius;

		public bool shouldDoSingleHitboxDamage;

		private void Awake()
		{
			StartCoroutine(Go());
		}

		private IEnumerator Go()
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, explosionRadius);
			List<Rigidbody> hitBodies = new List<Rigidbody>();
			float num = damage;
			foreach (Collider collider in array)
			{
				if (!Physics.Linecast(base.transform.position, collider.transform.position, layerMask))
				{
					if (scaleDamageByDistance)
					{
						num = damage * Vector3.Distance(base.transform.position, collider.transform.position) / explosionRadius;
					}
					if (shouldDoSingleHitboxDamage)
					{
						collider.GetComponent<Collider>().SendMessage("SingleHitBoxDamage", num, SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						collider.GetComponent<Collider>().SendMessage(damageMethodName, num, SendMessageOptions.DontRequireReceiver);
					}
					if ((bool)collider.GetComponent<Rigidbody>())
					{
						hitBodies.Add(collider.GetComponent<Rigidbody>());
					}
				}
			}
			yield return null;
			for (int i = 0; i < hitBodies.Count; i++)
			{
				if ((bool)hitBodies[i])
				{
					hitBodies[i].AddExplosionForce(explosionPower, base.transform.position, explosionRadius, upwardsPower, ForceMode.Impulse);
				}
			}
			Object.Destroy(base.gameObject, explosionTime);
		}

		private void OnDrawGizmos()
		{
			if (showBlastRadius)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, explosionRadius);
			}
		}
	}
}
