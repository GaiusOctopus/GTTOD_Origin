using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueVector2 : TweenBase
	{
		private Action<Vector2> _valueUpdatedCallback;

		private Vector2 _start;

		public Vector2 EndValue
		{
			get;
			private set;
		}

		public ValueVector2(Vector2 startValue, Vector2 endValue, Action<Vector2> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			Vector2 obj = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback(obj);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			Vector2 start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
