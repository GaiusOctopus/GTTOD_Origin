using UnityEngine;

public class Teleporter : MonoBehaviour
{
	public Transform TeleportPoint;

	private ac_CharacterController Player;

	private void Start()
	{
		Player = GameManager.GM.Player.GetComponent<ac_CharacterController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Player.transform.position = TeleportPoint.position;
			Player.SetXRotation(0f);
			Player.SetYRotation(0f);
		}
	}
}
