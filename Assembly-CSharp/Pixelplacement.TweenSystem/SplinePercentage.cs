using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class SplinePercentage : TweenBase
	{
		private Transform _target;

		private Spline _spline;

		private float _startPercentage;

		private bool _faceDirection;

		public float EndValue
		{
			get;
			private set;
		}

		public SplinePercentage(Spline spline, Transform target, float startPercentage, float endPercentage, bool faceDirection, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			if (!spline.loop)
			{
				startPercentage = Mathf.Clamp01(startPercentage);
				endPercentage = Mathf.Clamp01(endPercentage);
			}
			SetEssentials(Tween.TweenType.Spline, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_spline = spline;
			_target = target;
			EndValue = endPercentage;
			_startPercentage = startPercentage;
			_faceDirection = faceDirection;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			return true;
		}

		protected override void Operation(float percentage)
		{
			float percentage2 = TweenUtilities.LinearInterpolate(_startPercentage, EndValue, percentage);
			_target.position = _spline.GetPosition(percentage2);
			if (_faceDirection)
			{
				if (_spline.direction == SplineDirection.Forward)
				{
					_target.LookAt(_target.position + _spline.GetDirection(percentage2));
				}
				else
				{
					_target.LookAt(_target.position - _spline.GetDirection(percentage2));
				}
			}
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.position = _spline.GetPosition(_startPercentage);
		}

		public override void PingPong()
		{
			ResetStartTime();
			float endValue = EndValue;
			EndValue = _startPercentage;
			_startPercentage = endValue;
		}
	}
}
