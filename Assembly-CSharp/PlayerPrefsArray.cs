using UnityEngine;

public class PlayerPrefsArray : MonoBehaviour
{
	public static void SetIntArray(string key, int[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Int:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefs.SetInt("PlayerPrefsArray:Int:" + key + i.ToString(), value[i]);
		}
	}

	public static int[] GetIntArray(string key)
	{
		int[] array = new int[PlayerPrefs.GetInt("PlayerPrefsArray:Int:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Int:L:" + key); i++)
		{
			array.SetValue(PlayerPrefs.GetInt("PlayerPrefsArray:Int:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetFloatArray(string key, int[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Float:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefs.SetFloat("PlayerPrefsArray:Float:" + key + i.ToString(), value[i]);
		}
	}

	public static float[] GetFloatArray(string key)
	{
		float[] array = new float[PlayerPrefs.GetInt("PlayerPrefsArray:Float:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Float:L:" + key); i++)
		{
			array.SetValue(PlayerPrefs.GetFloat("PlayerPrefsArray:Float:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetStringArray(string key, string[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:String:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefs.SetString("PlayerPrefsArray:String:" + key + i.ToString(), value[i]);
		}
	}

	public static string[] GetStringArray(string key)
	{
		string[] array = new string[PlayerPrefs.GetInt("PlayerPrefsArray:String:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:String:L:" + key); i++)
		{
			array.SetValue(PlayerPrefs.GetString("PlayerPrefsArray:String:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetBoolArray(string key, bool[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Bool:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetBool("PlayerPrefsArray:Bool:" + key + i.ToString(), value[i]);
		}
	}

	public static bool[] GetBoolArray(string key)
	{
		bool[] array = new bool[PlayerPrefs.GetInt("PlayerPrefsArray:Bool:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Bool:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetBool("PlayerPrefsArray:Bool:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetColourArray(string key, Color[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Colour:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetColour("PlayerPrefsArray:Colour:" + key + i.ToString(), value[i]);
		}
	}

	public static Color[] GetColourArray(string key)
	{
		Color[] array = new Color[PlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetColour("PlayerPrefsArray:Colour:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetVector2Array(string key, Vector2[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Vector2:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetVector2("PlayerPrefsArray:Vector2:" + key + i.ToString(), value[i]);
		}
	}

	public static Vector2[] GetVector2Array(string key)
	{
		Vector2[] array = new Vector2[PlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetVector2("PlayerPrefsArray:Vector2:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetVector3Array(string key, Vector3[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Vector3:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetVector3("PlayerPrefsArray:Vector3:" + key + i.ToString(), value[i]);
		}
	}

	public static Vector3[] GetVector3Array(string key)
	{
		Vector3[] array = new Vector3[PlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetVector3("PlayerPrefsArray:Vector3:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetVector4Array(string key, Vector4[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Vector4:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetVector4("PlayerPrefsArray:Vector4:" + key + i.ToString(), value[i]);
		}
	}

	public static Vector4[] GetVector4Array(string key)
	{
		Vector4[] array = new Vector4[PlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetVector4("PlayerPrefsArray:Vector4:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetQuaternionArray(string key, Quaternion[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Quaternion:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetQuaternion("PlayerPrefsArray:Quaternion:" + key + i.ToString(), value[i]);
		}
	}

	public static Quaternion[] GetQuaternionArray(string key)
	{
		Quaternion[] array = new Quaternion[PlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetQuaternion("PlayerPrefsArray:Quaternion:" + key + i.ToString()), i);
		}
		return array;
	}

	public static void SetRectArray(string key, Rect[] value)
	{
		PlayerPrefs.SetInt("PlayerPrefsArray:Rect:L:" + key, value.Length);
		for (int i = 0; i < value.Length; i++)
		{
			PlayerPrefsPlus.SetRect("PlayerPrefsArray:Rect:" + key + i.ToString(), value[i]);
		}
	}

	public static Rect[] GetRectArray(string key)
	{
		Rect[] array = new Rect[PlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:" + key)];
		for (int i = 0; i < PlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:" + key); i++)
		{
			array.SetValue(PlayerPrefsPlus.GetRect("PlayerPrefsArray:Rect:" + key + i.ToString()), i);
		}
		return array;
	}
}
