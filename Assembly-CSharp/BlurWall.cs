using System.Collections.Generic;
using UnityEngine;

public class BlurWall : MonoBehaviour
{
	[Header("EFFECTS")]
	public Material BlurWallMaterial;

	public GameObject BlurEffect;

	[Header("OBJECT EDITS")]
	public List<GameObject> EnableObjects;

	public List<GameObject> DisableObjects;

	private ac_CharacterController CharacterController;

	private MeshRenderer Renderer;

	private bool hasTriggered;

	public void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Renderer = GetComponent<MeshRenderer>();
		Renderer.material = BlurWallMaterial;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !hasTriggered)
		{
			BlurWallInteraction();
			hasTriggered = true;
		}
	}

	public void BlurWallInteraction()
	{
		Object.Instantiate(BlurEffect);
		CharacterController.HeavyBlur();
		Renderer.enabled = false;
		GameManager.GM.GetComponent<AIManager>().StartSpawning();
		GameManager.GM.GetComponent<GTTODManager>().ChangeSongs();
		if (EnableObjects.Count > 0)
		{
			foreach (GameObject enableObject in EnableObjects)
			{
				enableObject.SetActive(value: true);
			}
		}
		if (DisableObjects.Count > 0)
		{
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: false);
			}
		}
	}

	public void ResetFuntionality()
	{
		Renderer.enabled = true;
		hasTriggered = false;
		GameManager.GM.GetComponent<AIManager>().StopSpawning();
		GameManager.GM.GetComponent<GTTODManager>().ChangeAmbience();
		if (EnableObjects.Count > 0)
		{
			foreach (GameObject enableObject in EnableObjects)
			{
				enableObject.SetActive(value: false);
			}
		}
		if (DisableObjects.Count > 0)
		{
			foreach (GameObject disableObject in DisableObjects)
			{
				disableObject.SetActive(value: true);
			}
		}
	}
}
