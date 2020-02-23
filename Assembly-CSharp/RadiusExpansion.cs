using UnityEngine;

public class RadiusExpansion : MonoBehaviour
{
	public float Speed;

	private SphereCollider Collider;

	private float Radius;

	private void Start()
	{
		Collider = GetComponent<SphereCollider>();
		Radius = Collider.radius;
		Collider.radius = 0f;
	}

	private void Update()
	{
		if (Collider.radius < Radius)
		{
			Collider.radius += Time.deltaTime * Speed;
		}
	}
}
