using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScenarioManager : MonoBehaviour
{
	public Transform ScenarioParent;

	public List<ScenarioLevel> Scenarios;

	private NavMeshSurface NavMesh;

	private Transform Player;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private AIManager AIManager;

	private GTTODManager MyGTTODManager;

	private GameObject ScenarioObject;

	private GameObject CustomObject;

	private float WaitTime = 1f;

	private int ScenarioIndex;

	private void Start()
	{
		NavMesh = GetComponent<NavMeshSurface>();
		Player = GameManager.GM.Player.transform;
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
		AIManager = GameManager.GM.GetComponent<AIManager>();
		MyGTTODManager = GameManager.GM.GetComponent<GTTODManager>();
		StartScenario();
	}

	public IEnumerator WeaponTimer()
	{
		yield return new WaitForSeconds(0.5f);
		Inventory.DropAllWeapons();
		Inventory.GrabWeapon(Scenarios[ScenarioIndex].Weapon);
		Inventory.AquireEquipment(Scenarios[ScenarioIndex].Equipment);
	}

	public void StartScenario()
	{
		if (ScenarioObject != null)
		{
			Object.Destroy(ScenarioObject);
			Object.Destroy(CustomObject);
		}
		ScenarioIndex = Random.Range(0, Scenarios.Count);
		ScenarioObject = Object.Instantiate(Scenarios[ScenarioIndex].Scenario, ScenarioParent.position, ScenarioParent.rotation).gameObject;
		ScenarioObject.GetComponent<Scenario>().SetScenarioManager(this);
		CustomObject = Object.Instantiate(Scenarios[ScenarioIndex].CustomSetting).gameObject;
		StartCoroutine(WeaponTimer());
		MyGTTODManager.ChangeSongs();
		NavMesh.BuildNavMeshAsync();
	}

	public void StopScenario()
	{
		MyGTTODManager.ChangeAmbience();
		ScenarioObject.GetComponent<Scenario>().Door.SetActive(value: true);
		NavMesh.RemoveData();
	}
}
