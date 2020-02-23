using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	public class TweenUtilities : MonoBehaviour
	{
		public static void GenerateAnimationCurvePropertyCode(AnimationCurve curve)
		{
			string text = "get { return new AnimationCurve (";
			for (int i = 0; i < curve.keys.Length; i++)
			{
				Keyframe keyframe = curve.keys[i];
				text = text + "new Keyframe (" + keyframe.time + "f, " + keyframe.value + "f, " + keyframe.inTangent + "f, " + keyframe.outTangent + "f)";
				if (i < curve.keys.Length - 1)
				{
					text += ", ";
				}
			}
			text += "); }";
			Debug.Log(text);
		}

		public static float LinearInterpolate(float from, float to, float percentage)
		{
			return (to - from) * percentage + from;
		}

		public static Vector2 LinearInterpolate(Vector2 from, Vector2 to, float percentage)
		{
			return new Vector2(LinearInterpolate(from.x, to.x, percentage), LinearInterpolate(from.y, to.y, percentage));
		}

		public static Vector3 LinearInterpolate(Vector3 from, Vector3 to, float percentage)
		{
			return new Vector3(LinearInterpolate(from.x, to.x, percentage), LinearInterpolate(from.y, to.y, percentage), LinearInterpolate(from.z, to.z, percentage));
		}

		public static Vector3 LinearInterpolateRotational(Vector3 from, Vector3 to, float percentage)
		{
			return new Vector3(CylindricalLerp(from.x, to.x, percentage), CylindricalLerp(from.y, to.y, percentage), CylindricalLerp(from.z, to.z, percentage));
		}

		public static Vector4 LinearInterpolate(Vector4 from, Vector4 to, float percentage)
		{
			return new Vector4(LinearInterpolate(from.x, to.x, percentage), LinearInterpolate(from.y, to.y, percentage), LinearInterpolate(from.z, to.z, percentage), LinearInterpolate(from.w, to.w, percentage));
		}

		public static Rect LinearInterpolate(Rect from, Rect to, float percentage)
		{
			return new Rect(LinearInterpolate(from.x, to.x, percentage), LinearInterpolate(from.y, to.y, percentage), LinearInterpolate(from.width, to.width, percentage), LinearInterpolate(from.height, to.height, percentage));
		}

		public static Color LinearInterpolate(Color from, Color to, float percentage)
		{
			return new Color(LinearInterpolate(from.r, to.r, percentage), LinearInterpolate(from.g, to.g, percentage), LinearInterpolate(from.b, to.b, percentage), LinearInterpolate(from.a, to.a, percentage));
		}

		public static float EvaluateCurve(AnimationCurve curve, float percentage)
		{
			return curve.Evaluate(curve[curve.length - 1].time * percentage);
		}

		private static float CylindricalLerp(float from, float to, float percentage)
		{
			float num = 0f;
			float num2 = 360f;
			float num3 = Mathf.Abs((num2 - num) * 0.5f);
			float num4 = 0f;
			float num5 = 0f;
			if (to - from < 0f - num3)
			{
				num5 = (num2 - from + to) * percentage;
				return from + num5;
			}
			if (to - from > num3)
			{
				num5 = (0f - (num2 - to + from)) * percentage;
				return from + num5;
			}
			return from + (to - from) * percentage;
		}
	}
}
