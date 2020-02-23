using UnityEngine;

public class KnockDownTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "WeakPoint")
		{
			other.SendMessage("RagdollAgent", SendMessageOptions.DontRequireReceiver);
		}
	}
}
