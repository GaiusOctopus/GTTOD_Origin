using System.Collections;
using UnityEngine;

namespace TacticalAI
{
	public class Cover : CustomAIBehaviour
	{
		private float maxTimeInCover = 10f;

		private float minTimeInCover = 5f;

		private bool foundDynamicCover;

		private float minDistToTargetIfNotInCover = 5f;

		private float maxTimeTilNoCoverCharge = 3f;

		private float timeTilNoCoverCharge;

		public override void Initiate()
		{
			base.Initiate();
			maxTimeInCover = baseScript.maxTimeInCover;
			minTimeInCover = baseScript.minTimeInCover;
			minDistToTargetIfNotInCover = baseScript.minDistToTargetIfNotInCover * baseScript.minDistToTargetIfNotInCover;
			timeTilNoCoverCharge = maxTimeTilNoCoverCharge;
		}

		public override void OnEndBehaviour()
		{
			LeaveCover();
		}

		private void Update()
		{
			if (!baseScript.currentCoverNodeScript && !foundDynamicCover)
			{
				timeTilNoCoverCharge -= Time.deltaTime;
			}
		}

		public override void AICycle()
		{
			if ((bool)coverFinderScript)
			{
				if (baseScript.useAdvancedCover || !gunScript.IsFiring() || !baseScript.shouldFireFromCover)
				{
					targetVector = baseScript.currentCoverNodePos;
				}
				else
				{
					targetVector = baseScript.currentCoverNodeFiringPos;
				}
				if ((bool)baseScript.currentCoverNodeScript || foundDynamicCover)
				{
					if (navI.PathPartial())
					{
						LeaveCover();
					}
					if (!baseScript.inCover && navI.ReachedDestination())
					{
						baseScript.inCover = true;
						StartCoroutine(SetTimeToLeaveCover(Random.Range(minTimeInCover, maxTimeInCover)));
					}
					return;
				}
				CoverData coverData = coverFinderScript.FindCover(baseScript.targetTransform, baseScript.keyTransform);
				if (coverData.foundCover)
				{
					SetCover(coverData.hidingPosition, coverData.firingPosition, coverData.isDynamicCover, coverData.coverNodeScript);
					if ((bool)soundScript)
					{
						soundScript.PlayCoverAudio();
					}
				}
				else if ((bool)baseScript.targetTransform && timeTilNoCoverCharge < 0f)
				{
					NoCoverFindDest();
				}
			}
			else if ((bool)baseScript.targetTransform && timeTilNoCoverCharge < 0f)
			{
				NoCoverFindDest();
			}
		}

		private void NoCoverFindDest()
		{
			if (Vector3.SqrMagnitude(myTransform.position - baseScript.targetTransform.position) > minDistToTargetIfNotInCover || Physics.Linecast(baseScript.GetEyePos(), baseScript.targetTransform.position, layerMask))
			{
				targetVector = baseScript.targetTransform.position;
			}
			else
			{
				targetVector = myTransform.position;
			}
		}

		private IEnumerator SetTimeToLeaveCover(float timeToLeave)
		{
			while (timeToLeave > 0f && ((bool)baseScript.currentCoverNodeScript || foundDynamicCover))
			{
				timeToLeave = ((!baseScript.inCover) ? (timeToLeave - 0.25f) : (timeToLeave - 1f));
				if ((bool)baseScript.targetTransform)
				{
					if (!foundDynamicCover && !baseScript.currentCoverNodeScript.CheckForSafety(baseScript.targetTransform.position))
					{
						LeaveCover();
					}
					else if (foundDynamicCover && !Physics.Linecast(baseScript.currentCoverNodePos, baseScript.targetTransform.position, layerMask.value))
					{
						LeaveCover();
					}
				}
				yield return new WaitForSeconds(1f);
			}
			if ((bool)baseScript.currentCoverNodeScript || foundDynamicCover)
			{
				LeaveCover();
			}
		}

		public void LeaveCover()
		{
			if ((bool)baseScript.currentCoverNodeScript)
			{
				baseScript.currentCoverNodeScript.setOccupied(b: false);
				baseScript.currentCoverNodeScript = null;
			}
			else if (foundDynamicCover)
			{
				ControllerScript.currentController.RemoveACoverSpot(baseScript.currentCoverNodeFiringPos);
			}
			baseScript.inCover = false;
			baseScript.SetOrigStoppingDistance();
			foundDynamicCover = false;
			if (!baseScript.shouldFireFromCover)
			{
				coverFinderScript.ResetLastCoverPos();
			}
			if (baseScript.useAdvancedCover)
			{
				animationScript.EndAdvancedCover();
			}
		}

		private void SetCover(Vector3 newCoverPos, Vector3 newCoverFiringSpot, bool isDynamicCover, CoverNodeScript newCoverNodeScript)
		{
			timeTilNoCoverCharge = maxTimeTilNoCoverCharge;
			baseScript.currentCoverNodePos = newCoverPos;
			baseScript.currentCoverNodeFiringPos = newCoverFiringSpot;
			navI.SetStoppingDistance(0f);
			if (isDynamicCover)
			{
				foundDynamicCover = true;
				ControllerScript.currentController.AddACoverSpot(baseScript.currentCoverNodeFiringPos);
				return;
			}
			baseScript.currentCoverNodeScript = newCoverNodeScript;
			baseScript.currentCoverNodeScript.setOccupied(b: true);
			if (baseScript.useAdvancedCover)
			{
				animationScript.StartAdvancedCover(baseScript.currentCoverNodeScript.advancedCoverDirection, baseScript.currentCoverNodeScript.faceDir);
			}
		}
	}
}
