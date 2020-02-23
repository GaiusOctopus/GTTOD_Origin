using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class Pitch : TweenBase
	{
		private AudioSource _target;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public Pitch(AudioSource target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.Pitch, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.pitch;
			return true;
		}

		protected override void Operation(float percentage)
		{
			float pitch = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.pitch = pitch;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.pitch = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.pitch = EndValue;
			EndValue = _start;
			_start = _target.pitch;
		}
	}
}
