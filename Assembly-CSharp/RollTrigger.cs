using UnityEngine;

public class RollTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "WeakPoint")
		{
			other.SendMessage("DodgeRoll");
		}
	}
}
