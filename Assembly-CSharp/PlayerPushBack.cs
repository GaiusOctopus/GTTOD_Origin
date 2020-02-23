using UnityEngine;

public class PlayerPushBack : MonoBehaviour
{
	private Rigidbody Player;

	private ac_CharacterController CharacterController;

	public float Force = 500f;

	private void Start()
	{
		Player = GameManager.GM.Player.GetComponent<Rigidbody>();
		CharacterController = Player.GetComponent<ac_CharacterController>();
		Player.AddForce(Player.transform.GetChild(0).transform.forward * (0f - Force));
		CharacterController.Unground();
	}
}
