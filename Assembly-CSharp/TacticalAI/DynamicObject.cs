using UnityEngine;

namespace TacticalAI
{
	public class DynamicObject : CustomAIBehaviour
	{
		private Transform movementTargetTransform;

		private string methodToCall;

		private string dynamicObjectAnimation;

		private float timeToWait;

		private int framesUntilCanReachObject = 5;

		public void StartDynamicObject(Transform newMovementObjectTransform, string newAnimationToUse, string newMethodToCall, bool requireEngaging, float timeToWaitF)
		{
			movementTargetTransform = newMovementObjectTransform;
			dynamicObjectAnimation = newAnimationToUse;
			navI.SetStoppingDistance(0f);
			timeToWait = timeToWaitF;
			baseScript.usingDynamicObject = false;
			methodToCall = newMethodToCall;
		}

		private void UseDynamicObject()
		{
			navI.SetSpeed(0f);
			if ((bool)gunScript)
			{
				gunScript.SetCanCurrentlyFire(b: false);
			}
			StartCoroutine(animationScript.DynamicObjectAnimation(dynamicObjectAnimation, movementTargetTransform.forward, this, timeToWait));
			baseScript.usingDynamicObject = true;
		}

		public void AffectDynamicObject()
		{
			movementTargetTransform.gameObject.SendMessage(methodToCall);
		}

		public void EndDynamicObjectUsage()
		{
			if (baseScript.usingDynamicObject)
			{
				baseScript.SetProperSpeed();
				baseScript.SetOrigStoppingDistance();
				baseScript.usingDynamicObject = false;
				if ((bool)gunScript)
				{
					gunScript.SetCanCurrentlyFire(b: true);
				}
				movementTargetTransform = null;
			}
			KillBehaviour();
		}

		private void MoveToDynamicObject()
		{
			if ((bool)movementTargetTransform)
			{
				Debug.DrawLine(myTransform.position, movementTargetTransform.position, Color.green, baseScript.cycleTime);
				if (framesUntilCanReachObject < 0 && navI.ReachedDestination() && !baseScript.usingDynamicObject)
				{
					UseDynamicObject();
				}
				else if (!baseScript.usingDynamicObject)
				{
					targetVector = movementTargetTransform.position;
				}
				framesUntilCanReachObject--;
			}
			else
			{
				KillBehaviour();
			}
		}

		public override void AICycle()
		{
			MoveToDynamicObject();
		}

		public override void EachFrame()
		{
		}
	}
}
