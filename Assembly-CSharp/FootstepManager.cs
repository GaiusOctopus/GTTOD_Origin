using UnityEngine;

public class FootstepManager : MonoBehaviour
{
	private ac_CharacterController Player;

	private void Start()
	{
		Player = GameManager.GM.Player.GetComponent<ac_CharacterController>();
	}

	public void PlayFootstepAudio()
	{
		Player.PlayFootStepAudio();
	}
}
