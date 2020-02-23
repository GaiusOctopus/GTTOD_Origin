using UnityEngine;

namespace TacticalAI
{
	public class SpawnerScript : MonoBehaviour
	{
		public Wave[] waves;

		public Transform[] spawnPoints;

		public int currentWave;

		public int enemiesLeft;

		public bool spawnerActive = true;

		private float timeTilNextWave;

		public float timeBeforeFirstWave = 1f;

		public float timeBetweenWaves = 3f;

		public float spawnPointDiameter = 10f;

		private void Start()
		{
			timeTilNextWave = timeBeforeFirstWave;
		}

		private void Update()
		{
			if (spawnerActive && timeTilNextWave < 0f && enemiesLeft <= 0 && currentWave < waves.Length)
			{
				StartWave();
			}
			timeTilNextWave -= Time.deltaTime;
		}

		private void StartWave()
		{
			for (int i = 0; i < waves[currentWave].agents.Length; i++)
			{
				Transform transform = spawnPoints[Random.Range(0, spawnPoints.Length)];
				Vector3 position = transform.position;
				position.x += (Random.value - 0.5f) * spawnPointDiameter;
				position.z += (Random.value - 0.5f) * spawnPointDiameter;
				Object.Instantiate(waves[currentWave].agents[i], position, transform.rotation).SendMessage("SetWaveSpawner", this);
				enemiesLeft++;
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			for (int i = 0; i < spawnPoints.Length; i++)
			{
				Gizmos.DrawWireSphere(spawnPoints[i].position, spawnPointDiameter / 2f);
			}
		}

		private void EndWave()
		{
			currentWave++;
			timeTilNextWave = timeBetweenWaves;
		}

		public void AgentDied()
		{
			enemiesLeft--;
			if (enemiesLeft <= 0)
			{
				EndWave();
			}
		}
	}
}
