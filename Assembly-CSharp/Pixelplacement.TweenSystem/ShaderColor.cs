using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ShaderColor : TweenBase
	{
		private Material _target;

		private Color _start;

		private string _propertyName;

		public Color EndValue
		{
			get;
			private set;
		}

		public ShaderColor(Material target, string propertyName, Color endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.ShaderColor, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			_propertyName = propertyName;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			_start = _target.GetColor(_propertyName);
			if (_target == null)
			{
				return false;
			}
			return true;
		}

		protected override void Operation(float percentage)
		{
			Color value = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.SetColor(_propertyName, value);
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.SetColor(_propertyName, _start);
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.SetColor(_propertyName, EndValue);
			EndValue = _start;
			_start = _target.GetColor(_propertyName);
		}
	}
}
