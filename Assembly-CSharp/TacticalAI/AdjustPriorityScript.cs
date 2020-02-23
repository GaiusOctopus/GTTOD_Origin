using UnityEngine;

namespace TacticalAI
{
	public class AdjustPriorityScript : MonoBehaviour
	{
		public float cycleTime = 2f;

		private float curCycleTime;

		public int[] teamNumbersToLookFor;

		public float radiusToIncreasePriority = 5f;

		private float radiusToIncreasePrioritySqr;

		public int amountToIncreasePerTargetFound = 2;

		private TargetScript targetScript;

		public bool showRadius;

		public bool alwaysShow;

		private int newPriority = -1;

		private void Awake()
		{
			targetScript = base.gameObject.GetComponent<TargetScript>();
			if (teamNumbersToLookFor.Length == 0)
			{
				teamNumbersToLookFor = new int[1];
				teamNumbersToLookFor[0] = targetScript.myTeamID;
			}
			SetRadiusToIncrease(radiusToIncreasePriority);
		}

		private void Update()
		{
			curCycleTime -= Time.deltaTime;
			if (curCycleTime < 0f)
			{
				UpdatePriority();
				curCycleTime = cycleTime;
			}
		}

		private void UpdatePriority()
		{
			targetScript.targetPriority = -1f;
			Transform[] currentAIsWithIDs = ControllerScript.currentController.GetCurrentAIsWithIDs(teamNumbersToLookFor);
			Vector3 position = base.transform.position;
			newPriority = -1;
			for (int i = 0; i < currentAIsWithIDs.Length; i++)
			{
				if (Vector3.SqrMagnitude(currentAIsWithIDs[i].position - position) < radiusToIncreasePrioritySqr)
				{
					newPriority += amountToIncreasePerTargetFound;
				}
			}
			targetScript.targetPriority = newPriority;
			if (newPriority > 0)
			{
				ControllerScript.currentController.UpdateAllEnemiesEnemyLists();
			}
		}

		private void SetRadiusToIncrease(float x)
		{
			radiusToIncreasePrioritySqr = x * x;
		}

		private void OnDrawGizmos()
		{
			if (showRadius && (newPriority > 0 || alwaysShow))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(base.transform.position, radiusToIncreasePriority);
			}
		}
	}
}
