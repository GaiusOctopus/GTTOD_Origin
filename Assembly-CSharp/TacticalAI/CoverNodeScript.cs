using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class CoverNodeScript : MonoBehaviour
	{
		public Vector3 SightNodeOffSet = new Vector3(0f, 1f, 0f);

		private Vector3 myPosition;

		private Vector3 sightNodePosition;

		public float nodeRadiusVisualization = 0.1f;

		public bool alwaysDisplay = true;

		public LayerMask layerMask;

		public bool isActive = true;

		private bool occupied;

		public bool isAdvancedCover = true;

		public float coverNodeGroup;

		public float angleForAdvancedCover = 100f;

		public Vector3 advancedCoverDirection = Vector3.forward;

		public bool shouldAutoChooseDirection = true;

		private float closestDistSoFar;

		[HideInInspector]
		public int faceDir;

		private void Start()
		{
			SetPositions();
			AutoSetCoverDirection();
			FindFaceDir();
		}

		private void SetPositions()
		{
			myPosition = base.transform.position;
			sightNodePosition = base.transform.position;
			sightNodePosition += base.transform.forward * SightNodeOffSet.x;
			sightNodePosition += base.transform.up * SightNodeOffSet.y;
			sightNodePosition += base.transform.right * SightNodeOffSet.z;
		}

		public bool ValidCoverCheck(Vector3 targetPos)
		{
			if (isActive && Physics.Linecast(myPosition, targetPos, layerMask) && !Physics.Linecast(sightNodePosition, targetPos, layerMask))
			{
				return true;
			}
			return false;
		}

		public bool CheckForSafety(Vector3 targetPos)
		{
			if ((!isAdvancedCover || Vector3.Angle(advancedCoverDirection, sightNodePosition - targetPos) > angleForAdvancedCover / 2f) && Physics.Linecast(myPosition, targetPos, layerMask))
			{
				return true;
			}
			return false;
		}

		private void OnDrawGizmosSelected()
		{
			if (!alwaysDisplay)
			{
				SetPositions();
				if (occupied)
				{
					Gizmos.color = Color.yellow;
				}
				else if (isActive)
				{
					Gizmos.color = Color.green;
				}
				else
				{
					Gizmos.color = Color.red;
				}
				Gizmos.DrawSphere(myPosition, nodeRadiusVisualization);
				Gizmos.DrawWireSphere(sightNodePosition, nodeRadiusVisualization * 2f);
				if (isAdvancedCover)
				{
					Gizmos.DrawRay(sightNodePosition, advancedCoverDirection * 1f);
					Vector3 a = Quaternion.AngleAxis(angleForAdvancedCover / 2f, Vector3.up) * advancedCoverDirection;
					Gizmos.DrawRay(sightNodePosition, a * 1f);
					a = Quaternion.AngleAxis((0f - angleForAdvancedCover) / 2f, Vector3.up) * advancedCoverDirection;
					Gizmos.DrawRay(sightNodePosition, a * 1f);
				}
			}
		}

		private void OnDrawGizmos()
		{
			if (alwaysDisplay)
			{
				SetPositions();
				if (occupied)
				{
					Gizmos.color = Color.yellow;
				}
				else if (isActive)
				{
					Gizmos.color = Color.green;
				}
				else
				{
					Gizmos.color = Color.red;
				}
				Gizmos.DrawSphere(myPosition, nodeRadiusVisualization);
				Gizmos.DrawWireSphere(sightNodePosition, nodeRadiusVisualization * 2f);
				if (isAdvancedCover)
				{
					Gizmos.DrawRay(sightNodePosition, advancedCoverDirection * 1f);
					Vector3 a = Quaternion.AngleAxis(angleForAdvancedCover / 2f, Vector3.up) * advancedCoverDirection;
					Gizmos.DrawRay(sightNodePosition, a * 1f);
					a = Quaternion.AngleAxis((0f - angleForAdvancedCover) / 2f, Vector3.up) * advancedCoverDirection;
					Gizmos.DrawRay(sightNodePosition, a * 1f);
				}
			}
		}

		public Vector3 GetSightNodePosition()
		{
			return sightNodePosition;
		}

		public Vector3 GetPosition()
		{
			return myPosition;
		}

		public void ActivateNode(float t)
		{
			StartCoroutine(EnableThisNode(t));
		}

		private IEnumerator EnableThisNode(float t)
		{
			yield return new WaitForSeconds(t);
			isActive = true;
		}

		public void DeActivateNode()
		{
			isActive = false;
		}

		public bool isOccupied()
		{
			return occupied;
		}

		public void setOccupied(bool b)
		{
			occupied = b;
		}

		public void FindFaceDir()
		{
			Vector3 lhs = sightNodePosition - myPosition;
			lhs.y = 0f;
			float f = Vector3.Dot(lhs, Quaternion.AngleAxis(90f, Vector3.up) * advancedCoverDirection);
			faceDir = Mathf.RoundToInt(f);
		}

		public void AutoSetCoverDirection()
		{
			if (shouldAutoChooseDirection)
			{
				Vector3 forward = Vector3.forward;
				myPosition = base.transform.position;
				closestDistSoFar = 999999f;
				forward = CompareDists(forward, base.transform.right);
				forward = CompareDists(forward, -base.transform.right);
				forward = CompareDists(forward, -base.transform.forward);
				forward = (advancedCoverDirection = CompareDists(forward, base.transform.forward));
			}
		}

		public Vector3 CompareDists(Vector3 oldDir, Vector3 dirNow)
		{
			if (Physics.Raycast(myPosition, dirNow, out RaycastHit hitInfo, (int)layerMask))
			{
				Debug.DrawLine(myPosition, hitInfo.point, Color.red);
				float num = Vector3.SqrMagnitude(hitInfo.point - myPosition);
				if (num < closestDistSoFar)
				{
					closestDistSoFar = num;
					return dirNow;
				}
			}
			return oldDir;
		}
	}
}
