using UnityEngine;
using UnityEngine.AI;

namespace TacticalAI
{
	public class TacticalNavLink : MonoBehaviour
	{
		public enum AnimType
		{
			Leap = 1,
			Vault
		}

		public AnimType animtype;

		public Vector3 position;

		[HideInInspector]
		public Transform destTransform;

		[HideInInspector]
		public string animString;

		public bool visualize = true;

		public float visualizationRadius = 0.5f;

		private void Awake()
		{
			position = base.transform.position;
			destTransform = base.gameObject.GetComponent<OffMeshLink>().endTransform;
			ControllerScript.currentController.AddParkourLink(this);
			switch (animtype)
			{
			case AnimType.Leap:
				animString = "Leap";
				break;
			case AnimType.Vault:
				animString = "Vault";
				break;
			}
		}

		public AnimType GetAnimationType()
		{
			return animtype;
		}

		private void OnDrawGizmos()
		{
			if (visualize)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawSphere(base.transform.position, visualizationRadius);
				if ((bool)base.gameObject.GetComponent<OffMeshLink>().endTransform)
				{
					Transform endTransform = base.gameObject.GetComponent<OffMeshLink>().endTransform;
					Gizmos.DrawSphere(endTransform.position, visualizationRadius / 2f);
					Gizmos.DrawLine(endTransform.position, base.transform.position);
				}
			}
		}
	}
}
