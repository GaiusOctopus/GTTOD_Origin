using UnityEngine;

namespace TacticalAI
{
	public class Wander : CustomAIBehaviour
	{
		private bool haveCurrentWanderPoint;

		public virtual void Inititae()
		{
			base.Initiate();
		}

		public override void EachFrame()
		{
			Debug.DrawLine(myTransform.position, targetVector);
		}

		public override void AICycle()
		{
			if (!haveCurrentWanderPoint)
			{
				if (!baseScript.keyTransform)
				{
					targetVector = FindDestinationWithinRadius(myTransform.position);
				}
				else
				{
					targetVector = FindDestinationWithinRadius(baseScript.keyTransform.position);
				}
				haveCurrentWanderPoint = true;
			}
			else if (!navI.PathPending() && navI.GetRemainingDistance() < baseScript.GetDistToChooseNewWanderPoint())
			{
				haveCurrentWanderPoint = false;
			}
		}

		public Vector3 FindDestinationWithinRadius(Vector3 originPos)
		{
			return new Vector3(originPos.x + (Random.value - 0.5f) * baseScript.GetWanderDiameter(), originPos.y, originPos.z + (Random.value - 0.5f) * baseScript.GetWanderDiameter());
		}
	}
}
