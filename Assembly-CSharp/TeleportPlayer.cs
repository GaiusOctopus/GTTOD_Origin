using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
	public GameObject TPFlash;

	private ac_CharacterController Controller;

	private GameManager GM;

	private void Start()
	{
		GM = GameManager.GM;
		Controller = GM.Player.GetComponent<ac_CharacterController>();
		GM.Player.transform.position = base.transform.position;
		Controller.ReParent();
		Object.Instantiate(TPFlash);
	}
}
