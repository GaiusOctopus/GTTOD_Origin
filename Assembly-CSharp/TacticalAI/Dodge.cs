using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class Dodge : CustomAIBehaviour
	{
		private float myOrigStoppingDist;

		private float origAcceleration;

		private bool dodgeRight;

		public override void Initiate()
		{
			base.Initiate();
			myOrigStoppingDist = navI.GetStoppingDistance();
			origAcceleration = navI.GetAcceleration();
		}

		private IEnumerator SetDodge()
		{
			navI.SetSpeed(baseScript.dodgingSpeed);
			navI.SetStoppingDistance(0f);
			navI.SetAcceleration(10000f);
			baseScript.isDodging = true;
			animationScript.StopRot();
			while (!navI.HasPath())
			{
				yield return null;
			}
			if (!navI.HaveOffMeshLinkInPath())
			{
				animationScript.PlayDodgingAnimation(dodgeRight);
				yield return new WaitForSeconds(baseScript.dodgingTime);
			}
			animationScript.StartRot();
			navI.SetAcceleration(origAcceleration);
			navI.SetStoppingDistance(myOrigStoppingDist);
			navI.SetSpeed(baseScript.runSpeed);
			baseScript.isDodging = false;
			KillBehaviour();
		}

		private bool AquireDodgingTarget()
		{
			Vector3 position = myTransform.position;
			if (Random.value < 0.5f)
			{
				position += animationScript.myAIBodyTransform.right * baseScript.dodgingSpeed * baseScript.dodgingTime;
				position.y = myTransform.position.y;
				dodgeRight = true;
			}
			else
			{
				position += -animationScript.myAIBodyTransform.right * baseScript.dodgingSpeed * baseScript.dodgingTime;
				position.y = myTransform.position.y;
				dodgeRight = false;
			}
			if (!Physics.Linecast(myTransform.position + baseScript.dodgeClearHeightCheckPos, position + baseScript.dodgeClearHeightCheckPos, out RaycastHit hitInfo, layerMask.value))
			{
				Debug.DrawLine(myTransform.position + baseScript.dodgeClearHeightCheckPos, hitInfo.point, Color.green);
				targetVector = position;
				return true;
			}
			Debug.DrawLine(myTransform.position + baseScript.dodgeClearHeightCheckPos, position, Color.red);
			return false;
		}

		public override void OnEndBehaviour()
		{
			animationScript.StartRot();
			navI.SetAcceleration(origAcceleration);
			navI.SetStoppingDistance(myOrigStoppingDist);
			navI.SetSpeed(baseScript.runSpeed);
			baseScript.isDodging = false;
			Object.Destroy(this);
		}

		public override void AICycle()
		{
			if (!baseScript.isDodging)
			{
				if (AquireDodgingTarget())
				{
					StartCoroutine(SetDodge());
				}
				else
				{
					KillBehaviour();
				}
			}
		}
	}
}
