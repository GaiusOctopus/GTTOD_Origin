using UnityEngine;

namespace TacticalAI
{
	public class Patrol : CustomAIBehaviour
	{
		private bool haveAPatrolTarget;

		private int currentPatrolIndex;

		private float patrolNodeDistSquared;

		public virtual void Inititae()
		{
			base.Initiate();
		}

		public override void AICycle()
		{
			if (baseScript.patrolNodes.Length >= 0)
			{
				if (!haveAPatrolTarget)
				{
					SetPatrolNodeDistSquared();
					targetVector = baseScript.patrolNodes[currentPatrolIndex].position;
					haveAPatrolTarget = true;
					currentPatrolIndex++;
					if (currentPatrolIndex >= baseScript.patrolNodes.Length)
					{
						currentPatrolIndex = 0;
					}
				}
				else if (Vector3.SqrMagnitude(targetVector - myTransform.position) < patrolNodeDistSquared)
				{
					haveAPatrolTarget = false;
				}
			}
			else
			{
				Debug.LogError("No patrol nodes set!  Please set the array in the inspector, via script, or change the AI's non-engaging behavior");
			}
		}

		private void SetPatrolNodeDistSquared()
		{
			patrolNodeDistSquared = baseScript.closeEnoughToPatrolNodeDist * baseScript.closeEnoughToPatrolNodeDist;
		}
	}
}
