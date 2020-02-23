using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueRect : TweenBase
	{
		private Action<Rect> _valueUpdatedCallback;

		private Rect _start;

		public Rect EndValue
		{
			get;
			private set;
		}

		public ValueRect(Rect startValue, Rect endValue, Action<Rect> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			Rect obj = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback(obj);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			Rect start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
