using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class Rotate : TweenBase
	{
		private Transform _target;

		private Vector3 _start;

		private Space _space;

		private Vector3 _previous;

		public Vector3 EndValue
		{
			get;
			private set;
		}

		public Rotate(Transform target, Vector3 endValue, Space space, float duration, float delay, bool obeyTimescale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			SetEssentials(Tween.TweenType.Rotation, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			EndValue = endValue;
			_space = space;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			_start = _target.localEulerAngles;
			return true;
		}

		protected override void Operation(float percentage)
		{
			if (percentage == 0f)
			{
				_target.localEulerAngles = _start;
			}
			Vector3 vector = TweenUtilities.LinearInterpolate(Vector3.zero, EndValue, percentage) - _previous;
			_previous += vector;
			_target.Rotate(vector, _space);
		}

		public override void Loop()
		{
			_previous = Vector3.zero;
			ResetStartTime();
		}

		public override void PingPong()
		{
			_previous = Vector3.zero;
			EndValue *= -1f;
			ResetStartTime();
		}
	}
}
