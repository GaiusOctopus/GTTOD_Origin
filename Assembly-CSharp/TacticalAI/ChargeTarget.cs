using UnityEngine;

namespace TacticalAI
{
	public class ChargeTarget : CustomAIBehaviour
	{
		private float minDistToTargetIfNotInCover;

		public override void Initiate()
		{
			base.Initiate();
			minDistToTargetIfNotInCover = baseScript.minDistToTargetIfNotInCover * baseScript.minDistToTargetIfNotInCover;
		}

		public override void AICycle()
		{
			if ((bool)baseScript.keyTransform)
			{
				targetVector = baseScript.keyTransform.position;
			}
			else if ((bool)baseScript.targetTransform)
			{
				if (Vector3.SqrMagnitude(myTransform.position - baseScript.targetTransform.position) > minDistToTargetIfNotInCover || Physics.Linecast(baseScript.GetEyePos(), baseScript.targetTransform.position, layerMask))
				{
					targetVector = baseScript.targetTransform.position;
				}
				else
				{
					targetVector = myTransform.position;
				}
			}
		}
	}
}
