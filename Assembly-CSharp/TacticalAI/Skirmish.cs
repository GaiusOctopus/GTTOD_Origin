using UnityEngine;

namespace TacticalAI
{
	public class Skirmish : CustomAIBehaviour
	{
		public float minDistFromTarget = 7f;

		public float maxDistFromTarget = 20f;

		private bool haveADestTarget;

		private int framesUntilCanReachObject;

		public bool canCrossBehindTarget = true;

		public float maxTimeToWaitAtEachPoint = 3f;

		private float timeLeftAtThisPoint;

		public override void Initiate()
		{
			base.Initiate();
			minDistFromTarget = baseScript.minSkirmishDistFromTarget;
			maxDistFromTarget = baseScript.maxSkirmishDistFromTarget;
			canCrossBehindTarget = baseScript.canCrossBehindTarget;
			maxTimeToWaitAtEachPoint = baseScript.maxTimeToWaitAtEachSkirmishPoint;
		}

		public override void AICycle()
		{
			if (haveADestTarget)
			{
				Debug.DrawLine(myTransform.position, targetVector, Color.yellow, 0.2f);
			}
			if (!haveADestTarget && timeLeftAtThisPoint <= 0f)
			{
				targetVector = GetNewDestTarget(baseScript.targetTransform);
			}
			else if (haveADestTarget && framesUntilCanReachObject < 0 && navI.ReachedDestination() && !baseScript.usingDynamicObject)
			{
				haveADestTarget = false;
				timeLeftAtThisPoint = maxTimeToWaitAtEachPoint * Random.value;
			}
			framesUntilCanReachObject--;
			timeLeftAtThisPoint -= baseScript.cycleTime;
		}

		private Vector3 GetNewDestTarget(Transform targ)
		{
			Vector3 lhs = new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f);
			if (!canCrossBehindTarget && Vector3.Dot(lhs, targ.position - myTransform.position) > 0f)
			{
				lhs *= -1f;
			}
			lhs = lhs.normalized;
			Vector3 vector = targ.position + lhs * Random.Range(minDistFromTarget, maxDistFromTarget);
			if (!vector.Equals(Vector3.zero))
			{
				if (Physics.Linecast(targ.position, vector + new Vector3(0f, baseScript.dodgingClearHeight, 0f), out RaycastHit hitInfo, layerMask.value))
				{
					framesUntilCanReachObject = 5;
					haveADestTarget = true;
					return hitInfo.point;
				}
				framesUntilCanReachObject = 5;
				haveADestTarget = true;
				return vector;
			}
			return targ.position;
		}

		public override void OnEndBehaviour()
		{
			haveADestTarget = false;
		}
	}
}
