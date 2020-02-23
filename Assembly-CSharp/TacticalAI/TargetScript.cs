using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TacticalAI
{
	public class TargetScript : MonoBehaviour
	{
		public Transform targetObjectTransform;

		public Transform myLOSTarget;

		public float targetPriority = 1f;

		public BaseScript myAIBaseScript;

		public GameObject healthScriptHolder;

		private int myUniqueID;

		public int myTeamID;

		public int[] alliedTeamsIDs;

		public int[] enemyTeamsIDs;

		private List<Target> listOfCurrentlyNoticedTargets = new List<Target>();

		public int timeBeforeTargetExpiration = 15;

		private List<int> targetIDs = new List<int>();

		private Target[] enemyTargets;

		public float timeBetweenTargetChecksIfEngaging = 7f;

		public float timeBetweenTargetChecksIfNotEngaging = 12f;

		public float timeBetweenLOSChecks = 0.5f;

		private bool engaging;

		public Target currentEnemyTarget;

		public float shoutDist = 50f;

		public float timeBetweenReactingToSounds = 15f;

		private bool shouldReactToNewSound = true;

		public float maxLineOfSightChecksPerFrame = 3f;

		public Transform eyeTransform;

		private float effectiveFOV;

		public float myFieldOfView = 130f;

		public bool debugFieldOfView;

		[HideInInspector]
		public LayerMask layerMask;

		public bool canAcceptDynamicObjectRequests;

		public float maxDistToNoticeTarget = 9999f;

		private bool isPlaying = true;

		private List<Vector3> lastKnownTargetPositions = new List<Vector3>();

		public float distToLoseAwareness = 35f;

		private void Awake()
		{
			if (!healthScriptHolder)
			{
				healthScriptHolder = base.gameObject;
			}
			layerMask = ControllerScript.currentController.GetLayerMask();
			if (!targetObjectTransform)
			{
				targetObjectTransform = base.transform;
			}
			if (!myLOSTarget)
			{
				myLOSTarget = targetObjectTransform;
			}
			if ((bool)ControllerScript.currentController)
			{
				myUniqueID = ControllerScript.currentController.AddTarget(myTeamID, targetObjectTransform, this);
			}
			else
			{
				Debug.LogWarning("No AI Controller Found!");
			}
			if (!eyeTransform)
			{
				eyeTransform = targetObjectTransform;
			}
			effectiveFOV = myFieldOfView / 2f;
			maxDistToNoticeTarget *= maxDistToNoticeTarget;
			if ((bool)myAIBaseScript)
			{
				myAIBaseScript.SetTargetObj(this);
			}
		}

		public void SetNewTeam(int newTeam)
		{
			ControllerScript.currentController.ChangeAgentsTeam(myUniqueID, newTeam);
			myTeamID = newTeam;
		}

		private void Start()
		{
			if ((bool)myAIBaseScript)
			{
				StartCoroutine(LoSLoop());
				StartCoroutine(TargetSelectionLoop());
			}
		}

		private IEnumerator LoSLoop()
		{
			yield return new WaitForSeconds(Random.value);
			while (myAIBaseScript.isCurrentlyActive())
			{
				CheckForLOSAwareness(shouldCheck360Degrees: false);
				yield return new WaitForSeconds(timeBetweenLOSChecks);
			}
		}

		private IEnumerator TargetSelectionLoop()
		{
			yield return new WaitForSeconds(Random.value);
			while (myAIBaseScript.isCurrentlyActive())
			{
				if (engaging)
				{
					yield return new WaitForSeconds(timeBetweenTargetChecksIfEngaging);
				}
				else
				{
					yield return new WaitForSeconds(timeBetweenTargetChecksIfNotEngaging);
				}
				ChooseTarget();
			}
		}

		private void OnDestroy()
		{
			RemoveThisTargetFromPLay();
		}

		public void RemoveThisTargetFromPLay()
		{
			if (ControllerScript.currentController != null && isPlaying)
			{
				ControllerScript.currentController.RemoveTargetFromTargetList(myUniqueID);
			}
		}

		private void OnApplicationQuit()
		{
			isPlaying = false;
		}

		public void UpdateEnemyAndAllyLists(Target[] a, Target[] e)
		{
			if (!myAIBaseScript)
			{
				return;
			}
			enemyTargets = e;
			if (enemyTargets.Length == 0)
			{
				myAIBaseScript.EndEngage();
				engaging = false;
			}
			Target[] array = listOfCurrentlyNoticedTargets.ToArray();
			listOfCurrentlyNoticedTargets = new List<Target>();
			Vector3[] array2 = lastKnownTargetPositions.ToArray();
			lastKnownTargetPositions = new List<Vector3>();
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < enemyTargets.Length; j++)
				{
					if (array[i].uniqueIdentifier == enemyTargets[j].uniqueIdentifier)
					{
						listOfCurrentlyNoticedTargets.Add(enemyTargets[j]);
						lastKnownTargetPositions.Add(array2[i]);
						break;
					}
				}
			}
			if (engaging)
			{
				CheckForLOSAwareness(shouldCheck360Degrees: true);
			}
			else
			{
				CheckForLOSAwareness(shouldCheck360Degrees: false);
			}
			ChooseTarget();
		}

		private void NoticeATarget(Target newTarget)
		{
			int uniqueIdentifier = newTarget.uniqueIdentifier;
			for (int i = 0; i < targetIDs.Count; i++)
			{
				if (targetIDs[i] == uniqueIdentifier)
				{
					return;
				}
			}
			lastKnownTargetPositions.Add(newTarget.transform.position);
			listOfCurrentlyNoticedTargets.Add(newTarget);
			targetIDs.Add(uniqueIdentifier);
			ChooseTarget();
			if (!engaging)
			{
				myAIBaseScript.StartEngage();
				engaging = true;
			}
		}

		private void CheckIfWeStillHaveAwareness()
		{
			int num = 0;
			for (num = 0; num < listOfCurrentlyNoticedTargets.Count; num++)
			{
				Transform transform = listOfCurrentlyNoticedTargets[num].transform;
				if ((bool)eyeTransform && (bool)transform && !Physics.Linecast(eyeTransform.position, transform.position, layerMask))
				{
					lastKnownTargetPositions[num] = transform.position;
				}
				else if ((bool)transform && Vector3.Distance(transform.position, lastKnownTargetPositions[num]) > distToLoseAwareness)
				{
					listOfCurrentlyNoticedTargets.RemoveAt(num);
					lastKnownTargetPositions.RemoveAt(num);
					num--;
				}
			}
			if (listOfCurrentlyNoticedTargets.Count == 0)
			{
				myAIBaseScript.EndEngage();
				engaging = false;
				listOfCurrentlyNoticedTargets = new List<Target>();
				lastKnownTargetPositions = new List<Vector3>();
				targetIDs = new List<int>();
			}
		}

		private void ChooseTarget()
		{
			if (!eyeTransform)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			Transform transform = eyeTransform;
			currentEnemyTarget = null;
			bool flag = false;
			int num3 = 0;
			CheckIfWeStillHaveAwareness();
			for (num3 = 0; num3 < listOfCurrentlyNoticedTargets.Count; num3++)
			{
				if (!listOfCurrentlyNoticedTargets[num3].transform)
				{
					continue;
				}
				transform = listOfCurrentlyNoticedTargets[num3].transform;
				if (!Physics.Linecast(eyeTransform.position, transform.position, layerMask))
				{
					num2 = Vector3.SqrMagnitude(transform.position - targetObjectTransform.position);
					num2 /= listOfCurrentlyNoticedTargets[num3].targetScript.GetComponent<TargetScript>().targetPriority;
					if (num2 < num || num == 0f || !flag)
					{
						currentEnemyTarget = listOfCurrentlyNoticedTargets[num3];
						num = num2;
						flag = true;
					}
				}
				else if (!flag)
				{
					num2 = Vector3.SqrMagnitude(transform.position - targetObjectTransform.position);
					if (num2 < num || num < 0f || !flag)
					{
						currentEnemyTarget = listOfCurrentlyNoticedTargets[num3];
						num = num2;
					}
				}
			}
			if (currentEnemyTarget != null)
			{
				AlertAlliesOfEnemy_Shout();
			}
			if (currentEnemyTarget == null && enemyTargets.Length != 0)
			{
				currentEnemyTarget = enemyTargets[Random.Range(0, enemyTargets.Length - 1)];
			}
			if (currentEnemyTarget != null)
			{
				myAIBaseScript.SetMyTarget(currentEnemyTarget.transform, currentEnemyTarget.targetScript.myLOSTarget);
			}
			if (currentEnemyTarget == null)
			{
				myAIBaseScript.RemoveMyTarget();
			}
		}

		public void AlertAlliesOfEnemy_Shout()
		{
			if (currentEnemyTarget != null && (bool)currentEnemyTarget.transform)
			{
				ControllerScript.currentController.CreateSound(currentEnemyTarget.transform.position, shoutDist, alliedTeamsIDs);
			}
		}

		public void HearSound(Vector3 soundPos)
		{
			if ((bool)myAIBaseScript && shouldReactToNewSound && !myAIBaseScript.IsEnaging())
			{
				CheckForLOSAwareness(shouldCheck360Degrees: true);
				myAIBaseScript.StartCoroutine("HearSound", soundPos);
				myAIBaseScript.SetAlertSpeed();
				StartCoroutine(SetTimeUntilNextSound());
			}
		}

		private IEnumerator SetTimeUntilNextSound()
		{
			shouldReactToNewSound = false;
			yield return new WaitForSeconds(timeBetweenReactingToSounds);
			shouldReactToNewSound = true;
		}

		public void CheckForLOSAwareness(bool shouldCheck360Degrees)
		{
			if (enemyTargets == null)
			{
				return;
			}
			for (int i = 0; i < enemyTargets.Length; i++)
			{
				if (debugFieldOfView)
				{
					Debug.DrawRay(eyeTransform.position, eyeTransform.forward * 20f, Color.green, timeBetweenLOSChecks);
					Vector3 a = Quaternion.AngleAxis(effectiveFOV, Vector3.up) * eyeTransform.forward;
					Debug.DrawRay(eyeTransform.position, a * 20f, Color.green, timeBetweenLOSChecks);
					a = Quaternion.AngleAxis(0f - effectiveFOV, Vector3.up) * eyeTransform.forward;
					Debug.DrawRay(eyeTransform.position, a * 20f, Color.green, timeBetweenLOSChecks);
				}
				if ((bool)eyeTransform && (bool)enemyTargets[i].transform && (shouldCheck360Degrees || Vector3.Angle(eyeTransform.forward, enemyTargets[i].transform.position - eyeTransform.position) < effectiveFOV) && Vector3.SqrMagnitude(eyeTransform.position - enemyTargets[i].transform.position) < maxDistToNoticeTarget && !Physics.Linecast(eyeTransform.position, enemyTargets[i].transform.position, layerMask))
				{
					NoticeATarget(enemyTargets[i]);
				}
			}
		}

		public void WarnOfGrenade(Transform t, float d)
		{
			if ((bool)myAIBaseScript && myAIBaseScript.inCover)
			{
				myAIBaseScript.WarnOfGrenade(t, d);
			}
		}

		public bool UseDynamicObject(Transform newMovementObjectTransform, string newAnimationToUse, string methodToCall, bool requireEngaging)
		{
			return UseDynamicObject(newMovementObjectTransform, newAnimationToUse, methodToCall, requireEngaging, 1f);
		}

		public bool UseDynamicObject(Transform newMovementObjectTransform, string newAnimationToUse, string methodToCall, bool requireEngaging, float timeToWait)
		{
			if (canAcceptDynamicObjectRequests && myAIBaseScript.SetDynamicObject(newMovementObjectTransform, newAnimationToUse, methodToCall, requireEngaging, timeToWait))
			{
				return true;
			}
			return false;
		}

		public int GetUniqueID()
		{
			return myUniqueID;
		}

		public int[] GetEnemyTeamIDs()
		{
			return enemyTeamsIDs;
		}

		public void ApplyDamage(float h)
		{
			healthScriptHolder.SendMessage("Damage", h, SendMessageOptions.DontRequireReceiver);
		}
	}
}
