namespace TacticalAI
{
	public class Search : CustomAIBehaviour
	{
		private float radiusToCallOffSearch;

		public override void Initiate()
		{
			base.Initiate();
			radiusToCallOffSearch = baseScript.radiusToCallOffSearch;
		}

		public override void AICycle()
		{
			if ((bool)baseScript.targetTransform && navI.GetRemainingDistance() < radiusToCallOffSearch)
			{
				targetVector = baseScript.targetTransform.position;
			}
			else if (!baseScript.targetTransform)
			{
				targetVector = myTransform.position;
			}
		}
	}
}
