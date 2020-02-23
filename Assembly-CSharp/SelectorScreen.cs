using System.Collections;
using UnityEngine;

public class SelectorScreen : MonoBehaviour
{
	public Transform CutscenePosition;

	public GameObject CutsceneManager;

	public AudioClip InSFX;

	public AudioClip OutSFX;

	private GTTODManager Manager;

	private LevelManager LevelManager;

	private Animation Anim;

	private ac_CharacterController CharacterController;

	private AudioSource Audio;

	private bool CanInteract;

	private bool InMenu;

	private void Start()
	{
		Manager = GameManager.GM.GetComponent<GTTODManager>();
		LevelManager = GameManager.GM.GetComponent<LevelManager>();
		Anim = CutsceneManager.GetComponent<Animation>();
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if ((!KeyBindingManager.GetKeyDown(KeyAction.Interact) && !Input.GetButtonDown("X")) || !CanInteract || InMenu)
		{
			return;
		}
		if (Manager.GameMode == 1)
		{
			if (!LevelManager.Trigger.StartedLoad)
			{
				InMenu = true;
				CutsceneManager.transform.position = CutscenePosition.position;
				CutsceneManager.transform.rotation = CutscenePosition.rotation;
				CharacterController.CutsceneStart();
				CutsceneManager.SetActive(value: true);
				Anim.Play("ComputerOn");
				Audio.PlayOneShot(InSFX);
				StartCoroutine(ScreenOn());
			}
			else
			{
				GameManager.GM.Player.GetComponent<InventoryScript>().PrintMessage("LEVEL HAS ALREADY STARTED LOADING");
			}
		}
		else
		{
			GameManager.GM.Player.GetComponent<InventoryScript>().PrintMessage("START OVERHAUL TO USE THIS TERMINAL");
		}
	}

	public IEnumerator ScreenOn()
	{
		yield return new WaitForSeconds(1f);
		GameManager.GM.GetComponent<MenuScript>().SelectLevel();
		CutsceneManager.SetActive(value: false);
	}

	public IEnumerator ScreenOff()
	{
		yield return new WaitForSeconds(1f);
		CutsceneManager.SetActive(value: false);
		CutsceneManager.transform.position = new Vector3(0f, 0.35f, 0f);
		CutsceneManager.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		CharacterController.CutsceneEnd(CutscenePosition);
		CharacterController.SetXRotation(105f);
		InMenu = false;
	}

	public void ExitLevelSelect()
	{
		CutsceneManager.transform.position = CutscenePosition.position;
		CutsceneManager.transform.rotation = CutscenePosition.rotation;
		CharacterController.CutsceneStart();
		GameManager.GM.GetComponent<MenuScript>().ForceCloseMenu();
		CutsceneManager.SetActive(value: true);
		Anim.Play("ComputerOff");
		Audio.PlayOneShot(OutSFX);
		StartCoroutine(ScreenOff());
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
