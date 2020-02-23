using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueColor : TweenBase
	{
		private Action<Color> _valueUpdatedCallback;

		private Color _start;

		public Color EndValue
		{
			get;
			private set;
		}

		public ValueColor(Color startValue, Color endValue, Action<Color> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			Color obj = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback(obj);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			Color start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
