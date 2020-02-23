using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class AnchoredPosition : TweenBase
	{
		private RectTransform _target;

		private Vector2 _start;

		public Vector2 EndValue
		{
			get;
			private set;
		}

		public AnchoredPosition(RectTransform target, Vector2 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.AnchoredPosition, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.anchoredPosition;
			return true;
		}

		protected override void Operation(float percentage)
		{
			Vector3 v = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.anchoredPosition = v;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.anchoredPosition = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.anchoredPosition = EndValue;
			EndValue = _start;
			_start = _target.anchoredPosition;
		}
	}
}
