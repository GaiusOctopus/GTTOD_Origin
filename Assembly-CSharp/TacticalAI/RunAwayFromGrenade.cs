using UnityEngine;
using UnityEngine.AI;

namespace TacticalAI
{
	public class RunAwayFromGrenade : CustomAIBehaviour
	{
		private float timeToGiveUp = 3f;

		public override void Initiate()
		{
			base.Initiate();
			GetPositionToMoveTo();
		}

		public override void AICycle()
		{
			if (timeToGiveUp < 0f || baseScript.transformToRunFrom == null || (!navI.PathPending() && navI.GetRemainingDistance() < 1f))
			{
				KillBehaviour();
			}
			timeToGiveUp -= Time.deltaTime;
		}

		private void GetPositionToMoveTo()
		{
			Vector3 position = baseScript.transformToRunFrom.position;
			position.x += (Random.value - 0.5f) * baseScript.distToRunFromGrenades;
			position.z = (Random.value - 0.5f) * baseScript.distToRunFromGrenades;
			NavMesh.SamplePosition(position, out NavMeshHit _, baseScript.distToRunFromGrenades, -1);
		}
	}
}
