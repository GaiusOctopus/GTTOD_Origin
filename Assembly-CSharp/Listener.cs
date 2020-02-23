using UnityEngine;

public class Listener : MonoBehaviour
{
	public GameObject box1;

	public GameObject box2;

	public GameObject box3;

	public GameObject box4;

	private void Update()
	{
		if (KeyBindingManager.GetKeyDown(KeyAction.Forward))
		{
			box1.SetActive(!box1.activeSelf);
		}
	}
}
