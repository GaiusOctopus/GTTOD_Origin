using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class DynamicObjectAddForceScript : MonoBehaviour
	{
		public Rigidbody rigidBodyToAddForceTo;

		public float forceToAdd = 100f;

		public bool shouldResetKinematic;

		public float timeUntilIsKinematicAgain = 2f;

		public Vector3 relativeVectorToAddForceIn = Vector3.forward;

		public bool showVector;

		public CoverNodeScript coverNodeToActivate;

		private void Awake()
		{
			if ((bool)coverNodeToActivate)
			{
				coverNodeToActivate.transform.parent = null;
			}
		}

		public IEnumerator UseDynamicObject()
		{
			if ((bool)rigidBodyToAddForceTo)
			{
				rigidBodyToAddForceTo.isKinematic = false;
				Vector3 force = GetVectorToAddForceIn() * forceToAdd;
				rigidBodyToAddForceTo.AddForce(force, ForceMode.Impulse);
				if ((bool)coverNodeToActivate)
				{
					coverNodeToActivate.ActivateNode(1f);
				}
				if (shouldResetKinematic)
				{
					yield return new WaitForSeconds(timeUntilIsKinematicAgain);
					rigidBodyToAddForceTo.isKinematic = true;
				}
			}
		}

		private Vector3 GetVectorToAddForceIn()
		{
			return base.transform.forward * relativeVectorToAddForceIn.z + base.transform.up * relativeVectorToAddForceIn.y + base.transform.right * relativeVectorToAddForceIn.x;
		}

		private void OnDrawGizmos()
		{
			if (showVector)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(rigidBodyToAddForceTo.position, GetVectorToAddForceIn());
			}
		}
	}
}
