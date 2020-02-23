using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ValueVector3 : TweenBase
	{
		private Action<Vector3> _valueUpdatedCallback;

		private Vector3 _start;

		public Vector3 EndValue
		{
			get;
			private set;
		}

		public ValueVector3(Vector3 startValue, Vector3 endValue, Action<Vector3> valueUpdatedCallback, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
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
			Vector3 obj = TweenUtilities.LinearInterpolate(_start, EndValue, percentage);
			_valueUpdatedCallback(obj);
		}

		public override void Loop()
		{
			ResetStartTime();
		}

		public override void PingPong()
		{
			ResetStartTime();
			Vector3 start = _start;
			_start = EndValue;
			EndValue = start;
		}
	}
}
