using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueInt : TweenBase
	{
		private Action<int> _valueUpdatedCallback;

		private float _start;

		public float EndValue
		{
			get;
			private set;
		}

		public ValueInt(int startValue, int endValue, Action<int> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			float num = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback((int)num);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			float start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
