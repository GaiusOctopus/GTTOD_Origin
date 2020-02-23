using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class BaseScript : MonoBehaviour
	{
		public enum AIType
		{
			Berserker,
			Tactical,
			Custom,
			Skirmish
		}

		public enum IdleBehaviour
		{
			Search = 1,
			Patrol,
			Wander,
			MoveToTransform,
			Custom
		}

		public enum AIBehaviour
		{
			GoToMoveTarget,
			Cover,
			Search,
			Dodging,
			Patrolling,
			Wander,
			UseDynamicObject,
			InvestigateSound,
			Custom,
			MoveToTransform,
			RunFromGrenade,
			Skirmish,
			Parkour,
			Stagger,
			ThrowGrenade
		}

		public GunScript gunScript;

		public SoundScript audioScript;

		public AnimationScript animationScript;

		public CoverFinderScript coverFinderScript;

		public Transform targetTransform;

		private LayerMask layerMask;

		public NavmeshInterface navmeshInterfaceClass;

		public NavmeshInterface navI;

		private Transform myTransform;

		private TargetScript myTargetScript;

		public float origAgentStoppingDist;

		public float sprintSpeed = 6f;

		public float runSpeed = 5f;

		public float alertSpeed = 4f;

		public float idleSpeed = 3f;

		public AIType myAIType = AIType.Tactical;

		public IdleBehaviour myIdleBehaviour = IdleBehaviour.Wander;

		private bool engaging;

		public float cycleTime = 0.2f;

		public float radiusToCallOffSearch = 5f;

		public Vector3 lastHeardNoisePos;

		public bool inCover;

		[HideInInspector]
		public CoverNodeScript currentCoverNodeScript;

		public float timeBetweenSafetyChecks = 1f;

		public Vector3 currentCoverNodePos;

		public Vector3 currentCoverNodeFiringPos;

		public float maxTimeInCover = 10f;

		public float minTimeInCover = 5f;

		public bool shouldFireFromCover = true;

		private bool foundDynamicCover;

		public float minDistToTargetIfNotInCover = 5f;

		public bool useAdvancedCover;

		public float dodgingSpeed = 6f;

		public float dodgingTime = 0.5f;

		private Vector3 dodgingTarget;

		public float dodgingClearHeight = 1f;

		[HideInInspector]
		public Vector3 dodgeClearHeightCheckPos;

		[HideInInspector]
		public float origAcceleration;

		private float timeUntilNextDodge;

		public float timeBetweenLoSDodges = 4f;

		public bool shouldTryAndDodge = true;

		[HideInInspector]
		public bool isDodging;

		public float minDistToDodge = 5f;

		public float closeEnoughToPatrolNodeDist = 5f;

		public Transform[] patrolNodes;

		public bool shouldShowPatrolPath;

		public float wanderDiameter = 50f;

		public float distToChooseNewWanderPoint = 4f;

		public Transform keyTransform;

		public bool canMelee;

		public float meleeDamage = 100f;

		public float timeBetweenMelees = 2f;

		public float meleeRange = 2f;

		public float timeUntilMeleeDamageIsDealt = 0.2f;

		[HideInInspector]
		public bool isMeleeing;

		public bool canSprint;

		public float distFromTargetToSprint = 25f;

		public float minSkirmishDistFromTarget = 7f;

		public float maxSkirmishDistFromTarget = 20f;

		public bool canCrossBehindTarget = true;

		public float maxTimeToWaitAtEachSkirmishPoint = 3f;

		private float perfCycleTime = 2f;

		private CustomAIBehaviour currentBehaviour;

		private bool canOverrideBehaviour = true;

		public CustomAIBehaviour idleBehaviour;

		public CustomAIBehaviour combatBehaviour;

		public CustomAIBehaviour tacticalBehaviour;

		private bool usingOverrideBehaviour;

		private Vector3 vectorLast;

		private bool isSprinting;

		public Transform transformToRunFrom;

		public float distToRunFromGrenades = 10f;

		public bool shouldRunFromGrenades = true;

		private Transform movementTargetTransform;

		private string methodToCall;

		private AIBehaviour lastBehavior;

		private string dynamicObjectAnimation;

		[HideInInspector]
		public bool usingDynamicObject;

		public bool canUseDynamicObject = true;

		public float timeUntilBodyIsDestroyedAfterDeath = 60f;

		public bool inParkour;

		public bool canParkour;

		public bool isStaggered;

		public float staggerSpeed = 4f;

		public float staggerTime = 1.5f;

		public float minDistRequiredToStagger = 2f;

		public GameObject grenadePrefab;

		public Vector3 targetPos;

		public Transform grenadeSpawn;

		public float grenadeDelay = 0.25f;

		public float remainingAnimDelay = 0.25f;

		private SpawnerScript spawnerScript;

		private bool spawnerScriptExists;

		private void Awake()
		{
			myTransform = base.transform;
			timeUntilNextDodge = timeBetweenLoSDodges * Random.value;
			dodgeClearHeightCheckPos = Vector3.zero;
			dodgeClearHeightCheckPos.y = dodgingClearHeight;
			distFromTargetToSprint *= distFromTargetToSprint;
			meleeRange *= meleeRange;
			if (navmeshInterfaceClass == null)
			{
				navI = (NavmeshInterface)base.gameObject.AddComponent(typeof(NavmeshInterface));
				navI.Initialize(base.gameObject);
			}
			else
			{
				navI = navmeshInterfaceClass;
				navI.Initialize(base.gameObject);
			}
			if (idleSpeed > runSpeed)
			{
				idleSpeed = runSpeed;
			}
		}

		private void Start()
		{
			GetDefaultBehaviours();
			if (ControllerScript.currentController != null)
			{
				layerMask = ControllerScript.currentController.GetLayerMask();
			}
			else
			{
				base.enabled = false;
			}
			if (!ControllerScript.pMode)
			{
				StartCoroutine("AICycle");
			}
			else
			{
				StartCoroutine("PerformanceAICycle");
			}
		}

		private IEnumerator PerformanceAICycle()
		{
			while (base.enabled)
			{
				if (currentBehaviour != null)
				{
					currentBehaviour.AICycle();
				}
				MoveAI();
				yield return new WaitForSeconds(perfCycleTime);
			}
		}

		private IEnumerator AICycle()
		{
			while (base.enabled)
			{
				if (currentBehaviour != null)
				{
					currentBehaviour.AICycle();
				}
				if (!isDodging)
				{
					if ((bool)myTransform && (bool)targetTransform && canMelee && !isMeleeing && Vector3.SqrMagnitude(myTransform.position - targetTransform.position) < meleeRange)
					{
						isSprinting = false;
						animationScript.StopSprinting();
						SetProperSpeed();
						StartCoroutine(AttackInMelee());
					}
					else if (!isMeleeing && Vector3.SqrMagnitude(myTransform.position - currentBehaviour.targetVector) > distFromTargetToSprint && canSprint && engaging)
					{
						animationScript.StartSprinting();
						SetSprintSpeed();
					}
				}
				MoveAI();
				if (!targetTransform && !engaging)
				{
					isSprinting = false;
					animationScript.StopSprinting();
				}
				else if (Vector3.SqrMagnitude(myTransform.position - currentBehaviour.targetVector) < distFromTargetToSprint && engaging && !isDodging)
				{
					animationScript.StopSprinting();
					SetProperSpeed();
				}
				yield return new WaitForSeconds(cycleTime);
			}
		}

		public void SetIdleBehaviour(CustomAIBehaviour c)
		{
			idleBehaviour = c;
			if (canOverrideBehaviour)
			{
				SetBehaviour();
			}
		}

		public void SetCombatBehaviour(CustomAIBehaviour c)
		{
			combatBehaviour = c;
			if (canOverrideBehaviour)
			{
				SetBehaviour();
			}
		}

		public bool hasOverrideBehaviour()
		{
			return !canOverrideBehaviour;
		}

		private void SetBehaviour()
		{
			SetProperSpeed();
			if ((bool)currentBehaviour)
			{
				currentBehaviour.OnEndBehaviour();
			}
			if ((bool)tacticalBehaviour)
			{
				SetCurrentBehaviour(tacticalBehaviour);
			}
			else if (engaging && (bool)combatBehaviour)
			{
				SetCurrentBehaviour(combatBehaviour);
			}
			else if ((bool)idleBehaviour)
			{
				SetCurrentBehaviour(idleBehaviour);
			}
		}

		public void SetTacticalBehaviour(CustomAIBehaviour c)
		{
			tacticalBehaviour = c;
			if (!engaging)
			{
				StartEngage();
			}
			if (canOverrideBehaviour)
			{
				SetBehaviour();
			}
		}

		private void SetCurrentBehaviour(CustomAIBehaviour b)
		{
			if ((bool)currentBehaviour && canOverrideBehaviour)
			{
				currentBehaviour.OnEndBehaviour();
			}
			currentBehaviour = b;
		}

		public void SetOverrideBehaviour(CustomAIBehaviour c, bool b)
		{
			if (canOverrideBehaviour)
			{
				usingOverrideBehaviour = true;
				if ((bool)currentBehaviour)
				{
					currentBehaviour.OnEndBehaviour();
				}
				canOverrideBehaviour = !b;
				currentBehaviour = c;
			}
		}

		public void KillOverrideBehaviour()
		{
			if (usingOverrideBehaviour)
			{
				currentBehaviour.KillBehaviour();
			}
		}

		private CustomAIBehaviour GetIdleBehaviour()
		{
			switch (myIdleBehaviour)
			{
			case IdleBehaviour.Search:
				return GetNewBehaviour(AIBehaviour.Search);
			case IdleBehaviour.Wander:
				return GetNewBehaviour(AIBehaviour.Wander);
			case IdleBehaviour.Patrol:
				return GetNewBehaviour(AIBehaviour.Patrolling);
			case IdleBehaviour.MoveToTransform:
				return GetNewBehaviour(AIBehaviour.MoveToTransform);
			default:
				return null;
			}
		}

		private CustomAIBehaviour GetCombatBehaviour()
		{
			if (myAIType == AIType.Tactical)
			{
				return GetNewBehaviour(AIBehaviour.Cover);
			}
			if (myAIType == AIType.Berserker)
			{
				return GetNewBehaviour(AIBehaviour.GoToMoveTarget);
			}
			if (myAIType == AIType.Skirmish)
			{
				return GetNewBehaviour(AIBehaviour.Skirmish);
			}
			return null;
		}

		private void GetDefaultBehaviours()
		{
			SetCombatBehaviour(GetCombatBehaviour());
			SetIdleBehaviour(GetIdleBehaviour());
		}

		public CustomAIBehaviour GetNewBehaviour(AIBehaviour t)
		{
			CustomAIBehaviour customAIBehaviour = null;
			switch (t)
			{
			case AIBehaviour.Search:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Search));
				break;
			case AIBehaviour.Dodging:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Dodge));
				break;
			case AIBehaviour.Cover:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Cover));
				break;
			case AIBehaviour.Patrolling:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Patrol));
				break;
			case AIBehaviour.Wander:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Wander));
				break;
			case AIBehaviour.GoToMoveTarget:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(ChargeTarget));
				break;
			case AIBehaviour.UseDynamicObject:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(DynamicObject));
				break;
			case AIBehaviour.InvestigateSound:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(InvestigateSound));
				break;
			case AIBehaviour.MoveToTransform:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(MoveToTransform));
				break;
			case AIBehaviour.RunFromGrenade:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(RunAwayFromGrenade));
				break;
			case AIBehaviour.Skirmish:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Skirmish));
				break;
			case AIBehaviour.Stagger:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Stagger));
				break;
			case AIBehaviour.Parkour:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(Parkour));
				break;
			case AIBehaviour.ThrowGrenade:
				customAIBehaviour = (CustomAIBehaviour)base.gameObject.AddComponent(typeof(ThrowGrenade));
				break;
			}
			if (customAIBehaviour == null)
			{
				Debug.Log(t);
			}
			customAIBehaviour.Initiate();
			return customAIBehaviour;
		}

		private void MoveAI()
		{
			if (base.enabled && (bool)currentBehaviour && currentBehaviour.targetVector != vectorLast)
			{
				navI.SetDestination(currentBehaviour.targetVector);
			}
			vectorLast = currentBehaviour.targetVector;
		}

		public void SetMyTarget(Transform currentEnemyTransform, Transform losTargetTransform)
		{
			targetTransform = currentEnemyTransform;
			if ((bool)gunScript)
			{
				gunScript.AssignTarget(targetTransform, losTargetTransform);
			}
		}

		public void RemoveMyTarget()
		{
			targetTransform = null;
		}

		public void StartEngage()
		{
			if ((bool)animationScript)
			{
				animationScript.SetEngaging();
			}
			if ((bool)audioScript)
			{
				audioScript.PlaySpottedAudio();
			}
			engaging = true;
			if (canOverrideBehaviour)
			{
				SetBehaviour();
			}
			if ((bool)navI)
			{
				navI.SetSpeed(runSpeed);
			}
		}

		public void EndEngage()
		{
			engaging = false;
			if (canOverrideBehaviour)
			{
				SetBehaviour();
			}
			if ((bool)gunScript)
			{
				gunScript.EndEngage();
			}
		}

		public void SetSpeed(float x)
		{
			navI.SetSpeed(x);
		}

		public void SetAlertSpeed()
		{
			navI.SetSpeed(alertSpeed);
		}

		public void SetSprintSpeed()
		{
			navI.SetSpeed(sprintSpeed);
			isSprinting = true;
		}

		public void SetProperSpeed()
		{
			if (engaging)
			{
				navI.SetSpeed(runSpeed);
			}
			else
			{
				navI.SetSpeed(idleSpeed);
			}
		}

		private void LateUpdate()
		{
			if (currentBehaviour != null)
			{
				currentBehaviour.EachFrame();
			}
			else
			{
				canOverrideBehaviour = true;
				SetBehaviour();
			}
			if (navI.OnNavmeshLink() && !inParkour && canParkour)
			{
				StartParkour();
			}
			if (!inCover && !ControllerScript.pMode && !inParkour && !isDodging && !isMeleeing && !isStaggered && !navI.OnNavmeshLink() && (bool)targetTransform && shouldTryAndDodge && engaging)
			{
				if (timeUntilNextDodge < 0f)
				{
					CheckToSeeIfWeShouldDodge();
					timeUntilNextDodge = Random.value * timeBetweenLoSDodges;
				}
				else
				{
					timeUntilNextDodge -= Time.deltaTime;
				}
			}
		}

		private IEnumerator WaitBetweenDodges()
		{
			if (shouldTryAndDodge)
			{
				shouldTryAndDodge = false;
				yield return new WaitForSeconds(timeBetweenLoSDodges + dodgingTime);
				shouldTryAndDodge = true;
			}
			yield return null;
		}

		public void CheckToSeeIfWeShouldDodge()
		{
			if (shouldTryAndDodge && (bool)myTransform && (bool)targetTransform && !inCover && !isDodging && !isSprinting && !isStaggered && !Physics.Linecast(myTransform.position, targetTransform.position, layerMask) && Vector3.Distance(myTransform.position, targetTransform.position) > minDistToDodge && !gunScript.IsFiring() && canOverrideBehaviour)
			{
				SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.Dodging), b: true);
				StartCoroutine(WaitBetweenDodges());
			}
		}

		private IEnumerator AttackInMelee()
		{
			isMeleeing = true;
			navI.SetSpeed(0f);
			if ((bool)animationScript)
			{
				StartCoroutine(animationScript.StartMelee());
			}
			yield return new WaitForSeconds(timeUntilMeleeDamageIsDealt);
			DealDamage(meleeDamage, meleeRange, targetTransform.position);
		}

		private void DealDamage(float damage, float range, Vector3 enemyPos)
		{
			if (Vector3.SqrMagnitude(enemyPos - myTransform.position) <= range * range)
			{
				myTargetScript.currentEnemyTarget.targetScript.ApplyDamage(meleeDamage);
			}
		}

		public void StopMelee()
		{
			SetProperSpeed();
			isMeleeing = false;
		}

		private void OnDrawGizmos()
		{
			if (!shouldShowPatrolPath || patrolNodes.Length <= 1)
			{
				return;
			}
			for (int i = 1; i < patrolNodes.Length; i++)
			{
				if ((bool)patrolNodes[i])
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(patrolNodes[i].position, patrolNodes[i - 1].position);
				}
			}
		}

		public void WarnOfGrenade(Transform t, float d)
		{
			if (canOverrideBehaviour && shouldRunFromGrenades)
			{
				transformToRunFrom = t;
				distToRunFromGrenades = d;
				SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.RunFromGrenade), b: true);
			}
		}

		public bool SetDynamicObject(Transform newMovementObjectTransform, string anim, string newMethodToCall, bool requireEngaging)
		{
			return SetDynamicObject(newMovementObjectTransform, anim, newMethodToCall, requireEngaging, 1f);
		}

		public bool SetDynamicObject(Transform newMovementObjectTransform, string anim, string newMethodToCall, bool requireEngaging, float timeToWait)
		{
			if (!isDodging && (!requireEngaging || engaging) && canOverrideBehaviour && canUseDynamicObject)
			{
				DynamicObject dynamicObject = (DynamicObject)GetNewBehaviour(AIBehaviour.UseDynamicObject);
				SetOverrideBehaviour(dynamicObject, b: true);
				dynamicObject.StartDynamicObject(newMovementObjectTransform, anim, newMethodToCall, requireEngaging, timeToWait);
				return true;
			}
			return false;
		}

		public IEnumerator HearSound(Vector3 s)
		{
			yield return new WaitForSeconds(0.1f);
			lastHeardNoisePos = s;
			SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.InvestigateSound), b: false);
		}

		public void KillAI()
		{
			if (base.enabled)
			{
				if ((bool)combatBehaviour)
				{
					combatBehaviour.KillBehaviour();
				}
				if ((bool)idleBehaviour)
				{
					idleBehaviour.KillBehaviour();
				}
				base.gameObject.SendMessage("OnAIDeath", SendMessageOptions.DontRequireReceiver);
				if (spawnerScriptExists)
				{
					spawnerScript.AgentDied();
				}
				Object.Destroy(animationScript.myAIBodyTransform.gameObject, timeUntilBodyIsDestroyedAfterDeath);
				Object.Destroy(base.gameObject);
			}
		}

		public void SetTargetObj(TargetScript x)
		{
			myTargetScript = x;
		}

		public void SetOrigStoppingDistance()
		{
			if ((bool)navI)
			{
				navI.SetStoppingDistance(origAgentStoppingDist);
			}
		}

		public void ShouldFireFromCover(bool b)
		{
			shouldFireFromCover = b;
		}

		public bool UsingDynamicObject()
		{
			return usingDynamicObject;
		}

		public bool HaveCover()
		{
			if (!(currentCoverNodeScript != null))
			{
				return foundDynamicCover;
			}
			return true;
		}

		public Transform GetTranform()
		{
			return myTransform;
		}

		public bool IsEnaging()
		{
			return engaging;
		}

		public float MaxSpeed()
		{
			return runSpeed;
		}

		public Vector3 GetCurrentCoverNodePos()
		{
			return currentCoverNodePos;
		}

		public LayerMask GetLayerMask()
		{
			return layerMask;
		}

		public bool isCurrentlyActive()
		{
			if ((bool)this)
			{
				return base.enabled;
			}
			return false;
		}

		public int[] GetEnemyTeamIDs()
		{
			return GetEnemyIDsFromTargetObj();
		}

		public NavmeshInterface GetAgent()
		{
			return navI;
		}

		public float GetWanderDiameter()
		{
			return wanderDiameter;
		}

		public float GetDistToChooseNewWanderPoint()
		{
			return distToChooseNewWanderPoint;
		}

		public Vector3 GetEyePos()
		{
			return myTargetScript.eyeTransform.position;
		}

		private int[] GetEnemyIDsFromTargetObj()
		{
			if ((bool)myTargetScript)
			{
				return myTargetScript.GetEnemyTeamIDs();
			}
			return null;
		}

		private void StartParkour()
		{
			if (isDodging)
			{
				currentBehaviour.KillBehaviour();
			}
			inParkour = true;
			SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.Parkour), b: true);
		}

		public void StaggerAgent()
		{
			if (!isDodging && !isMeleeing && !inParkour && !isStaggered && !navI.OnNavmeshLink() && (!useAdvancedCover || !inCover))
			{
				isStaggered = true;
				SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.Stagger), b: false);
				WaitBetweenDodges();
			}
		}

		public void ThrowGrenade(GameObject prefab, Vector3 tPos, Transform spawnPos)
		{
			if (!isDodging && !isMeleeing && !inParkour && !isStaggered && !navI.OnNavmeshLink() && (!useAdvancedCover || !inCover))
			{
				grenadeSpawn = spawnPos;
				targetPos = tPos;
				grenadePrefab = prefab;
				isStaggered = true;
				SetOverrideBehaviour(GetNewBehaviour(AIBehaviour.ThrowGrenade), b: false);
				WaitBetweenDodges();
			}
		}

		public bool CanStartCommand()
		{
			if (!isStaggered && !isDodging && !inParkour && !usingDynamicObject)
			{
				return true;
			}
			return false;
		}

		public void SetWaveSpawner(SpawnerScript w)
		{
			spawnerScript = w;
			spawnerScriptExists = true;
		}
	}
}
