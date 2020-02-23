using UnityEngine;

public class Gamepad : MonoBehaviour
{
	public GameManager GM;

	private void Update()
	{
		Input.GetButtonDown("A");
	}
}
