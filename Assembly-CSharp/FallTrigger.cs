using UnityEngine;

public class FallTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<ac_CharacterController>().Revert();
		}
		if (other.tag == "WeakPoint")
		{
			other.GetComponent<ExplosionTransfer>().Base.SendMessage("Die");
		}
	}
}
