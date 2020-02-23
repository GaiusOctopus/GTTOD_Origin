using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
	public GameObject ObjectToSendTo;

	public string MessageToSend;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			ObjectToSendTo.SendMessage(MessageToSend);
		}
	}
}
