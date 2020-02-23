using Pixelplacement.TweenSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pixelplacement
{
	public class Tween
	{
		public enum TweenType
		{
			Position,
			Rotation,
			LocalScale,
			LightColor,
			LightIntensity,
			LightRange,
			FieldOfView,
			SpriteRendererColor,
			RawImageColor,
			ImageColor,
			AnchoredPosition,
			Size,
			Volume,
			Pitch,
			PanStereo,
			ShaderFloat,
			ShaderColor,
			ShaderInt,
			ShaderVector,
			Value,
			TextMeshColor,
			GUITextColor,
			TextColor,
			CanvasGroupAlpha,
			Spline
		}

		public enum LoopType
		{
			None,
			Loop,
			PingPong
		}

		public enum TweenStatus
		{
			Delayed,
			Running,
			Canceled,
			Stopped,
			Finished
		}

		public static List<TweenBase> activeTweens = new List<TweenBase>();

		private static TweenEngine _instance;

		public static TweenEngine Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GameObject("(Tween Engine)", typeof(TweenEngine)).GetComponent<TweenEngine>();
				}
				UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
				return _instance;
			}
		}

		public static AnimationCurve EaseLinear => null;

		public static AnimationCurve EaseIn => new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 1f, 0f));

		public static AnimationCurve EaseInStrong => new AnimationCurve(new Keyframe(0f, 0f, 0.03f, 0.03f), new Keyframe(0.45f, 0.03f, 0.2333333f, 0.2333333f), new Keyframe(0.7f, 0.13f, 0.7666667f, 0.7666667f), new Keyframe(0.85f, 0.3f, 2.233334f, 2.233334f), new Keyframe(0.925f, 0.55f, 4.666665f, 4.666665f), new Keyframe(1f, 1f, 5.999996f, 5.999996f));

		public static AnimationCurve EaseOut => new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 0f, 0f));

		public static AnimationCurve EaseOutStrong => new AnimationCurve(new Keyframe(0f, 0f, 13.80198f, 13.80198f), new Keyframe(0.04670785f, 0.3973127f, 5.873408f, 5.873408f), new Keyframe(0.1421811f, 0.7066917f, 2.313627f, 2.313627f), new Keyframe(0.2483539f, 0.8539293f, 0.9141542f, 0.9141542f), new Keyframe(0.4751028f, 0.954047f, 0.264541f, 0.264541f), new Keyframe(1f, 1f, 0.03f, 0.03f));

		public static AnimationCurve EaseInOut => AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public static AnimationCurve EaseInOutStrong => new AnimationCurve(new Keyframe(0f, 0f, 0.03f, 0.03f), new Keyframe(0.5f, 0.5f, 3.257158f, 3.257158f), new Keyframe(1f, 1f, 0.03f, 0.03f));

		public static AnimationCurve EaseInBack => new AnimationCurve(new Keyframe(0f, 0f, -1.1095f, -1.1095f), new Keyframe(1f, 1f, 2f, 2f));

		public static AnimationCurve EaseOutBack => new AnimationCurve(new Keyframe(0f, 0f, 2f, 2f), new Keyframe(1f, 1f, -1.1095f, -1.1095f));

		public static AnimationCurve EaseInOutBack => new AnimationCurve(new Keyframe(1f, 1f, -1.754543f, -1.754543f), new Keyframe(0f, 0f, -1.754543f, -1.754543f));

		public static AnimationCurve EaseSpring => new AnimationCurve(new Keyframe(0f, -0.0003805831f, 8.990726f, 8.990726f), new Keyframe(0.35f, 1f, -4.303913f, -4.303913f), new Keyframe(0.55f, 1f, 1.554695f, 1.554695f), new Keyframe(0.7730452f, 1f, -2.007816f, -2.007816f), new Keyframe(1f, 1f, -1.23451f, -1.23451f));

		public static AnimationCurve EaseBounce => new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.25f, 1f, 11.73749f, -5.336508f), new Keyframe(0.4f, 0.6f, -0.1904764f, -0.1904764f), new Keyframe(0.575f, 1f, 5.074602f, -3.89f), new Keyframe(0.7f, 0.75f, 0.001192093f, 0.001192093f), new Keyframe(0.825f, 1f, 4.18469f, -2.657566f), new Keyframe(0.895f, 0.9f, 0f, 0f), new Keyframe(0.95f, 1f, 3.196362f, -2.028364f), new Keyframe(1f, 1f, 2.258884f, 0.5f));

		public static AnimationCurve EaseWobble => new AnimationCurve(new Keyframe(0f, 0f, 11.01978f, 30.76278f), new Keyframe(0.08054394f, 1f, 0f, 0f), new Keyframe(0.3153235f, -0.75f, 0f, 0f), new Keyframe(0.5614113f, 0.5f, 0f, 0f), new Keyframe(0.75f, -0.25f, 0f, 0f), new Keyframe(0.9086903f, 0.1361611f, 0f, 0f), new Keyframe(1f, 0f, -4.159244f, -1.351373f));

		public static TweenBase Shake(Transform target, Vector3 initialPosition, Vector3 intensity, float duration, float delay, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShakePosition shakePosition = new ShakePosition(target, initialPosition, intensity, duration, delay, EaseLinear, startCallback, completeCallback, loop, obeyTimescale);
			SendTweenForProcessing(shakePosition, interrupt: true);
			return shakePosition;
		}

		public static TweenBase Spline(Spline spline, Transform target, float startPercentage, float endPercentage, bool faceDirection, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			SplinePercentage splinePercentage = new SplinePercentage(spline, target, startPercentage, endPercentage, faceDirection, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(splinePercentage, interrupt: true);
			return splinePercentage;
		}

		public static TweenBase CanvasGroupAlpha(CanvasGroup target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			CanvasGroupAlpha canvasGroupAlpha = new CanvasGroupAlpha(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(canvasGroupAlpha, interrupt: true);
			return canvasGroupAlpha;
		}

		public static TweenBase CanvasGroupAlpha(CanvasGroup target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.alpha = startValue;
			return CanvasGroupAlpha(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Value(Rect startValue, Rect endValue, Action<Rect> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueRect valueRect = new ValueRect(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueRect, interrupt: true);
			return valueRect;
		}

		public static TweenBase Value(Vector4 startValue, Vector4 endValue, Action<Vector4> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueVector4 valueVector = new ValueVector4(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueVector);
			return valueVector;
		}

		public static TweenBase Value(Vector3 startValue, Vector3 endValue, Action<Vector3> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueVector3 valueVector = new ValueVector3(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueVector);
			return valueVector;
		}

		public static TweenBase Value(Vector2 startValue, Vector2 endValue, Action<Vector2> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueVector2 valueVector = new ValueVector2(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueVector);
			return valueVector;
		}

		public static TweenBase Value(Color startValue, Color endValue, Action<Color> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueColor valueColor = new ValueColor(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueColor);
			return valueColor;
		}

		public static TweenBase Value(int startValue, int endValue, Action<int> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueInt valueInt = new ValueInt(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueInt);
			return valueInt;
		}

		public static TweenBase Value(float startValue, float endValue, Action<float> valueUpdatedCallback, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ValueFloat valueFloat = new ValueFloat(startValue, endValue, valueUpdatedCallback, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(valueFloat);
			return valueFloat;
		}

		public static TweenBase ShaderVector(Material target, string propertyName, Vector4 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShaderVector shaderVector = new ShaderVector(target, propertyName, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(shaderVector, interrupt: true);
			return shaderVector;
		}

		public static TweenBase ShaderVector(Material target, string propertyName, Vector4 startValue, Vector4 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.SetVector(propertyName, startValue);
			return ShaderVector(target, propertyName, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase ShaderInt(Material target, string propertyName, int endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShaderInt shaderInt = new ShaderInt(target, propertyName, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(shaderInt, interrupt: true);
			return shaderInt;
		}

		public static TweenBase ShaderInt(Material target, string propertyName, int startValue, int endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.SetInt(propertyName, startValue);
			return ShaderInt(target, propertyName, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase ShaderColor(Material target, string propertyName, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShaderColor shaderColor = new ShaderColor(target, propertyName, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(shaderColor, interrupt: true);
			return shaderColor;
		}

		public static TweenBase ShaderColor(Material target, string propertyName, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.SetColor(propertyName, startValue);
			return ShaderColor(target, propertyName, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase ShaderFloat(Material target, string propertyName, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShaderFloat shaderFloat = new ShaderFloat(target, propertyName, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(shaderFloat, interrupt: true);
			return shaderFloat;
		}

		public static TweenBase ShaderFloat(Material target, string propertyName, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.SetFloat(propertyName, startValue);
			return ShaderFloat(target, propertyName, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Pitch(AudioSource target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Pitch pitch = new Pitch(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(pitch, interrupt: true);
			return pitch;
		}

		public static TweenBase Pitch(AudioSource target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.pitch = startValue;
			return Pitch(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase PanStereo(AudioSource target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			PanStereo panStereo = new PanStereo(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(panStereo, interrupt: true);
			return panStereo;
		}

		public static TweenBase PanStereo(AudioSource target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.panStereo = startValue;
			return PanStereo(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Volume(AudioSource target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Volume volume = new Volume(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(volume, interrupt: true);
			return volume;
		}

		public static TweenBase Volume(AudioSource target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.volume = startValue;
			return Volume(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Size(RectTransform target, Vector2 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Size size = new Size(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(size, interrupt: true);
			return size;
		}

		public static TweenBase Size(RectTransform target, Vector2 startValue, Vector2 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.sizeDelta = startValue;
			return Size(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase FieldOfView(Camera target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			FieldOfView fieldOfView = new FieldOfView(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(fieldOfView, interrupt: true);
			return fieldOfView;
		}

		public static TweenBase FieldOfView(Camera target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.fieldOfView = startValue;
			return FieldOfView(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LightRange(Light target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LightRange lightRange = new LightRange(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(lightRange, interrupt: true);
			return lightRange;
		}

		public static TweenBase LightRange(Light target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.range = startValue;
			return LightRange(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LightIntensity(Light target, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LightIntensity lightIntensity = new LightIntensity(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(lightIntensity, interrupt: true);
			return lightIntensity;
		}

		public static TweenBase LightIntensity(Light target, float startValue, float endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.intensity = startValue;
			return LightIntensity(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LocalScale(Transform target, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LocalScale localScale = new LocalScale(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(localScale, interrupt: true);
			return localScale;
		}

		public static TweenBase LocalScale(Transform target, Vector3 startValue, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.localScale = startValue;
			return LocalScale(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(RawImage target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			RawImageColor rawImageColor = new RawImageColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(rawImageColor, interrupt: true);
			return rawImageColor;
		}

		public static TweenBase Color(RawImage target, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startValue;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Image target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ImageColor imageColor = new ImageColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(imageColor, interrupt: true);
			return imageColor;
		}

		public static TweenBase Color(Image target, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startValue;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Text target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			TextColor textColor = new TextColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(textColor, interrupt: true);
			return textColor;
		}

		public static TweenBase Color(Text target, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startValue;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Light target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LightColor lightColor = new LightColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(lightColor, interrupt: true);
			return lightColor;
		}

		public static TweenBase Color(Light target, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startValue;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(TextMesh target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			TextMeshColor textMeshColor = new TextMeshColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(textMeshColor, interrupt: true);
			return textMeshColor;
		}

		public static TweenBase Color(TextMesh target, Color startValue, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startValue;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Material target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			ShaderColor shaderColor = new ShaderColor(target, "_Color", endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(shaderColor, interrupt: true);
			return shaderColor;
		}

		public static TweenBase Color(Material target, Color startColor, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startColor;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Renderer target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			return Color(target.material, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Renderer target, Color startColor, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			return Color(target.material, startColor, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(SpriteRenderer target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			SpriteRendererColor spriteRendererColor = new SpriteRendererColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(spriteRendererColor, interrupt: true);
			return spriteRendererColor;
		}

		public static TweenBase Color(SpriteRenderer target, Color startColor, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.color = startColor;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Color(Camera target, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			CameraBackgroundColor cameraBackgroundColor = new CameraBackgroundColor(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(cameraBackgroundColor, interrupt: true);
			return cameraBackgroundColor;
		}

		public static TweenBase Color(Camera target, Color startColor, Color endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.backgroundColor = startColor;
			return Color(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Position(Transform target, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Position position = new Position(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(position, interrupt: true);
			return position;
		}

		public static TweenBase Position(Transform target, Vector3 startValue, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.position = startValue;
			return Position(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase AnchoredPosition(RectTransform target, Vector2 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			AnchoredPosition anchoredPosition = new AnchoredPosition(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(anchoredPosition, interrupt: true);
			return anchoredPosition;
		}

		public static TweenBase AnchoredPosition(RectTransform target, Vector2 startValue, Vector2 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.anchoredPosition = startValue;
			return AnchoredPosition(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LocalPosition(Transform target, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LocalPosition localPosition = new LocalPosition(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(localPosition, interrupt: true);
			return localPosition;
		}

		public static TweenBase LocalPosition(Transform target, Vector3 startValue, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.localPosition = startValue;
			return LocalPosition(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Rotate(Transform target, Vector3 amount, Space space, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Pixelplacement.TweenSystem.Rotate rotate = new Pixelplacement.TweenSystem.Rotate(target, amount, space, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(rotate, interrupt: true);
			return rotate;
		}

		public static TweenBase Rotation(Transform target, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			endValue = Quaternion.Euler(endValue).eulerAngles;
			Rotation rotation = new Rotation(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(rotation, interrupt: true);
			return rotation;
		}

		public static TweenBase Rotation(Transform target, Vector3 startValue, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			startValue = Quaternion.Euler(startValue).eulerAngles;
			endValue = Quaternion.Euler(endValue).eulerAngles;
			target.rotation = Quaternion.Euler(startValue);
			return Rotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase Rotation(Transform target, Quaternion endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Rotation rotation = new Rotation(target, endValue.eulerAngles, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(rotation, interrupt: true);
			return rotation;
		}

		public static TweenBase Rotation(Transform target, Quaternion startValue, Quaternion endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.rotation = startValue;
			return Rotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LocalRotation(Transform target, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			endValue = Quaternion.Euler(endValue).eulerAngles;
			LocalRotation localRotation = new LocalRotation(target, endValue, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(localRotation, interrupt: true);
			return localRotation;
		}

		public static TweenBase LocalRotation(Transform target, Vector3 startValue, Vector3 endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			startValue = Quaternion.Euler(startValue).eulerAngles;
			endValue = Quaternion.Euler(endValue).eulerAngles;
			target.localRotation = Quaternion.Euler(startValue);
			return LocalRotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LocalRotation(Transform target, Quaternion endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			LocalRotation localRotation = new LocalRotation(target, endValue.eulerAngles, duration, delay, obeyTimescale, easeCurve, loop, startCallback, completeCallback);
			SendTweenForProcessing(localRotation, interrupt: true);
			return localRotation;
		}

		public static TweenBase LocalRotation(Transform target, Quaternion startValue, Quaternion endValue, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			target.localRotation = startValue;
			return LocalRotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LookAt(Transform target, Transform targetToLookAt, Vector3 up, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Quaternion endValue = Quaternion.LookRotation(targetToLookAt.position - target.position, up);
			return Rotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static TweenBase LookAt(Transform target, Vector3 positionToLookAt, Vector3 up, float duration, float delay, AnimationCurve easeCurve = null, LoopType loop = LoopType.None, Action startCallback = null, Action completeCallback = null, bool obeyTimescale = true)
		{
			Quaternion endValue = Quaternion.LookRotation(positionToLookAt - target.position, up);
			return Rotation(target, endValue, duration, delay, easeCurve, loop, startCallback, completeCallback, obeyTimescale);
		}

		public static void Stop(int targetInstanceID, TweenType tweenType)
		{
			if (targetInstanceID == -1)
			{
				return;
			}
			for (int i = 0; i < activeTweens.Count; i++)
			{
				if (activeTweens[i].targetInstanceID == targetInstanceID && activeTweens[i].tweenType == tweenType && activeTweens[i].Status != 0)
				{
					activeTweens[i].Stop();
				}
			}
		}

		public static void Stop(int targetInstanceID)
		{
			StopInstanceTarget(targetInstanceID);
		}

		public static void StopAll()
		{
			foreach (TweenBase activeTween in activeTweens)
			{
				activeTween.Stop();
			}
		}

		public static void FinishAll()
		{
			foreach (TweenBase activeTween in activeTweens)
			{
				activeTween.Finish();
			}
		}

		public static void Finish(int targetInstanceID)
		{
			FinishInstanceTarget(targetInstanceID);
		}

		public static void Cancel(int targetInstanceID)
		{
			CancelInstanceTarget(targetInstanceID);
		}

		public static void CancelAll()
		{
			foreach (TweenBase activeTween in activeTweens)
			{
				activeTween.Cancel();
			}
		}

		private static void StopInstanceTarget(int id)
		{
			for (int i = 0; i < activeTweens.Count; i++)
			{
				if (activeTweens[i].targetInstanceID == id)
				{
					activeTweens[i].Stop();
				}
			}
		}

		private static void StopInstanceTargetType(int id, TweenType type)
		{
			for (int i = 0; i < activeTweens.Count; i++)
			{
				if (activeTweens[i].targetInstanceID == id && activeTweens[i].tweenType == type)
				{
					activeTweens[i].Stop();
				}
			}
		}

		private static void FinishInstanceTarget(int id)
		{
			for (int i = 0; i < activeTweens.Count; i++)
			{
				if (activeTweens[i].targetInstanceID == id)
				{
					activeTweens[i].Finish();
				}
			}
		}

		private static void CancelInstanceTarget(int id)
		{
			for (int i = 0; i < activeTweens.Count; i++)
			{
				if (activeTweens[i].targetInstanceID == id)
				{
					activeTweens[i].Cancel();
				}
			}
		}

		private static void SendTweenForProcessing(TweenBase tween, bool interrupt = false)
		{
			if (Application.isPlaying)
			{
				if (interrupt && tween.Delay == 0f)
				{
					StopInstanceTargetType(tween.targetInstanceID, tween.tweenType);
				}
				Instance.ExecuteTween(tween);
			}
		}
	}
}
