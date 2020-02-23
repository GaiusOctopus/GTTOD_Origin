using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class KeyBinding : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public delegate void remap(KeyBinding key);

	public KeyAction keyAction;

	public KeyCode keyCode = KeyCode.W;

	public Text keyDisplay;

	public GameObject button;

	public Color toggleColor = new Color(0.75f, 0.75f, 0.75f, 1f);

	private Image buttonImage;

	private Color originalColor;

	public bool allowMouseButtons = true;

	private bool reassignKey;

	private Event curEvent;

	private bool isHovering;

	public static event remap keyRemap;

	private void OnGUI()
	{
		curEvent = Event.current;
		if (curEvent.isKey && curEvent.keyCode != 0 && reassignKey)
		{
			this.keyCode = curEvent.keyCode;
			ChangeKeyCode(toggle: false);
			UpdateKeyCode();
			SaveKeyCode();
		}
		else if (curEvent.shift && reassignKey)
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				this.keyCode = KeyCode.LeftShift;
			}
			else
			{
				this.keyCode = KeyCode.RightShift;
			}
			ChangeKeyCode(toggle: false);
			UpdateKeyCode();
			SaveKeyCode();
		}
		else if (curEvent.isMouse && reassignKey && isHovering && allowMouseButtons)
		{
			StartCoroutine(Wait());
			KeyCode keyCode = this.keyCode = (KeyCode)(curEvent.button + 323);
			ChangeKeyCode(toggle: false);
			UpdateKeyCode();
			SaveKeyCode();
		}
		else if ((Input.GetKey(KeyCode.Mouse3) && reassignKey && isHovering && allowMouseButtons) || (Input.GetKey(KeyCode.Mouse4) && reassignKey && isHovering && allowMouseButtons) || (Input.GetKey(KeyCode.Mouse5) && reassignKey && isHovering && allowMouseButtons) || (Input.GetKey(KeyCode.Mouse6) && reassignKey && isHovering && allowMouseButtons))
		{
			StartCoroutine(Wait());
			if (Input.GetKey(KeyCode.Mouse3))
			{
				KeyCode keyCode2 = this.keyCode = KeyCode.Mouse3;
				ChangeKeyCode(toggle: false);
				UpdateKeyCode();
				SaveKeyCode();
			}
			else if (Input.GetKey(KeyCode.Mouse4))
			{
				KeyCode keyCode3 = this.keyCode = KeyCode.Mouse4;
				ChangeKeyCode(toggle: false);
				UpdateKeyCode();
				SaveKeyCode();
			}
			else if (Input.GetKey(KeyCode.Mouse5))
			{
				KeyCode keyCode4 = this.keyCode = KeyCode.Mouse5;
				ChangeKeyCode(toggle: false);
				UpdateKeyCode();
				SaveKeyCode();
			}
			else if (Input.GetKey(KeyCode.Mouse6))
			{
				KeyCode keyCode5 = this.keyCode = KeyCode.Mouse6;
				ChangeKeyCode(toggle: false);
				UpdateKeyCode();
				SaveKeyCode();
			}
		}
		else if (curEvent.isMouse && !isHovering)
		{
			ChangeKeyCode(toggle: false);
		}
	}

	public void OnPointerEnter(PointerEventData data)
	{
		isHovering = true;
	}

	public void OnPointerExit(PointerEventData data)
	{
		isHovering = false;
	}

	private void Awake()
	{
		buttonImage = button.GetComponent<Image>();
		originalColor = buttonImage.color;
		button.GetComponent<Button>().onClick.AddListener(delegate
		{
			ChangeKeyCode(toggle: true);
		});
	}

	private void OnEnable()
	{
		keyRemap += PreventDoubleAssign;
		KeyCode @int = (KeyCode)PlayerPrefs.GetInt(keyAction.ToString());
		if (@int.ToString() == "None")
		{
			Debug.Log(keyCode.ToString());
			keyDisplay.text = keyCode.ToString();
			UpdateKeyCode();
			SaveKeyCode();
		}
		else
		{
			keyCode = @int;
			keyDisplay.text = keyCode.ToString();
			UpdateKeyCode();
		}
	}

	public void LoadKeybinds()
	{
		keyRemap += PreventDoubleAssign;
		KeyCode @int = (KeyCode)PlayerPrefs.GetInt(keyAction.ToString());
		if (@int.ToString() == "None")
		{
			Debug.Log(keyCode.ToString());
			keyDisplay.text = keyCode.ToString();
			UpdateKeyCode();
			SaveKeyCode();
		}
		else
		{
			keyCode = @int;
			keyDisplay.text = keyCode.ToString();
			UpdateKeyCode();
		}
	}

	private void OnDisable()
	{
		keyRemap -= PreventDoubleAssign;
	}

	public void ChangeKeyCode(bool toggle)
	{
		reassignKey = toggle;
		if (toggle)
		{
			buttonImage.color = toggleColor;
			if (KeyBinding.keyRemap != null)
			{
				KeyBinding.keyRemap(this);
			}
		}
		else
		{
			buttonImage.color = originalColor;
		}
	}

	public void SaveKeyCode()
	{
		keyDisplay.text = keyCode.ToString();
		PlayerPrefs.SetInt(keyAction.ToString(), (int)keyCode);
		PlayerPrefs.Save();
	}

	private void PreventDoubleAssign(KeyBinding kb)
	{
		if (kb != this)
		{
			reassignKey = false;
			buttonImage.color = originalColor;
		}
	}

	public void UpdateKeyCode()
	{
		KeyBindingManager.UpdateDictionary(this);
	}

	private IEnumerator Wait()
	{
		button.GetComponent<Button>().onClick.RemoveAllListeners();
		yield return new WaitUntil(() => !Input.GetMouseButton(0));
		button.GetComponent<Button>().onClick.AddListener(delegate
		{
			ChangeKeyCode(toggle: true);
		});
	}
}
