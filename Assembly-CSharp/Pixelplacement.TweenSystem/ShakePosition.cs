using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	internal class ShakePosition : TweenBase
	{
		private Transform _target;

		private Vector3 _initialPosition;

		private Vector3 _intensity;

		public Vector3 EndValue
		{
			get;
			private set;
		}

		public ShakePosition(Transform target, Vector3 initialPosition, Vector3 intensity, float duration, float delay, AnimationCurve curve, Action startCallback, Action completeCallback, Tween.LoopType loop, bool obeyTimescale)
		{
			SetEssentials(Tween.TweenType.Position, target.GetInstanceID(), duration, delay, obeyTimescale, curve, loop, startCallback, completeCallback);
			_target = target;
			_initialPosition = initialPosition;
			_intensity = intensity;
		}

		protected override bool SetStartValue()
		{
			if (_target == null)
			{
				return false;
			}
			return true;
		}

		protected override void Operation(float percentage)
		{
			if (percentage == 0f)
			{
				_target.localPosition = _initialPosition;
			}
			percentage = 1f - percentage;
			Vector3 b = _intensity * percentage;
			b.x = UnityEngine.Random.Range(0f - b.x, b.x);
			b.y = UnityEngine.Random.Range(0f - b.y, b.y);
			b.z = UnityEngine.Random.Range(0f - b.z, b.z);
			_target.localPosition = _initialPosition + b;
		}

		public override void Loop()
		{
			ResetStartTime();
			_target.localPosition = _initialPosition;
		}

		public override void PingPong()
		{
		}
	}
}
