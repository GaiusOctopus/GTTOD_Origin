using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIManager : MonoBehaviour
{
	[Header("SPAWN VARIABLES")]
	public bool LeveledSpawn = true;

	public int CurrentLevel;

	public float DesiredWeight;

	public float CurrentWeight;

	[Header("SPAWNER OBJECTS")]
	public GameObject CurrentSpawnObject;

	public List<GameObject> Spawners;

	[Header("PRIVATE VARIABLES")]
	private GTTODManager Manager;

	private Transform Player;

	private Respawner[] EnemiesToKill;

	private bool IsSpawning;

	private float TimeToSpawn = 0.5f;

	private float LevelWaitTime = 10f;

	public GameObject[] SpawnPoints;

	public List<Respawner> CurrentEnemies;

	[HideInInspector]
	public List<AITalker> Talkers;

	[HideInInspector]
	public List<GameObject> NearSpawnPoints;

	[HideInInspector]
	public bool OffIsland;

	private bool HasRaisedDamage;

	private void Start()
	{
		Manager = GameManager.GM.GetComponent<GTTODManager>();
		Player = GameManager.GM.Player.transform;
	}

	private void Update()
	{
		UpdateIslandCheck();
		if (OffIsland && IsSpawning)
		{
			UpdateSpawning();
		}
	}

	public IEnumerator LeveledSpawner()
	{
		if (!IsSpawning)
		{
			yield break;
		}
		yield return new WaitForSeconds(LevelWaitTime);
		if (!IsSpawning || CurrentLevel == Spawners.Count)
		{
			yield break;
		}
		CurrentLevel++;
		CurrentSpawnObject = Spawners[CurrentLevel];
		LevelWaitTime *= 1.75f;
		if (CurrentLevel < 7)
		{
			if (CurrentLevel == 6)
			{
				Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString() + " --- ENEMY DAMAGE DOUBLED");
				GameManager.GM.Player.GetComponent<HealthScript>().DamageMultiplier *= 2f;
				HasRaisedDamage = true;
			}
			else
			{
				Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString());
			}
			StartCoroutine(LeveledSpawner());
		}
		else
		{
			Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString() + " --- ENEMY HEALTH DOUBLED");
		}
	}

	public void UpdateIslandCheck()
	{
		if (Vector3.Distance(base.transform.position, Player.position) > 20f)
		{
			OffIsland = true;
		}
		else
		{
			OffIsland = false;
		}
	}

	public void UpdateSpawning()
	{
		if (CurrentWeight < DesiredWeight)
		{
			TimeToSpawn -= Time.deltaTime;
			if (TimeToSpawn <= 0f)
			{
				Spawn();
				TimeToSpawn = UnityEngine.Random.Range(1.5f, 3.5f);
			}
		}
		else
		{
			TimeToSpawn = 1f;
		}
	}

	public void StartSpawning()
	{
		if (!GameManager.GM.Speedrunner)
		{
			IsSpawning = true;
			FindPoints();
			if (LeveledSpawn)
			{
				CurrentLevel = 0;
				CurrentSpawnObject = Spawners[CurrentLevel];
				LevelWaitTime = 20f;
				Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString());
				StartCoroutine(LeveledSpawner());
			}
		}
	}

	public void StartSpawningAtLevel(int LevelToStart)
	{
		if (!GameManager.GM.Speedrunner)
		{
			IsSpawning = true;
			FindPoints();
			if (LeveledSpawn)
			{
				CurrentLevel = LevelToStart;
				CurrentSpawnObject = Spawners[CurrentLevel];
				LevelWaitTime = 60f;
				Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString());
				StartCoroutine(LeveledSpawner());
			}
		}
	}

	public void StopSpawning()
	{
		IsSpawning = false;
		CurrentLevel = 0;
		CurrentSpawnObject = Spawners[CurrentLevel];
		LevelWaitTime = 20f;
		if (HasRaisedDamage)
		{
			GameManager.GM.Player.GetComponent<HealthScript>().DamageMultiplier /= 2f;
			HasRaisedDamage = false;
		}
	}

	public void RestartSpawning()
	{
		CurrentSpawnObject = Spawners[CurrentLevel];
		LevelWaitTime = 20f;
		if (HasRaisedDamage)
		{
			GameManager.GM.Player.GetComponent<HealthScript>().DamageMultiplier /= 2f;
			CurrentLevel = 3;
			HasRaisedDamage = false;
		}
		else
		{
			CurrentLevel = 0;
		}
	}

	public void Spawn()
	{
		NearSpawnPoints.Clear();
		GameObject[] spawnPoints = SpawnPoints;
		foreach (GameObject gameObject in spawnPoints)
		{
			if (Vector3.Distance(Player.position, gameObject.transform.position) <= 50f && Vector3.Distance(Player.position, gameObject.transform.position) >= 5f)
			{
				NearSpawnPoints.Add(gameObject);
			}
		}
		if (NearSpawnPoints.Count >= 1)
		{
			int index = UnityEngine.Random.Range(0, NearSpawnPoints.Count);
			UnityEngine.Object.Instantiate(CurrentSpawnObject, NearSpawnPoints[index].transform.position, base.transform.rotation);
			TimeToSpawn = UnityEngine.Random.Range(1.5f, 3.5f);
		}
		else
		{
			TimeToSpawn = UnityEngine.Random.Range(1.5f, 3.5f);
		}
	}

	public void IncreaseDifficulty()
	{
		if (CurrentLevel < 5)
		{
			CurrentLevel = 5;
			CurrentSpawnObject = Spawners[CurrentLevel];
			Player.GetComponent<InventoryScript>().PrintFancyMessage("THREAT LEVEL " + CurrentLevel.ToString());
		}
	}

	public void FindPoints()
	{
		Array.Clear(SpawnPoints, 0, SpawnPoints.Length);
		SpawnPoints = GameObject.FindGameObjectsWithTag("EndlessSpawnPoint");
	}

	public void AddEnemy(int EnemyWeight, Respawner NewEnemy)
	{
		CurrentWeight += EnemyWeight;
		CurrentEnemies.Add(NewEnemy);
		NewEnemy.StoreMyIndex(CurrentEnemies.IndexOf(NewEnemy));
	}

	public void RemoveEnemy(int EnemyWeight, int EnemyIndex)
	{
		CurrentWeight -= EnemyWeight;
		CurrentEnemies.RemoveAt(EnemyIndex);
		CurrentEnemies.TrimExcess();
		ShiftIndex();
	}

	public void ShiftIndex()
	{
		foreach (Respawner currentEnemy in CurrentEnemies)
		{
			currentEnemy.MyIndex = CurrentEnemies.IndexOf(currentEnemy);
		}
	}

	public void ClearEnemies()
	{
		CurrentEnemies.Clear();
		EnemiesToKill = UnityEngine.Object.FindObjectsOfType<Respawner>();
		Respawner[] enemiesToKill = EnemiesToKill;
		for (int i = 0; i < enemiesToKill.Length; i++)
		{
			UnityEngine.Object.Destroy(enemiesToKill[i].gameObject);
		}
	}

	public void TalkerReact()
	{
		if (Talkers.Count > 0 && UnityEngine.Random.Range(-1f, 1f) >= 0.75f)
		{
			int index = UnityEngine.Random.Range(0, Talkers.Count);
			Talkers[index].SayRandomDeath();
		}
	}

	public void ToggleAbyssSpawning()
	{
		if (!Manager.Abyss.activeInHierarchy)
		{
			return;
		}
		if (SceneManager.GetActiveScene().name == "2. GTTOD")
		{
			if (!IsSpawning)
			{
				StartSpawning();
			}
			else
			{
				StopSpawning();
			}
		}
		else
		{
			Player.GetComponent<InventoryScript>().PrintMessage("Can't spawn in this scene");
		}
	}
}
