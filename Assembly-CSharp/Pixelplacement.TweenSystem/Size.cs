using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class Size : TweenBase
	{
		private RectTransform _target;

		private Vector2 _start;

		public Vector2 EndValue
		{
			get;
			private set;
		}

		public Size(RectTransform target, Vector2 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.Size, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.sizeDelta;
			return true;
		}

		protected override void Operation(float percentage)
		{
			Vector2 sizeDelta = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.sizeDelta = sizeDelta;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.sizeDelta = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.sizeDelta = EndValue;
			EndValue = _start;
			_start = _target.sizeDelta;
		}
	}
}
