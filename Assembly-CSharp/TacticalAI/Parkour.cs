namespace TacticalAI
{
	public class Parkour : CustomAIBehaviour
	{
		private TacticalNavLink linkObj;

		private bool origRotateToAimGun;

		private bool started;

		public override void Initiate()
		{
			base.Initiate();
			if (ControllerScript.currentController.GetClosestParkourLink(base.transform.position, ref linkObj))
			{
				started = true;
				animationScript.Parkour(linkObj.destTransform.position - linkObj.position, linkObj.animString, linkObj.position);
			}
		}

		public override void EachFrame()
		{
			if (!animationScript.onLink)
			{
				KillBehaviour();
			}
		}

		public override void OnEndBehaviour()
		{
			if (started)
			{
				navI.CompleteOffMeshLink();
				myTransform.position = animationScript.myAIBodyTransform.position;
				myTransform.rotation = animationScript.myAIBodyTransform.rotation;
			}
			baseScript.inParkour = false;
		}
	}
}
