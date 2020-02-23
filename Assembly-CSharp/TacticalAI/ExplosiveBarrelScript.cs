using UnityEngine;

namespace TacticalAI
{
	public class ExplosiveBarrelScript : MonoBehaviour
	{
		public float health = 50f;

		public GameObject explosion;

		private bool exploded;

		private void Damage(float damage)
		{
			health -= damage;
			if (health < 0f && !exploded)
			{
				Detonate();
			}
		}

		private void SingleHitBoxDamage(float damage)
		{
			health -= damage;
			if (health < 0f && !exploded)
			{
				Detonate();
			}
		}

		private void Detonate()
		{
			exploded = true;
			if ((bool)explosion)
			{
				Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
