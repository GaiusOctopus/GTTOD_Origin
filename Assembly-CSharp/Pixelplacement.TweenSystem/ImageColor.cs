using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelplacement.TweenSystem
{
	internal class ImageColor : TweenBase
	{
		private Image _target;

		private Color _start;

		public Color EndValue
		{
			get;
			private set;
		}

		public ImageColor(Image target, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			_start = _target.color;
			return true;
		}

		protected override void Operation(float percentage)
		{
			Color color = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.color = color;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.color = _start;
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.color = EndValue;
			EndValue = _start;
			_start = _target.color;
		}
	}
}
