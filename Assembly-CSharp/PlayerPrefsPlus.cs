using UnityEngine;

public class PlayerPrefsPlus : MonoBehaviour
{
	public static bool HasKey(string key)
	{
		string[] obj = new string[9]
		{
			"{0}",
			"PlayerPrefsPlus:bool:{0}",
			"PlayerPrefsPlus:Colour:{0}-r",
			"PlayerPrefsPlus:Colour32:{0}-r",
			"PlayerPrefsPlus:Vector2:{0}-x",
			"PlayerPrefsPlus:Vector3:{0}-x",
			"PlayerPrefsPlus:Vector4:{0}-x",
			"PlayerPrefsPlus:Vector3:Quaternion:{0}-x",
			"PlayerPrefsPlus:Vector4:Rect:{0}-x"
		};
		bool result = false;
		string[] array = obj;
		for (int i = 0; i < array.Length; i++)
		{
			if (PlayerPrefs.HasKey(string.Format(array[i], key)))
			{
				result = true;
			}
		}
		return result;
	}

	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
	}

	public static int GetInt(string key)
	{
		return PlayerPrefs.GetInt(key);
	}

	public static int GetInt(string key, int defaultValue)
	{
		return PlayerPrefs.GetInt(key, defaultValue);
	}

	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
	}

	public static float GetFloat(string key)
	{
		return PlayerPrefs.GetFloat(key);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		return PlayerPrefs.GetFloat(key, defaultValue);
	}

	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
	}

	public static string GetString(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	public static string GetString(string key, string defaultValue)
	{
		return PlayerPrefs.GetString(key, defaultValue);
	}

	public static void SetBool(string key, bool value)
	{
		if (value)
		{
			PlayerPrefs.SetInt("PlayerPrefsPlus:bool:" + key, 1);
		}
		else
		{
			PlayerPrefs.SetInt("PlayerPrefsPlus:bool:" + key, 0);
		}
	}

	public static bool GetBool(string key)
	{
		return GetBool(key, defaultValue: false);
	}

	public static bool GetBool(string key, bool defaultValue)
	{
		switch (PlayerPrefs.GetInt("PlayerPrefsPlus:bool:" + key, 2))
		{
		case 2:
			return defaultValue;
		case 1:
			return true;
		default:
			return false;
		}
	}

	public static void SetColour(string key, Color value)
	{
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-r", value.r);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-g", value.g);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-b", value.b);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-a", value.a);
	}

	public static Color GetColour(string key)
	{
		return GetColour(key, Color.clear);
	}

	public static Color GetColour(string key, Color defaultValue)
	{
		Color result = default(Color);
		result.r = PlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-r", defaultValue.r);
		result.g = PlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-g", defaultValue.g);
		result.b = PlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-b", defaultValue.b);
		result.a = PlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-a", defaultValue.a);
		return result;
	}

	public static void SetVector2(string key, Vector2 value)
	{
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector2:" + key + "-x", value.x);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector2:" + key + "-y", value.y);
	}

	public static Vector2 GetVector2(string key)
	{
		return GetVector2(key, Vector2.zero);
	}

	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		Vector2 result = default(Vector2);
		result.x = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector2:" + key + "-x", defaultValue.x);
		result.y = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector2:" + key + "-y", defaultValue.y);
		return result;
	}

	public static void SetVector3(string key, Vector3 value)
	{
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-x", value.x);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-y", value.y);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-z", value.z);
	}

	public static Vector3 GetVector3(string key)
	{
		return GetVector3(key, Vector3.zero);
	}

	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		Vector3 result = default(Vector3);
		result.x = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-x", defaultValue.x);
		result.y = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-y", defaultValue.y);
		result.z = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-z", defaultValue.z);
		return result;
	}

	public static void SetVector4(string key, Vector4 value)
	{
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-x", value.x);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-y", value.y);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-z", value.z);
		PlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-w", value.w);
	}

	public static Vector4 GetVector4(string key)
	{
		return GetVector4(key, Vector4.zero);
	}

	public static Vector4 GetVector4(string key, Vector4 defaultValue)
	{
		Vector4 result = default(Vector4);
		result.x = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-x", defaultValue.x);
		result.y = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-y", defaultValue.y);
		result.z = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-z", defaultValue.z);
		result.w = PlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-w", defaultValue.w);
		return result;
	}

	public static void SetQuaternion(string key, Quaternion value)
	{
		SetVector3("Quaternion:" + key, value.eulerAngles);
	}

	public static Quaternion GetQuaternion(string key)
	{
		return Quaternion.Euler(GetVector3("Quaternion:" + key, Quaternion.identity.eulerAngles));
	}

	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		return Quaternion.Euler(GetVector3("Quaternion:" + key, defaultValue.eulerAngles));
	}

	public static void SetRect(string key, Rect value)
	{
		SetVector4("Rect:" + key, new Vector4(value.x, value.y, value.width, value.height));
	}

	public static Rect GetRect(string key)
	{
		Vector4 vector = GetVector4("Rect:" + key, Vector4.zero);
		return new Rect(vector.x, vector.y, vector.z, vector.w);
	}

	public static Rect GetRect(string key, Rect defaultValue)
	{
		Vector4 vector = GetVector4("Rect:" + key, new Vector4(defaultValue.x, defaultValue.y, defaultValue.width, defaultValue.height));
		return new Rect(vector.x, vector.y, vector.z, vector.w);
	}
}
