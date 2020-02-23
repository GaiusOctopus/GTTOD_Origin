using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderButtonInteraction : MonoBehaviour
{
	private void Reset()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	private void Awake()
	{
		GetComponent<Collider>().isTrigger = true;
	}
}
