using UnityEngine;

public class StartPlayer : MonoBehaviour
{
	public GameObject Player;

	public MenuScript Menu;

	public GameObject FlashCanvas;

	private void OnDisable()
	{
		Player.SetActive(value: true);
		Player.GetComponent<ac_CharacterController>().UnFreezePlayer();
		Object.Instantiate(FlashCanvas);
		Time.timeScale = 1f;
	}
}
