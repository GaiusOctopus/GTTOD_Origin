using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class Stagger : CustomAIBehaviour
	{
		public override void Initiate()
		{
			base.Initiate();
			if (CheckIfStagger())
			{
				StartCoroutine(StaggerLoop());
			}
			else
			{
				KillBehaviour();
			}
		}

		private IEnumerator StaggerLoop()
		{
			targetVector = myTransform.position;
			navI.DisableAgent();
			animationScript.StopRot();
			animationScript.PlayStaggerAnimation();
			yield return new WaitForSeconds(baseScript.staggerTime);
			navI.EnableAgent();
			animationScript.StartRot();
			navI.SetSpeed(baseScript.runSpeed);
			baseScript.isStaggered = false;
			myTransform.position = animationScript.myAIBodyTransform.position;
			KillBehaviour();
		}

		private bool CheckIfStagger()
		{
			Vector3 position = myTransform.position;
			position -= animationScript.myAIBodyTransform.forward * baseScript.minDistRequiredToStagger;
			position.y = myTransform.position.y;
			if (!Physics.Linecast(myTransform.position, position, out RaycastHit _, layerMask.value))
			{
				return true;
			}
			return false;
		}

		public override void OnEndBehaviour()
		{
			baseScript.isStaggered = false;
		}
	}
}
