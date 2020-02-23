using System.Collections.Generic;
using UnityEngine;

namespace TacticalAI
{
	public class CoverFinderScript : MonoBehaviour
	{
		public enum CoverSeekMethods
		{
			RandomCover,
			WithinCombatRange,
			AdvanceTowardsTarget
		}

		public enum DynamicCoverMethods
		{
			NavmeshScan,
			Raycasts
		}

		public CoverSeekMethods currentCoverSeekMethod = CoverSeekMethods.WithinCombatRange;

		private Vector3 lastCoverPos;

		public float minDistBetweenLastCover = 20f;

		private float minDistBetweenLastCoverSquared = 10f;

		public float minCoverDistFromEnemy = 10f;

		public float maxCoverDistFromEnemy = 50f;

		private float maxCoverDistSqrd;

		private float minCoverDistSqrd;

		public float maxDistToCover = 9999f;

		public float minDistToAdvance = 5f;

		private CoverNodeScript[] coverNodeScripts;

		public DynamicCoverMethods dynamicCoverSelectionMode = DynamicCoverMethods.Raycasts;

		public bool shouldUseDynamicCover = true;

		public bool useFirstDynamicCoverFound = true;

		public float dynamicCoverMaxDistFromMe = 15f;

		public float dynamicCoverNodeHeightOffset = 0.3f;

		public float dynamicCoverNodeFireOffset = 1.5f;

		public float dynamicCoverWidthNeededToHide = 1.5f;

		public float maxDistBehindDynamicCover = 5f;

		public bool useOnlyStaticCover = true;

		public float defendingDist = 20f;

		private float defendingDistSquared = 20f;

		private Transform myTransform;

		[HideInInspector]
		public LayerMask layerMask;

		private Vector3[] verts;

		private NavmeshInterface navI;

		public int coverNodeGroup;

		public int angleBetweenCasts = 10;

		public float maxRayDist = 1000f;

		public float rayCastHeightOffGround = 0.1f;

		public float distFromWallToBe = 1f;

		private void Start()
		{
			navI = base.gameObject.GetComponent<BaseScript>().navI;
			if (useOnlyStaticCover && shouldUseDynamicCover)
			{
				verts = navI.GetNavmeshVertices();
			}
			myTransform = base.transform;
			maxCoverDistSqrd = maxCoverDistFromEnemy * maxCoverDistFromEnemy;
			minCoverDistSqrd = minCoverDistFromEnemy * minCoverDistFromEnemy;
			layerMask = ControllerScript.currentController.GetLayerMask();
			defendingDistSquared = defendingDist * defendingDist;
			minDistToAdvance *= minDistToAdvance;
			if (ControllerScript.currentController != null)
			{
				coverNodeScripts = ControllerScript.currentController.GetCovers();
			}
			else
			{
				Debug.LogWarning("No Controller has been detected!  An AIControllerScript is required for the AI to work!  Please create a new gameObject and attach the Paragon AI ControllerScript to it!");
			}
		}

		public void ResetLastCoverPos()
		{
			lastCoverPos = new Vector3(100000f, 100000f, 100000f);
		}

		public CoverData FindCover(Transform targetTransform)
		{
			return FindStaticCover(targetTransform, null);
		}

		public CoverData FindCover(Transform targetTransform, Transform transformToDefend)
		{
			return FindStaticCover(targetTransform, transformToDefend);
		}

		private CoverData FindStaticCover(Transform targetTransform, Transform transformToDefend)
		{
			if ((bool)targetTransform && (bool)myTransform)
			{
				Vector3 position = targetTransform.position;
				if (currentCoverSeekMethod == CoverSeekMethods.WithinCombatRange)
				{
					return FindCoverWithinCombatRange(position, transformToDefend);
				}
				if (currentCoverSeekMethod == CoverSeekMethods.AdvanceTowardsTarget)
				{
					return FindCoverWithinCombatRange(position, transformToDefend);
				}
				return FindRandomCover(position, transformToDefend);
			}
			return new CoverData();
		}

