using System.Collections;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
	public float UpForce;

	public float ForwardForce;

	private Rigidbody Player;

	private bool HasLaunched;

	private void Start()
	{
		Player = GameManager.GM.Player.GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, Player.transform.position) <= 2f && !HasLaunched)
		{
			HasLaunched = true;
			Launch();
			StartCoroutine(LaunchCooldown());
		}
	}

	public void Launch()
	{
		Player.transform.position = base.transform.position;
		Player.gameObject.GetComponent<ac_CharacterController>().StopDashing();
		Player.gameObject.GetComponent<ac_CharacterController>().Unground();
		Player.gameObject.GetComponent<ac_CharacterController>().PlayerUnCrouch();
		Player.velocity = Vector3.zero;
		Player.velocity = base.transform.up * UpForce + base.transform.forward * ForwardForce;
		Player.gameObject.GetComponent<ac_CharacterController>().MovementShake();
		base.gameObject.GetComponent<AudioSource>().Play();
	}

	private IEnumerator LaunchCooldown()
	{
		yield return new WaitForSeconds(1f);
		HasLaunched = false;
	}
}
