using System;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_GUI
	{
		internal static int viewWidth;

		public static void HeaderLabel(string label)
		{
			GUILayout.Label("==== " + label + " ====");
		}

		public static void InspectorLabel(string label)
		{
			GUILayout.Label(label, GUILayout.MaxWidth(120f));
		}

		public static float FloatField(float value, params GUILayoutOption[] layoutOptions)
		{
			if (float.TryParse(GUILayout.TextField(value.ToString("G"), layoutOptions), out float result) && !Mathf.Approximately(value, result))
			{
				return result;
			}
			return value;
		}

		public static int IntField(int value)
		{
			if (int.TryParse(GUILayout.TextField(value.ToString()), out int result))
			{
				return result;
			}
			return value;
		}

		public static int EnumField(Enum value)
		{
			Array values = Enum.GetValues(value.GetType());
			return (int)GUILayout.HorizontalSlider(Convert.ToInt32(value), 0f, values.Length);
		}

		public static Vector2 Vector2Field(Vector2 value)
		{
			GUILayout.BeginHorizontal();
			value.x = FloatField(value.x);
			value.y = FloatField(value.y);
			GUILayout.EndHorizontal();
			return value;
		}

		public static Vector3 Vector3Field(Vector3 value, int width = 0)
		{
			GUILayout.BeginHorizontal();
			if (width > 0)
			{
				width = width / 3 - 1;
				value.x = FloatField(value.x, GUILayout.MinWidth(width), GUILayout.MaxWidth(width));
				value.y = FloatField(value.y, GUILayout.MinWidth(width), GUILayout.MaxWidth(width));
				value.z = FloatField(value.z, GUILayout.MinWidth(width), GUILayout.MaxWidth(width));
			}
			else
			{
				value.x = FloatField(value.x);
				value.y = FloatField(value.y);
				value.z = FloatField(value.z);
			}
			GUILayout.EndHorizontal();
			return value;
		}

		public static Vector4 Vector4Field(Vector4 value)
		{
			GUILayout.BeginHorizontal();
			value.x = FloatField(value.x);
			value.y = FloatField(value.y);
			value.z = FloatField(value.z);
			value.w = FloatField(value.w);
			GUILayout.EndHorizontal();
			return value;
		}

		public static Quaternion QuaternionField(Quaternion value)
		{
			GUILayout.BeginHorizontal();
			value.x = FloatField(value.x);
			value.y = FloatField(value.y);
			value.z = FloatField(value.z);
			value.w = FloatField(value.w);
			GUILayout.EndHorizontal();
			return value;
		}
	}
}
