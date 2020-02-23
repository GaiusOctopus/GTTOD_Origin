using System.Collections;
using UnityEngine;

public class DistanceEnabler : MonoBehaviour
{
	[Tooltip("When the player has reached this point on the map, I will enable my objects")]
	public float EnableDistance;

	[Tooltip("When the player has reached this point on the map, I will disable my objects")]
	public float DisableDistance;

	public GameObject MyObject;

	private Transform Player;

	private bool HasEnabled;

	private bool HasStartedGame;

	private void Start()
	{
		StartCoroutine(StartGame());
	}

	public IEnumerator StartGame()
	{
		HasStartedGame = false;
		yield return new WaitForSeconds(4f);
		HasStartedGame = true;
	}

	private void Update()
	{
		if (!HasStartedGame)
		{
			return;
		}
		if (Player != null)
		{
			if (Player.transform.position.z > EnableDistance && Player.transform.position.z < DisableDistance && !HasEnabled)
			{
				MyObject.SetActive(value: true);
				HasEnabled = true;
			}
			if ((Player.transform.position.z > DisableDistance || Player.transform.position.z < EnableDistance) && HasEnabled)
			{
				MyObject.SetActive(value: false);
				HasEnabled = false;
			}
		}
		else
		{
			Player = GameManager.GM.Player.transform;
		}
	}
}
