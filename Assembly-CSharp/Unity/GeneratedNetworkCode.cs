using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public class GeneratedNetworkCode
	{
		public static void _WriteArraySingle_None(NetworkWriter writer, float[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static float[] _ReadArraySingle_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new float[0];
			}
			float[] array = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadSingle();
			}
			return array;
		}

		public static void _WriteArrayInt32_None(NetworkWriter writer, int[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.WritePackedUInt32((uint)value[num]);
			}
		}

		public static int[] _ReadArrayInt32_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new int[0];
			}
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (int)reader.ReadPackedUInt32();
			}
			return array;
		}

		public static void _WriteArrayVector2_None(NetworkWriter writer, Vector2[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Vector2[] _ReadArrayVector2_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Vector2[0];
			}
			Vector2[] array = new Vector2[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadVector2();
			}
			return array;
		}

		public static void _WriteArrayVector3_None(NetworkWriter writer, Vector3[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Vector3[] _ReadArrayVector3_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Vector3[0];
			}
			Vector3[] array = new Vector3[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadVector3();
			}
			return array;
		}

		public static void _WriteArrayQuaternion_None(NetworkWriter writer, Quaternion[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Quaternion[] _ReadArrayQuaternion_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Quaternion[0];
			}
			Quaternion[] array = new Quaternion[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadQuaternion();
			}
			return array;
		}

		public static void _WriteArrayVector4_None(NetworkWriter writer, Vector4[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Vector4[] _ReadArrayVector4_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Vector4[0];
			}
			Vector4[] array = new Vector4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadVector4();
			}
			return array;
		}

		public static void _WriteArrayRect_None(NetworkWriter writer, Rect[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Rect[] _ReadArrayRect_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Rect[0];
			}
			Rect[] array = new Rect[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadRect();
			}
			return array;
		}

		public static void _WriteArrayColor_None(NetworkWriter writer, Color[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Color[] _ReadArrayColor_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Color[0];
			}
			Color[] array = new Color[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadColor();
			}
			return array;
		}

		public static void _WriteArrayColor32_None(NetworkWriter writer, Color32[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static Color32[] _ReadArrayColor32_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new Color32[0];
			}
			Color32[] array = new Color32[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadColor32();
			}
			return array;
		}

		public static void _WriteArrayString_None(NetworkWriter writer, string[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static string[] _ReadArrayString_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new string[0];
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadString();
			}
			return array;
		}

		public static void _WriteArrayBoolean_None(NetworkWriter writer, bool[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num = (ushort)(num + 1))
			{
				writer.Write(value[num]);
			}
		}

		public static bool[] _ReadArrayBoolean_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new bool[0];
			}
			bool[] array = new bool[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.ReadBoolean();
			}
			return array;
		}
	}
}
