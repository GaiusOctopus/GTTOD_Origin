using UnityEngine;

namespace TacticalAI
{
	public class InvestigateSound : CustomAIBehaviour
	{
		private float radiusToCallOffSearch;

		public virtual void Inititae()
		{
			base.Initiate();
			radiusToCallOffSearch = baseScript.radiusToCallOffSearch;
		}

		public override void AICycle()
		{
			if (navI.GetRemainingDistance() < radiusToCallOffSearch || baseScript.IsEnaging())
			{
				KillBehaviour();
			}
			else
			{
				targetVector = baseScript.lastHeardNoisePos;
			}
		}

		public override void OnEndBehaviour()
		{
			Object.Destroy(this);
		}
	}
}
