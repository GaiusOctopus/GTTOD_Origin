using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class CanvasGroupAlpha : TweenBase
	{
		private CanvasGroup _target;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public CanvasGroupAlpha(CanvasGroup target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.CanvasGroupAlpha, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.alpha;
			return true;
		}

		protected override void Operation(float percentage)
		{
			float alpha = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.alpha = alpha;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.alpha = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.alpha = EndValue;
			EndValue = _start;
			_start = _target.alpha;
		}
	}
}
