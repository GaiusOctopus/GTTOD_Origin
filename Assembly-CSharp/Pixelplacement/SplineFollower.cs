using System;
using UnityEngine;

namespace Pixelplacement
{
	[Serializable]
	public class SplineFollower
	{
		public Transform target;

		public float percentage = -1f;

		public bool faceDirection;

		private float _previousPercentage;

		private bool _previousFaceDirection;

		private bool _detached;

		public bool WasMoved
		{
			get
			{
				if (percentage != _previousPercentage || faceDirection != _previousFaceDirection)
				{
					_previousPercentage = percentage;
					_previousFaceDirection = faceDirection;
					return true;
				}
				return false;
			}
		}

		public void UpdateOrientation(Spline spline)
		{
			if (target == null)
			{
				return;
			}
			if (!spline.loop)
			{
				percentage = Mathf.Clamp01(percentage);
			}
			if (faceDirection)
			{
				if (spline.direction == SplineDirection.Forward)
				{
					target.LookAt(target.position + spline.GetDirection(percentage));
				}
				else
				{
					target.LookAt(target.position - spline.GetDirection(percentage));
				}
			}
			target.position = spline.GetPosition(percentage);
		}
	}
}
