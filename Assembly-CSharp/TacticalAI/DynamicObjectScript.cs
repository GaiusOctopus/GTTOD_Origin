using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class DynamicObjectScript : MonoBehaviour
	{
		public int[] teamsToAlert;

		public float lookForTeamsRadius = 20f;

		public float timeBetweenChecks = 1f;

		public Transform dynamicObjectTransform;

		public string dynamicObjectAnimationClipKey = "DynamicObject";

		public string dynamicObjectMethod = "UseDynamicObject";

		public bool requireEngaging = true;

		public bool currentlyEnabled = true;

		public bool showRadius = true;

		[Range(0f, 1f)]
		public float oddsToLookEachCycle = 0.1f;

		public float timeToWait = 1f;

		private void Start()
		{
			if ((bool)dynamicObjectTransform)
			{
				StartCoroutine(Cycle());
			}
			else
			{
				Debug.LogWarning("No Dynamic Object Transform found!  Please assign it in the inspector!");
			}
		}

		private IEnumerator Cycle()
		{
			yield return new WaitForSeconds(timeBetweenChecks);
			while (base.enabled)
			{
				if (currentlyEnabled && Random.value < oddsToLookEachCycle)
				{
					Target[] currentAIsWithinRadius = ControllerScript.currentController.GetCurrentAIsWithinRadius(teamsToAlert, lookForTeamsRadius, dynamicObjectTransform.position);
					for (int i = 0; i < currentAIsWithinRadius.Length; i++)
					{
						if (currentAIsWithinRadius[i].targetScript.UseDynamicObject(dynamicObjectTransform, dynamicObjectAnimationClipKey, dynamicObjectMethod, requireEngaging, timeToWait))
						{
							DisableDynamicObject();
							break;
						}
					}
				}
				yield return new WaitForSeconds(timeBetweenChecks);
			}
		}

		private void EnableDynamicObject()
		{
			currentlyEnabled = true;
		}

		private void DisableDynamicObject()
		{
			currentlyEnabled = false;
		}

		private void OnDrawGizmos()
		{
			if (showRadius && (bool)dynamicObjectTransform)
			{
				Gizmos.color = Color.grey;
				Gizmos.DrawWireSphere(dynamicObjectTransform.position, lookForTeamsRadius);
			}
		}
	}
}
