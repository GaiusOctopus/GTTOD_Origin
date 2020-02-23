using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class GrenadeScript : MonoBehaviour
	{
		public float timeTilExplode = 3f;

		public GameObject explosion;

		private Vector3 target;

		public float warningRadius = 7f;

		public float timeUntilWarningCanBeGiven = 1f;

		private bool hasTarget;

		public LayerMask layerMask;

		private bool warned;

		private bool canBeWarnedYet;

		private Rigidbody myRigidBody;

		public float runAwayBuffer = 3f;

		private void Go()
		{
			float d = 0f;
			if (hasTarget)
			{
				float num = Vector2.Distance(new Vector2(base.transform.position.x, base.transform.position.z), new Vector2(target.x, target.z));
				float num2 = 0f - (base.transform.position.y - target.y);
				d = num / Mathf.Sqrt(Mathf.Abs((num2 - num) / (0.5f * (0f - Physics.gravity.y))));
				d = 1.414f * d * GetComponent<Rigidbody>().mass;
				base.transform.Rotate(-45f, 0f, 0f);
			}
			myRigidBody = GetComponent<Rigidbody>();
			myRigidBody.AddForce(base.transform.forward * d, ForceMode.Impulse);
			StartCoroutine(StartDetonationTimer());
			StartCoroutine(SetTimeUntilWarning());
		}

		private void Warn()
		{
			if (warned)
			{
				return;
			}
			warningRadius *= warningRadius;
			Target[] currentTargets = GameObject.FindGameObjectWithTag("AI Controller").GetComponent<ControllerScript>().GetCurrentTargets();
			for (int i = 0; i < currentTargets.Length; i++)
			{
				if (Vector3.SqrMagnitude(myRigidBody.position - currentTargets[i].transform.position) < warningRadius && !Physics.Linecast(currentTargets[i].transform.position, myRigidBody.position, layerMask))
				{
					currentTargets[i].targetScript.WarnOfGrenade(base.transform, warningRadius + runAwayBuffer);
				}
			}
			warned = true;
		}

		private void DetonateGrenade()
		{
			if ((bool)explosion)
			{
				Object.Instantiate(explosion, base.transform.position, base.transform.rotation);
			}
			else
			{
				Debug.LogWarning("No explosion object assigned to grenade!");
			}
			Object.Destroy(base.gameObject);
		}

		private IEnumerator StartDetonationTimer()
		{
			yield return new WaitForSeconds(timeTilExplode);
			DetonateGrenade();
		}

		private IEnumerator SetTimeUntilWarning()
		{
			yield return new WaitForSeconds(timeUntilWarningCanBeGiven);
			canBeWarnedYet = true;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (canBeWarnedYet)
			{
				Warn();
			}
		}

		private void Awake()
		{
			base.gameObject.GetComponent<Rigidbody>().useGravity = false;
		}

		public void SetTarget(Vector3 pos)
		{
			target = pos;
			Debug.DrawLine(base.transform.position, pos);
			pos.y = base.transform.position.y;
			base.transform.LookAt(pos);
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			hasTarget = true;
			Go();
		}

		public void DropGrenade()
		{
			base.gameObject.GetComponent<Rigidbody>().useGravity = true;
			base.transform.parent = null;
			StartCoroutine(StartDetonationTimer());
		}
	}
}
