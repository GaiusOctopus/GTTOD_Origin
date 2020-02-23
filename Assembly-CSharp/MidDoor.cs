using UnityEngine;

public class MidDoor : MonoBehaviour
{
	public InLevelDoor manager;

	public Transform mySpot;

	public bool entrance;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !entrance)
		{
			manager.EnterDoor(other.transform);
		}
	}
}
