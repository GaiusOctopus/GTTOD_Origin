using UnityEngine;

public class NotesTrigger : MonoBehaviour
{
	public int NoteID;

	private MenuScript Menu;

	private bool CanInteract;

	private bool InMenu;

	private void Start()
	{
		Menu = GameManager.GM.GetComponent<MenuScript>();
	}

	private void Update()
	{
		if ((KeyBindingManager.GetKey(KeyAction.Interact) || Input.GetButtonDown("X")) && CanInteract && !InMenu)
		{
			InMenu = true;
			Menu.OpenNotes(NoteID);
		}
		if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start")) && InMenu)
		{
			InMenu = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			CanInteract = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			CanInteract = false;
		}
	}
}
