using System.Collections;
using UnityEngine;

public class Scenario : MonoBehaviour
{
	public GameObject Door;

	private AIManager AIManager;

	private ScenarioSpawner[] Spawners;

	private ScenarioManager MyManager;

	private bool HasSpawned;

	private bool CanCheck;

	private void Start()
	{
		Door.SetActive(value: false);
		AIManager = GameManager.GM.GetComponent<AIManager>();
		Spawners = GetComponentsInChildren<ScenarioSpawner>();
	}

	public void SetScenarioManager(ScenarioManager Manager)
	{
		MyManager = Manager;
	}

	private void Update()
	{
		if (AIManager.OffIsland && !HasSpawned)
		{
			ScenarioSpawner[] spawners = Spawners;
			for (int i = 0; i < spawners.Length; i++)
			{
				spawners[i].Spawn();
			}
			HasSpawned = true;
			StartCoroutine(TimeToCheck());
		}
		if (CanCheck && HasSpawned && AIManager.CurrentWeight <= 0f)
		{
			MyManager.StopScenario();
			CanCheck = false;
		}
	}

	private IEnumerator TimeToCheck()
	{
		yield return new WaitForSeconds(2f);
		CanCheck = true;
	}
}
