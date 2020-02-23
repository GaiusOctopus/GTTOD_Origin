using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
	private LevelManager LevelManager;

	[HideInInspector]
	public bool StartedLoad;

	private void Start()
	{
		LevelManager = GameManager.GM.GetComponent<LevelManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && base.isActiveAndEnabled && GameManager.GM.GetComponent<GTTODManager>().GameMode == 1 && !StartedLoad)
		{
			LevelManager.StartLoadScene();
			GameManager.GM.Player.GetComponent<InventoryScript>().PrintMessage("Level loading...");
			StartedLoad = true;
		}
	}
}
