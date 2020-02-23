namespace TacticalAI
{
	public class MoveToTransform : CustomAIBehaviour
	{
		public override void Initiate()
		{
			base.Initiate();
		}

		public override void AICycle()
		{
			if ((bool)baseScript.keyTransform)
			{
				targetVector = baseScript.keyTransform.position;
			}
		}
	}
}
