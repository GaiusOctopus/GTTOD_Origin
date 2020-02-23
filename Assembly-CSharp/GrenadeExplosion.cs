using EZCameraShake;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
	public float Radius = 5f;

	public float Power = 10f;

	public float Damage;

	private string damageMethodName = "Damage";

	private void Start()
	{
		Vector3 position = base.transform.position;
		Collider[] array = Physics.OverlapSphere(position, Radius);
		foreach (Collider collider in array)
		{
			if (collider.tag == "HitBox")
			{
				Rigidbody component = collider.GetComponent<Rigidbody>();
				if (component != null)
				{
					component.AddExplosionForce(Power, position, Radius, 3f);
				}
				collider.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
			}
			if (collider.tag == "Player")
			{
				CameraShaker.Instance.ShakeOnce(10f, 2f, 0.35f, 2f);
				collider.SendMessage("Damage", Damage / 5f, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
