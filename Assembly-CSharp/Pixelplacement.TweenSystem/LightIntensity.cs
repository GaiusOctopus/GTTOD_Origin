using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class LightIntensity : TweenBase
	{
		private Light _target;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public LightIntensity(Light target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.LightIntensity, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.intensity;
			return true;
		}

		protected override void Operation(float percentage)
		{
			float intensity = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.intensity = intensity;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.intensity = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.intensity = EndValue;
			EndValue = _start;
			_start = _target.intensity;
		}
	}
}
