using UnityEngine;

namespace TacticalAI
{
	public class CustomAIBehaviour : MonoBehaviour
	{
		public enum BehaviourLevel
		{
			None,
			Idle,
			Combat
		}

		[HideInInspector]
		public BaseScript baseScript;

		[HideInInspector]
		public GunScript gunScript;

		[HideInInspector]
		public SoundScript soundScript;

		[HideInInspector]
		public AnimationScript animationScript;

		[HideInInspector]
		public CoverFinderScript coverFinderScript;

		[HideInInspector]
		public Transform myTransform;

		[HideInInspector]
		public NavmeshInterface navI;

		[HideInInspector]
		public LayerMask layerMask;

		public Vector3 targetVector;

		public BehaviourLevel behaveLevel;

		public void Start()
		{
			ApplyBehaviour();
		}

		public virtual void Initiate()
		{
			if ((bool)base.gameObject.GetComponent<BaseScript>())
			{
				baseScript = base.gameObject.GetComponent<BaseScript>();
				gunScript = baseScript.gunScript;
				soundScript = baseScript.audioScript;
				animationScript = baseScript.animationScript;
				coverFinderScript = baseScript.coverFinderScript;
				myTransform = baseScript.GetTranform();
				layerMask = ControllerScript.currentController.GetLayerMask();
				navI = baseScript.GetAgent();
			}
		}

		public virtual void AICycle()
		{
		}

		public virtual void EachFrame()
		{
		}

		public void KillBehaviour()
		{
			OnEndBehaviour();
			Object.Destroy(this);
		}

		public virtual void OnEndBehaviour()
		{
		}

		public void ApplyBehaviour()
		{
			if (behaveLevel == BehaviourLevel.Idle)
			{
				Initiate();
				if ((bool)baseScript)
				{
					baseScript.SetIdleBehaviour(this);
				}
			}
			else if (behaveLevel == BehaviourLevel.Combat)
			{
				Initiate();
				if ((bool)baseScript)
				{
					baseScript.SetCombatBehaviour(this);
				}
			}
		}
	}
}
