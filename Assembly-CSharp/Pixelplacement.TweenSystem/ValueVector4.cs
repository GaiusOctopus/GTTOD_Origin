using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueVector4 : TweenBase
	{
		private Action<Vector4> _valueUpdatedCallback;

		private Vector4 _start;

		public Vector4 EndValue
		{
			get;
			private set;
		}

		public ValueVector4(Vector4 startValue, Vector4 endValue, Action<Vector4> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.Value, -1, duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_valueUpdatedCallback = valueUpdatedCallback;
			_start = startValue;
			EndValue = endValue;
		}

		protected override bool SetStartValue()
		{
			return true;
		}

		protected override void Operation(float percentage)
		{
			Vector4 obj = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback(obj);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			Vector4 start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