		private CoverData FindCoverWithinCombatRange(Vector3 targetTransformPos, Transform transformToDefend)
		{
			int num = 0;
			Vector3 position = myTransform.position;
			CoverNodeScript coverNodeScript = null;
			float num2 = maxDistToCover;
			bool flag = false;
			for (num = 0; num < coverNodeScripts.Length; num++)
			{
				if (coverNodeScripts[num].isOccupied() || (float)coverNodeGroup != coverNodeScripts[num].coverNodeGroup || !(Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - lastCoverPos) > minDistBetweenLastCoverSquared) || ((bool)transformToDefend && !(Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - transformToDefend.position) < defendingDistSquared)))
				{
					continue;
				}
				float num3 = Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - targetTransformPos);
				float num4 = Vector3.SqrMagnitude(position - coverNodeScripts[num].GetPosition());
				if (!coverNodeScripts[num].ValidCoverCheck(targetTransformPos))
				{
					continue;
				}
				if (minCoverDistSqrd < num3 && maxCoverDistSqrd > num3)
				{
					if (!flag || num4 < num2)
					{
						num2 = num4;
						coverNodeScript = coverNodeScripts[num];
						flag = true;
					}
				}
				else if (!flag && num4 < num2)
				{
					num2 = num4;
					coverNodeScript = coverNodeScripts[num];
				}
			}
			if (coverNodeScript != null)
			{
				lastCoverPos = coverNodeScript.GetPosition();
				return new CoverData(f: true, coverNodeScript.GetPosition(), coverNodeScript.GetSightNodePosition(), d: false, coverNodeScript);
			}
			if (shouldUseDynamicCover && !ControllerScript.pMode)
			{
				return FindDynamicCover(targetTransformPos, transformToDefend);
			}
			return new CoverData();
		}

		private CoverData FindAdvancingCover(Vector3 targetTransformPos, Transform transformToDefend)
		{
			int num = 0;
			Vector3 position = myTransform.position;
			CoverNodeScript coverNodeScript = null;
			Vector3 b = (!transformToDefend) ? targetTransformPos : transformToDefend.position;
			float num2 = Vector3.SqrMagnitude(position - b) - minDistToAdvance;
			float num3 = num2;
			for (num = 0; num < coverNodeScripts.Length; num++)
			{
				if (!coverNodeScripts[num].isOccupied())
				{
					float num4 = Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - b);
					if (num4 < num2 && Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - position) < num3 && coverNodeScripts[num].ValidCoverCheck(targetTransformPos))
					{
						num3 = num4;
						coverNodeScript = coverNodeScripts[num];
					}
				}
			}
			if (coverNodeScript != null)
			{
				lastCoverPos = coverNodeScript.GetPosition();
				return new CoverData(f: true, coverNodeScript.GetPosition(), coverNodeScript.GetSightNodePosition(), d: false, coverNodeScript);
			}
			return new CoverData();
		}

		private CoverData FindRandomCover(Vector3 targetTransformPos, Transform transformToDefend)
		{
			int num = 0;
			CoverNodeScript coverNodeScript = null;
			List<CoverNodeScript> list = new List<CoverNodeScript>();
			for (num = 0; num < coverNodeScripts.Length; num++)
			{
				if (!coverNodeScripts[num].isOccupied() && coverNodeScripts[num].ValidCoverCheck(targetTransformPos) && (!transformToDefend || Vector3.SqrMagnitude(coverNodeScripts[num].GetPosition() - transformToDefend.position) < defendingDistSquared))
				{
					list.Add(coverNodeScripts[num]);
				}
			}
			if (list.Count > 0)
			{
				coverNodeScript = list[Random.Range(0, list.Count)];
				lastCoverPos = coverNodeScript.GetPosition();
				return new CoverData(f: true, coverNodeScript.GetPosition(), coverNodeScript.GetSightNodePosition(), d: false, coverNodeScript);
			}
			if (shouldUseDynamicCover && !ControllerScript.pMode)
			{
				return FindDynamicCover(targetTransformPos, transformToDefend);
			}
			return new CoverData();
		}

		private CoverData FindDynamicCover(Vector3 targetTransformPos, Transform transformToDefend)
		{
			if (dynamicCoverSelectionMode == DynamicCoverMethods.Raycasts)
			{
				return FindRaycastDynamicCover(targetTransformPos, transformToDefend);
			}
			if (!useOnlyStaticCover)
			{
				verts = navI.GetNavmeshVertices();
			}
			Vector3 b = transformToDefend ? transformToDefend.position : myTransform.position;
			b.y += dynamicCoverNodeHeightOffset;
			float num = dynamicCoverNodeFireOffset - dynamicCoverNodeHeightOffset;
			int num2 = 0;
			Vector3 vector = Vector3.zero;
			Vector3 fP = Vector3.zero;
			float num3 = dynamicCoverMaxDistFromMe * dynamicCoverMaxDistFromMe;
			bool flag = true;
			for (int i = 0; i < verts.Length; i++)
			{
				if (!((double)Random.value > 0.5) || !(Vector3.SqrMagnitude(verts[i] - b) > minDistBetweenLastCover))
				{
					continue;
				}
				float num4 = Vector3.SqrMagnitude(verts[i] - targetTransformPos);
				float num5 = Vector3.SqrMagnitude(verts[i] - b);
				if (!(num5 < num3) || !(num4 > minCoverDistSqrd) || !(num4 < maxCoverDistSqrd))
				{
					continue;
				}
				verts[i].y += dynamicCoverNodeFireOffset;
				if (Physics.Linecast(verts[i], targetTransformPos, layerMask))
				{
					continue;
				}
				verts[i].y -= num;
				if (!Physics.Raycast(verts[i], targetTransformPos - verts[i], maxDistBehindDynamicCover, layerMask) || ControllerScript.currentController.isDynamicCoverSpotCurrentlyUsed(verts[i]))
				{
					continue;
				}
				flag = true;
				Vector3 vector2 = verts[i];
				if (!Physics.Linecast(targetTransformPos, verts[i], layerMask) && Physics.Linecast(vector2, targetTransformPos, layerMask))
				{
					num3 = num5;
					vector = vector2;
					fP = verts[i];
					flag = false;
					if (useFirstDynamicCoverFound)
					{
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				for (num2 = -1; num2 <= 1; num2 += 2)
				{
					vector2 = verts[i] + myTransform.right * num2 * dynamicCoverWidthNeededToHide;
					if (!Physics.Linecast(vector2, verts[i], layerMask) && Physics.Linecast(vector2, targetTransformPos, layerMask))
					{
						num3 = num5;
						vector = vector2;
						fP = verts[i];
						flag = false;
						if (useFirstDynamicCoverFound)
						{
							break;
						}
					}
				}
			}
			if (vector != Vector3.zero)
			{
				lastCoverPos = vector;
				return new CoverData(f: true, vector, fP, d: true, null);
			}
			return new CoverData();
		}

		private CoverData FindRaycastDynamicCover(Vector3 targetTransformPos, Transform transformToDefend)
		{
			if (!transformToDefend)
			{
				transformToDefend = myTransform;
			}
			int num = 0;
			Vector3 origin = transformToDefend.position;
			Vector3 point = Vector3.Normalize(targetTransformPos - transformToDefend.position);
			point = Quaternion.Euler(0f, -90f, 0f) * point;
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(transformToDefend.position, Vector3.down, out hitInfo, maxRayDist, layerMask))
			{
				origin = hitInfo.point;
			}
			origin.y += rayCastHeightOffGround;
			for (num = 0; (float)num < 360f; num += angleBetweenCasts)
			{
				if (Physics.Raycast(origin, point, out hitInfo, maxRayDist, layerMask) && !ControllerScript.currentController.isDynamicCoverSpotCurrentlyUsed(hitInfo.point))
				{
					Vector3 point2 = hitInfo.point;
					point2 -= point * distFromWallToBe;
					if (Physics.Linecast(point2, targetTransformPos, layerMask))
					{
						Vector3 vector = point2 + new Vector3(0f, dynamicCoverNodeFireOffset, 0f);
						if (!Physics.Linecast(vector, targetTransformPos, layerMask))
						{
							lastCoverPos = point2;
							return new CoverData(f: true, point2, vector, d: true, null);
						}
					}
				}
				point = Quaternion.Euler(0f, angleBetweenCasts, 0f) * point;
			}
			return new CoverData();
		}
	}
}
