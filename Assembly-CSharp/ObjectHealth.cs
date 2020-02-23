using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
	public float Health;

	public GameObject DeathObject;

	public void Damage(float damage)
	{
		Health -= damage;
		if (Health <= 0f)
		{
			Die();
		}
	}

	public void Die()
	{
		Object.Instantiate(DeathObject, base.transform.position, base.transform.rotation);
		Object.Destroy(base.gameObject);
	}
}
