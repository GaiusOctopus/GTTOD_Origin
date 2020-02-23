using UnityEngine;

public class StartingIsland : MonoBehaviour
{
	private GTTODManager Manager;

	private LevelManager Level;

	private PopUpManager PopUp;

	private ac_CharacterController CharacterController;

	private bool OnIsland;

	private bool HasAsked;

	private bool Loading;

	private float TimeToAsk = 1f;

	private void Start()
	{
		Manager = GameManager.GM.GetComponent<GTTODManager>();
		Level = GameManager.GM.GetComponent<LevelManager>();
		PopUp = GameManager.GM.Player.GetComponent<PopUpManager>();
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Loading = false;
	}

	private void Update()
	{
		if (OnIsland && !Loading && !CharacterController.isFrozen && !CharacterController.inAir)
		{
			if (TimeToAsk <= 0f && !HasAsked && Manager.GameMode != 4)
			{
				HasAsked = true;
				PopUp.AskQuestion("TRAVEL TO THE SURFACE?", base.gameObject);
			}
			else
			{
				TimeToAsk -= Time.deltaTime;
			}
		}
		else
		{
			TimeToAsk = 1f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			OnIsland = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			OnIsland = false;
			HasAsked = false;
		}
	}

	public void Yes()
	{
		Loading = true;
		Level.StartNewScene();
		PlayerPrefsPlus.SetBool("FirstTimeBoot", value: false);
	}

	public void No()
	{
	}

	public void ResetIsland()
	{
		Loading = false;
		HasAsked = false;
		OnIsland = false;
	}
}
