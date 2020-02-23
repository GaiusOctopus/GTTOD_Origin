using UnityEngine;

public class PlayerLaunch : MonoBehaviour
{
	public float ForwardSpeed;

	public float UpwardSpeed;

	private Rigidbody PlayerPhysics;

	private ac_CharacterController CharacterController;

	private void Start()
	{
		PlayerPhysics = GameManager.GM.Player.GetComponent<Rigidbody>();
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Boost();
		Object.Destroy(base.gameObject, 3f);
	}

	public void Boost()
	{
		CharacterController.Unground();
		CharacterController.MovementShake();
		CharacterController.MovementShake();
		CharacterController.ResetAbilities();
		CharacterController.Blur();
		PlayerPhysics.velocity = Vector3.zero;
		PlayerPhysics.velocity = PlayerPhysics.transform.forward * ForwardSpeed + PlayerPhysics.transform.up * UpwardSpeed;
	}
}
