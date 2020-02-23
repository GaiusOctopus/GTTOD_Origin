using UnityEngine;

public class RicochetRound : MonoBehaviour
{
	public LayerMask PotentialLayers;

	public float Damage;

	public float BulletSpeed;

	public int MaxBounces;

	public GameObject HitEffect;

	public GameObject MissEffect;

	public bool Frisbee;

	[ConditionalField("Frisbee", null)]
	public Frisbee Object;

	private bool HadGravity;

	private Rigidbody ObjectPhysics;

	private int currentBounces;

	private void Start()
	{
		ObjectPhysics = GetComponent<Rigidbody>();
		ObjectPhysics.velocity = base.transform.forward * BulletSpeed;
	}

	private void OnCollisionEnter(Collision collision)
	{
		currentBounces++;
		if (collision.collider.tag == "HitBox")
		{
			collision.collider.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
			UnityEngine.Object.Instantiate(HitEffect, base.transform.position, base.transform.rotation);
			if (Frisbee)
			{
				Object.Return();
			}
		}
		else
		{
			UnityEngine.Object.Instantiate(MissEffect, base.transform.position, base.transform.rotation);
			if (collision.collider.tag == "Object")
			{
				collision.collider.SendMessage("Interact");
			}
		}
		CheckBounce();
	}

	private void CheckBounce()
	{
		if (currentBounces >= MaxBounces)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Freeze()
	{
		ObjectPhysics.velocity = Vector3.zero;
		if (ObjectPhysics.useGravity)
		{
			HadGravity = true;
			ObjectPhysics.useGravity = false;
		}
	}

	public void UnFreeze()
	{
		ObjectPhysics.velocity = base.transform.forward * BulletSpeed;
		if (HadGravity)
		{
			HadGravity = false;
			ObjectPhysics.useGravity = true;
		}
	}
}
