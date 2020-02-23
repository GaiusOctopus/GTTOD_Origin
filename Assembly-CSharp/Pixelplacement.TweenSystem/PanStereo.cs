using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class PanStereo : TweenBase
	{
		private AudioSource _target;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public PanStereo(AudioSource target, float endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.PanStereo, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.panStereo;
			return true;
		}

		protected override void Operation(float percentage)
		{
			float panStereo = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.panStereo = panStereo;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.panStereo = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.panStereo = EndValue;
			EndValue = _start;
			_start = _target.panStereo;
		}
	}
}
