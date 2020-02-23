using System.Collections.Generic;
using UnityEngine;

public static class KeyBindingManager
{
	public static Dictionary<KeyAction, KeyCode> keyDict = new Dictionary<KeyAction, KeyCode>();

	public static KeyCode GetKeyCode(KeyAction key)
	{
		KeyCode value = KeyCode.None;
		keyDict.TryGetValue(key, out value);
		return value;
	}

	public static bool GetKey(KeyAction key)
	{
		KeyCode value = KeyCode.None;
		keyDict.TryGetValue(key, out value);
		return Input.GetKey(value);
	}

	public static bool GetKeyDown(KeyAction key)
	{
		KeyCode value = KeyCode.None;
		keyDict.TryGetValue(key, out value);
		return Input.GetKeyDown(value);
	}

	public static bool GetKeyUp(KeyAction key)
	{
		KeyCode value = KeyCode.None;
		keyDict.TryGetValue(key, out value);
		return Input.GetKeyUp(value);
	}

	public static void UpdateDictionary(KeyBinding key)
	{
		if (!keyDict.ContainsKey(key.keyAction))
		{
			keyDict.Add(key.keyAction, key.keyCode);
		}
		else
		{
			keyDict[key.keyAction] = key.keyCode;
		}
	}
}
