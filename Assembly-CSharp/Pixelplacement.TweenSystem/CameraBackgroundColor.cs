using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class CameraBackgroundColor : TweenBase
	{
		private Camera _target;

		private Color _start;

		public Color EndValue
		{
			get;
			private set;
		}

		public CameraBackgroundColor(Camera target, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.ImageColor, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.backgroundColor;
			return true;
		}

		protected override void Operation(float percentage)
		{
			Color backgroundColor = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.backgroundColor = backgroundColor;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.backgroundColor = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.backgroundColor = EndValue;
			EndValue = _start;
			_start = _target.backgroundColor;
		}
	}
}
