using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class ThrowGrenade : CustomAIBehaviour
	{
		private GameObject currentGrenade;

		public override void Initiate()
		{
			base.Initiate();
			StartCoroutine(TossGrenade());
		}

		private IEnumerator TossGrenade()
		{
			targetVector = myTransform.position;
			navI.DisableAgent();
			animationScript.StopRot();
			animationScript.PlayGrenadeAnimation();
			currentGrenade = Object.Instantiate(baseScript.grenadePrefab, baseScript.grenadeSpawn.position, baseScript.grenadeSpawn.rotation);
			currentGrenade.transform.parent = baseScript.grenadeSpawn;
			yield return new WaitForSeconds(baseScript.grenadeDelay);
			currentGrenade.transform.parent = null;
			currentGrenade.SendMessage("SetTarget", baseScript.targetPos, SendMessageOptions.DontRequireReceiver);
			currentGrenade = null;
			yield return new WaitForSeconds(baseScript.remainingAnimDelay);
			navI.EnableAgent();
			animationScript.StartRot();
			navI.SetSpeed(baseScript.runSpeed);
			baseScript.isStaggered = false;
			myTransform.position = animationScript.myAIBodyTransform.position;
			KillBehaviour();
		}

		public void OnDestroy()
		{
			OnEndBehaviour();
		}

		public override void OnEndBehaviour()
		{
			if ((bool)baseScript)
			{
				baseScript.isStaggered = false;
			}
			if ((bool)currentGrenade)
			{
				currentGrenade.transform.parent = null;
				currentGrenade.SendMessage("DropGrenade", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
