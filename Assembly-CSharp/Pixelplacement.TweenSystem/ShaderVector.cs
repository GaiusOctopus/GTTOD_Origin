using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ShaderVector : TweenBase
	{
		private Material _target;

		private Vector4 _start;

		private string _propertyName;

		public Vector4 EndValue
		{
			get;
			private set;
		}

		public ShaderVector(Material target, string propertyName, Vector4 endValue, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.ShaderVector, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			_propertyName = propertyName;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.GetVector(_propertyName);
			return true;
		}

		protected override void Operation(float percentage)
		{
			Vector4 value = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_target.SetVector(_propertyName, value);
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.SetVector(_propertyName, _start);
		}

		public override void PingPong()
		{
			ResetStartTime();
			_target.SetVector(_propertyName, EndValue);
			EndValue = _start;
			_start = _target.GetVector(_propertyName);
		}
	}
}
