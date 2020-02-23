using System.Collections;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
	private ac_CharacterController CharacterController;

	private BoxCollider Collider;

	private void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Collider = GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			CharacterController.SetSwimming(SwimmingState: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			CharacterController.SetSwimming(SwimmingState: false);
			StartCoroutine(TriggerCool());
		}
	}

	public IEnumerator TriggerCool()
	{
		Collider.enabled = false;
		yield return new WaitForSeconds(0.5f);
		Collider.enabled = true;
	}
}
