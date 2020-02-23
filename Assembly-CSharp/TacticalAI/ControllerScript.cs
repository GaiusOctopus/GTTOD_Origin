using System.Collections.Generic;
using UnityEngine;

namespace TacticalAI
{
	public class ControllerScript : MonoBehaviour
	{
		public static ControllerScript currentController;

		private CoverNodeScript[] coverNodeScripts;

		private List<Target> currentTargets = new List<Target>();

		private int currentID;

		private List<Vector3> currentDynamicCoverSpots = new List<Vector3>();

		public float minDistForDynamicCoverSimilarity = 3f;

		public LayerMask layerMask;

		public static bool pMode;

		public bool usePerformanceMode;

		private List<TacticalNavLink> parkourLinks = new List<TacticalNavLink>();

		public float maximumtDistForParkour = 3f;

		private void Awake()
		{
			currentController = this;
			base.transform.tag = "AI Controller";
			GameObject[] array = GameObject.FindGameObjectsWithTag("Cover");
			pMode = usePerformanceMode;
			minDistForDynamicCoverSimilarity *= minDistForDynamicCoverSimilarity;
			List<CoverNodeScript> list = new List<CoverNodeScript>();
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i].GetComponent<CoverNodeScript>());
			}
			coverNodeScripts = list.ToArray();
		}

		public void UpdateAllEnemiesEnemyLists()
		{
			for (int i = 0; i < currentTargets.Count; i++)
			{
				currentTargets[i].targetScript.UpdateEnemyAndAllyLists(GetCurrentTargetsWithIDs(currentTargets[i].targetScript.alliedTeamsIDs), GetCurrentTargetsWithIDs(currentTargets[i].targetScript.enemyTeamsIDs));
			}
		}

		public int AddTarget(int id, Transform transformToAdd, TargetScript script)
		{
			currentID++;
			Target item = new Target(currentID, id, transformToAdd, script);
			currentTargets.Add(item);
			UpdateAllEnemiesEnemyLists();
			return currentID;
		}

		public void RemoveTargetFromTargetList(int id)
		{
			if (currentTargets.Count <= 0)
			{
				return;
			}
			int num = 0;
			while (true)
			{
				if (num < currentTargets.Count)
				{
					if (currentTargets[num].targetScript.GetUniqueID() == id)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			currentTargets.RemoveAt(num);
			UpdateAllEnemiesEnemyLists();
		}

		public void CreateSound(Vector3 pos, float radius, int[] teams)
		{
			radius *= radius;
			int num = 0;
			for (int i = 0; i < currentTargets.Count; i++)
			{
				for (num = 0; num < teams.Length; num++)
				{
					if ((bool)currentTargets[i].transform && currentTargets[i].teamID == teams[num])
					{
						if (Vector3.SqrMagnitude(currentTargets[i].transform.position - pos) < radius)
						{
							currentTargets[i].targetScript.HearSound(pos);
						}
						num = teams.Length;
					}
				}
			}
		}

		public void CreateSound(Vector3 pos, float radius)
		{
			radius *= radius;
			for (int i = 0; i < currentTargets.Count; i++)
			{
				if (Vector3.SqrMagnitude(currentTargets[i].transform.position - pos) < radius)
				{
					currentTargets[i].targetScript.HearSound(pos);
				}
			}
		}

		public bool isDynamicCoverSpotCurrentlyUsed(Vector3 v)
		{
			for (int i = 0; i < currentDynamicCoverSpots.Count; i++)
			{
				if (Vector3.SqrMagnitude(v - currentDynamicCoverSpots[i]) < minDistForDynamicCoverSimilarity)
				{
					return true;
				}
			}
			return false;
		}

		public void AddACoverSpot(Vector3 v)
		{
			currentDynamicCoverSpots.Add(v);
		}

		public void RemoveACoverSpot(Vector3 v)
		{
			currentDynamicCoverSpots.Remove(v);
		}

		public CoverNodeScript[] GetCovers()
		{
			return coverNodeScripts;
		}

		public Transform[] GetCurrentAIsWithIDs(int[] ids)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < currentTargets.Count; i++)
			{
				for (int j = 0; j < ids.Length; j++)
				{
					if (ids[j] == currentTargets[i].teamID && currentTargets[i].targetScript.targetPriority >= 0f)
					{
						list.Add(currentTargets[i].transform);
						break;
					}
				}
			}
			return list.ToArray();
		}

		public Target[] GetCurrentTargetsWithIDs(int[] ids)
		{
			List<Target> list = new List<Target>();
			for (int i = 0; i < currentTargets.Count; i++)
			{
				for (int j = 0; j < ids.Length; j++)
				{
					if (ids[j] == currentTargets[i].teamID && currentTargets[i].targetScript.targetPriority >= 0f)
					{
						list.Add(currentTargets[i]);
						break;
					}
				}
			}
			return list.ToArray();
		}

		public Target[] GetCurrentAIsWithinRadius(int[] ids, float rad, Vector3 origin)
		{
			List<Target> list = new List<Target>();
			rad *= rad;
			for (int i = 0; i < currentTargets.Count; i++)
			{
				for (int j = 0; j < ids.Length; j++)
				{
					if (ids[j] == currentTargets[i].teamID && currentTargets[i].targetScript.targetPriority >= 0f && Vector3.SqrMagnitude(currentTargets[i].transform.position - origin) < rad)
					{
						list.Add(currentTargets[i]);
						break;
					}
				}
			}
			return list.ToArray();
		}

		public bool TargetOnTeamsInRadius(int[] ids, float rad, Vector3 origin)
		{
			rad *= rad;
			for (int i = 0; i < currentTargets.Count; i++)
			{
				for (int j = 0; j < ids.Length; j++)
				{
					if (ids[j] == currentTargets[i].teamID && currentTargets[i].targetScript.targetPriority >= 0f && Vector3.SqrMagnitude(currentTargets[i].transform.position - origin) < rad)
					{
						return true;
					}
				}
			}
			return false;
		}

		public Target[] GetCurrentTargets()
		{
			return currentTargets.ToArray();
		}

		public LayerMask GetLayerMask()
		{
			return layerMask;
		}

		public void AddParkourLink(TacticalNavLink p)
		{
			parkourLinks.Add(p);
		}

		public bool GetClosestParkourLink(Vector3 p, ref TacticalNavLink linkObj)
		{
			if (parkourLinks.Count == 0)
			{
				return false;
			}
			float num = 1E+12f;
			float num2 = 0f;
			linkObj = parkourLinks[0];
			for (int i = 0; i < parkourLinks.Count; i++)
			{
				num2 = Vector3.Magnitude(p - parkourLinks[i].position);
				if (num2 < num)
				{
					linkObj = parkourLinks[i];
					num = num2;
				}
			}
			return true;
		}

		public void ChangeAgentsTeam(int uI, int newTeam)
		{
			ChangeAgentsTeam(uI, newTeam, shouldUpdateTargetListsNow: true);
		}

		public void ChangeAgentsTeam(int uI, int newTeam, bool shouldUpdateTargetListsNow)
		{
			for (int i = 0; i < currentTargets.Count; i++)
			{
				if (currentTargets[i].uniqueIdentifier == uI)
				{
					currentTargets[i].teamID = newTeam;
				}
			}
			if (shouldUpdateTargetListsNow)
			{
				UpdateAllEnemiesEnemyLists();
			}
		}
	}
}
