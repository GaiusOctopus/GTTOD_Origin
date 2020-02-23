using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class LocalScale : TweenBase
	{
		private Transform _target;

		private Vector3 _start;

		public Vector3 EndValue
		{
			get;
			private set;
		}

		public LocalScale(Transform target, Vector3 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.LocalScale, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.localScale;
			return true;
		}

		protected override void Operation(float percentage)
		{
			Vector3 localScale = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.localScale = localScale;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.localScale = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.localScale = EndValue;
			EndValue = _start;
			_start = _target.localScale;
		}
	}
}
