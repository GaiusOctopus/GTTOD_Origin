using System;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	public abstract class TweenBase
	{
		public int targetInstanceID;

		public Tween.TweenType tweenType;

		protected float _startTime;

		private float _timeVested;

		public Tween.TweenStatus Status
		{
			get;
			private set;
		}

		public float Duration
		{
			get;
			private set;
		}

		public AnimationCurve Curve
		{
			get;
			private set;
		}

		public Keyframe[] CurveKeys
		{
			get;
			private set;
		}

		public bool ObeyTimescale
		{
			get;
			private set;
		}

		public Action StartCallback
		{
			get;
			private set;
		}

		public Action CompleteCallback
		{
			get;
			private set;
		}

		public float Delay
		{
			get;
			private set;
		}

		public Tween.LoopType LoopType
		{
			get;
			private set;
		}

		public float Percentage
		{
			get;
			private set;
		}

		public void Stop()
		{
			Status = Tween.TweenStatus.Stopped;
			if (ObeyTimescale)
			{
				_timeVested = Time.time - _startTime;
			}
			else
			{
				_timeVested = Time.realtimeSinceStartup - _startTime;
			}
		}

		public void Start()
		{
			if (ObeyTimescale)
			{
				_startTime = Time.time;
			}
			else
			{
				_startTime = Time.realtimeSinceStartup;
			}
			if (Status == Tween.TweenStatus.Canceled || Status == Tween.TweenStatus.Finished || Status == Tween.TweenStatus.Stopped)
			{
				Status = Tween.TweenStatus.Running;
				Operation(0f);
				Tween.Instance.ExecuteTween(this);
			}
		}

		public void Resume()
		{
			if (Status == Tween.TweenStatus.Stopped)
			{
				if (ObeyTimescale)
				{
					_startTime = Time.time - _timeVested;
				}
				else
				{
					_startTime = Time.realtimeSinceStartup - _timeVested;
				}
				if (Status == Tween.TweenStatus.Stopped)
				{
					Status = Tween.TweenStatus.Running;
					Tween.Instance.ExecuteTween(this);
				}
			}
		}

		public void Rewind()
		{
			Cancel();
			Operation(0f);
		}

		public void Cancel()
		{
			Status = Tween.TweenStatus.Canceled;
		}

		public void Finish()
		{
			Status = Tween.TweenStatus.Finished;
		}

		public bool Tick()
		{
			if (Status == Tween.TweenStatus.Stopped)
			{
				return false;
			}
			if (Status == Tween.TweenStatus.Canceled)
			{
				Operation(0f);
				Percentage = 0f;
				return false;
			}
			if (Status == Tween.TweenStatus.Finished)
			{
				Operation(1f);
				Percentage = 1f;
				if (CompleteCallback != null)
				{
					CompleteCallback();
				}
				return false;
			}
			float num = 0f;
			num = ((!ObeyTimescale) ? Mathf.Max(Time.realtimeSinceStartup - _startTime, 0f) : Mathf.Max(Time.time - _startTime, 0f));
			float num2 = Mathf.Min(num / Duration, 1f);
			if (num2 == 0f && Status != 0)
			{
				Status = Tween.TweenStatus.Delayed;
			}
			if (num2 > 0f && Status == Tween.TweenStatus.Delayed)
			{
				Tween.Stop(targetInstanceID, tweenType);
				if (!SetStartValue())
				{
					return false;
				}
				if (StartCallback != null)
				{
					StartCallback();
				}
				Status = Tween.TweenStatus.Running;
			}
			float percentage = num2;
			if (Curve != null && CurveKeys.Length != 0)
			{
				percentage = TweenUtilities.EvaluateCurve(Curve, num2);
			}
			if (Status == Tween.TweenStatus.Running)
			{
				try
				{
					Operation(percentage);
					Percentage = percentage;
				}
				catch (Exception)
				{
					return false;
				}
			}
			if (num2 == 1f)
			{
				if (CompleteCallback != null)
				{
					CompleteCallback();
				}
				switch (LoopType)
				{
				case Tween.LoopType.Loop:
					Loop();
					break;
				case Tween.LoopType.PingPong:
					PingPong();
					break;
				default:
					Status = Tween.TweenStatus.Finished;
					return false;
				}
			}
			return true;
		}

		protected void ResetStartTime()
		{
			if (ObeyTimescale)
			{
				_startTime = Time.time + Delay;
			}
			else
			{
				_startTime = Time.realtimeSinceStartup + Delay;
			}
		}

		protected void SetEssentials(Tween.TweenType tweenType, int targetInstanceID, float duration, float delay, bool obeyTimeScale, AnimationCurve curve, Tween.LoopType loop, Action startCallback, Action completeCallback)
		{
			this.tweenType = tweenType;
			this.targetInstanceID = targetInstanceID;
			if (delay > 0f)
			{
				Status = Tween.TweenStatus.Delayed;
			}
			Duration = duration;
			Delay = delay;
			Curve = curve;
			CurveKeys = curve?.keys;
			StartCallback = startCallback;
			CompleteCallback = completeCallback;
			LoopType = loop;
			ObeyTimescale = obeyTimeScale;
			ResetStartTime();
		}

		protected abstract bool SetStartValue();

		protected abstract void Operation(float percentage);

		public abstract void Loop();

		public abstract void PingPong();
	}
}
