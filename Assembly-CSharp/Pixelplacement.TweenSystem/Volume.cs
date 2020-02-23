using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class Volume : TweenBase
	{
		private AudioSource _target;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public Volume(AudioSource target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.Volume, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.volume;
			return true;
		}

		protected override void Operation(float percentage)
		{
			float volume = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.volume = volume;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.volume = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.volume = EndValue;
			EndValue = _start;
			_start = _target.volume;
		}
	}
}
