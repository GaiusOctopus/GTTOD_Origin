using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
	public class ExplosionPhysicsForce : MonoBehaviour
	{
		public float explosionForce = 4f;

		private IEnumerator Start()
		{
			yield return null;
			Collider[] array = Physics.OverlapSphere(base.transform.position, 1f);
			List<Rigidbody> list = new List<Rigidbody>();
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				if (collider.attachedRigidbody != null && !list.Contains(collider.attachedRigidbody))
				{
					list.Add(collider.attachedRigidbody);
				}
			}
			foreach (Rigidbody item in list)
			{
				item.AddExplosionForce(explosionForce * 10f, base.transform.position, 10f, 0.25f, ForceMode.Impulse);
			}
		}
	}
}
