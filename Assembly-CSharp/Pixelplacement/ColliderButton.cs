using Pixelplacement.TweenSystem;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Pixelplacement
{
	[RequireComponent(typeof(Rigidbody))]
	[ExecuteInEditMode]
	[RequireComponent(typeof(Collider))]
	public sealed class ColliderButton : MonoBehaviour
	{
		public enum EaseType
		{
			EaseOut,
			EaseOutBack
		}

		public ColliderButtonEvent OnSelected;

		public ColliderButtonEvent OnDeselected;

		public ColliderButtonEvent OnClick;

		public ColliderButtonEvent OnPressed;

		public ColliderButtonEvent OnReleased;

		public KeyCode[] keyInput;

		public bool _unityEventsFolded;

		public bool _scaleResponseFolded;

		public bool _colorResponseFolded;

		public bool applyColor;

		public bool applyScale;

		public LayerMask collisionLayerMask = -1;

		public Renderer colorRendererTarget;

		public Image colorImageTarget;

		public Color selectedColor = Color.gray;

		public Color pressedColor = Color.green;

		public float colorDuration = 0.1f;

		public Transform scaleTarget;

		public Vector3 normalScale;

		public Vector3 selectedScale;

		public Vector3 pressedScale;

		public float scaleDuration = 0.1f;

		public EaseType scaleEaseType;

		public bool resizeGUIBoxCollider = true;

		public Vector2 guiBoxColliderPadding;

		private bool _clicking;

		private int _selectedCount;

		private bool _colliderSelected;

		private bool _pressed;

		private bool _released;

		private bool _vrRunning;

		private RectTransform _rectTransform;

		private EventTrigger _eventTrigger;

		private EventTrigger.Entry _pressedEventTrigger;

		private EventTrigger.Entry _releasedEventTrigger;

		private EventTrigger.Entry _enterEventTrigger;

		private EventTrigger.Entry _exitEventTrigger;

		private int _colliderCount;

		private BoxCollider _boxCollider;

		private TweenBase _colorTweenImage;

		private TweenBase _colorTweenMaterial;

		private TweenBase _scaleTween;

		private Color _normalColorRenderer;

		private Color _normalColorImage;

		public bool IsSelected
		{
			get;
			private set;
		}

		public static event Action<ColliderButton> OnSelectedGlobal;

		public static event Action<ColliderButton> OnDeselectedGlobal;

		public static event Action<ColliderButton> OnClickGlobal;

		public static event Action<ColliderButton> OnPressedGlobal;

		public static event Action<ColliderButton> OnReleasedGlobal;

		private void Reset()
		{
			applyColor = true;
			keyInput = new KeyCode[1]
			{
				KeyCode.Mouse0
			};
			Image component = GetComponent<Image>();
			if (component != null)
			{
				colorImageTarget = component;
			}
			Renderer component2 = GetComponent<Renderer>();
			if (component2 != null && component2.sharedMaterial.HasProperty("_Color"))
			{
				colorRendererTarget = component2;
			}
		}

		private void Awake()
		{
			if (Application.isPlaying)
			{
				if (colorRendererTarget != null && colorRendererTarget.material.HasProperty("_Color"))
				{
					_normalColorRenderer = colorRendererTarget.material.color;
				}
				if (colorImageTarget != null)
				{
					_normalColorImage = colorImageTarget.color;
				}
			}
			scaleTarget = base.transform;
			normalScale = base.transform.localScale;
			selectedScale = base.transform.localScale * 1.15f;
			pressedScale = base.transform.localScale * 1.25f;
			_rectTransform = GetComponent<RectTransform>();
			_boxCollider = GetComponent<BoxCollider>();
			if (_rectTransform != null && _boxCollider != null)
			{
				ResizeGUIBoxCollider(_boxCollider);
			}
			GetComponent<Rigidbody>().isKinematic = true;
			_rectTransform = GetComponent<RectTransform>();
			_boxCollider = GetComponent<BoxCollider>();
			if (Application.isPlaying)
			{
				_rectTransform = GetComponent<RectTransform>();
				if (_rectTransform != null)
				{
					_eventTrigger = base.gameObject.AddComponent<EventTrigger>();
					_pressedEventTrigger = new EventTrigger.Entry();
					_pressedEventTrigger.eventID = EventTriggerType.PointerDown;
					_releasedEventTrigger = new EventTrigger.Entry();
					_releasedEventTrigger.eventID = EventTriggerType.PointerUp;
					_enterEventTrigger = new EventTrigger.Entry();
					_enterEventTrigger.eventID = EventTriggerType.PointerEnter;
					_exitEventTrigger = new EventTrigger.Entry();
					_exitEventTrigger.eventID = EventTriggerType.PointerExit;
				}
				if (_rectTransform != null)
				{
					_pressedEventTrigger.callback.AddListener(delegate(BaseEventData data)
					{
						OnPointerDownDelegate((PointerEventData)data);
					});
					_eventTrigger.triggers.Add(_pressedEventTrigger);
					_releasedEventTrigger.callback.AddListener(delegate(BaseEventData data)
					{
						OnPointerUpDelegate((PointerEventData)data);
					});
					_eventTrigger.triggers.Add(_releasedEventTrigger);
					_enterEventTrigger.callback.AddListener(delegate(BaseEventData data)
					{
						OnPointerEnterDelegate((PointerEventData)data);
					});
					_eventTrigger.triggers.Add(_enterEventTrigger);
					_exitEventTrigger.callback.AddListener(delegate(BaseEventData data)
					{
						OnPointerExitDelegate((PointerEventData)data);
					});
					_eventTrigger.triggers.Add(_exitEventTrigger);
				}
			}
		}

		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				ColorReset();
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				_pressed = false;
				_released = false;
				_clicking = false;
				_colliderSelected = false;
				_selectedCount = 0;
				_colliderCount = 0;
				ColorReset();
				ScaleReset();
			}
		}

		private void Update()
		{
			if (resizeGUIBoxCollider && _rectTransform != null && _boxCollider != null)
			{
				ResizeGUIBoxCollider(_boxCollider);
			}
			if (!Application.isPlaying)
			{
				return;
			}
			_vrRunning = XRSettings.isDeviceActive;
			if (!_colliderSelected && _colliderCount > 0)
			{
				_colliderSelected = true;
				Selected();
			}
			if (_colliderSelected && _colliderCount == 0)
			{
				_colliderSelected = false;
				Deselected();
			}
			if (keyInput == null || _selectedCount <= 0)
			{
				return;
			}
			KeyCode[] array = keyInput;
			foreach (KeyCode key in array)
			{
				if (Input.GetKeyDown(key))
				{
					if (_selectedCount == 0)
					{
						break;
					}
					Pressed();
				}
				if (Input.GetKeyUp(key))
				{
					Released();
				}
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (_colliderCount == 0)
			{
				_colliderCount++;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			_colliderCount++;
		}

		private void OnTriggerExit(Collider other)
		{
			_colliderCount--;
		}

		private void OnPointerDownDelegate(PointerEventData data)
		{
			if (Array.IndexOf(keyInput, KeyCode.Mouse0) != -1)
			{
				Pressed();
			}
		}

		private void OnPointerUpDelegate(PointerEventData data)
		{
			if (Array.IndexOf(keyInput, KeyCode.Mouse0) != -1)
			{
				Released();
			}
		}

		private void OnPointerEnterDelegate(PointerEventData data)
		{
			Selected();
		}

		private void OnPointerExitDelegate(PointerEventData data)
		{
			Deselected();
		}

		private void OnMouseDown()
		{
			if (!_vrRunning && Array.IndexOf(keyInput, KeyCode.Mouse0) != -1)
			{
				Pressed();
			}
		}

		private void OnMouseUp()
		{
			if (!_vrRunning && Array.IndexOf(keyInput, KeyCode.Mouse0) != -1)
			{
				Released();
				if (Application.isMobilePlatform)
				{
					Deselected();
				}
			}
		}

		private void OnMouseEnter()
		{
			if (!Application.isMobilePlatform && !_vrRunning)
			{
				Selected();
			}
		}

		private void OnMouseExit()
		{
			if (!_vrRunning)
			{
				Deselected();
			}
		}

		public void Deselected()
		{
			_selectedCount--;
			if (_selectedCount < 0)
			{
				_selectedCount = 0;
			}
			if (_selectedCount > 0)
			{
				return;
			}
			_clicking = false;
			ColorNormal();
			ScaleNormal();
			if (!Application.isMobilePlatform)
			{
				if (OnDeselected != null)
				{
					OnDeselected.Invoke(this);
				}
				if (ColliderButton.OnDeselectedGlobal != null)
				{
					ColliderButton.OnDeselectedGlobal(this);
				}
				IsSelected = false;
			}
		}

		public void Selected()
		{
			_selectedCount++;
			if (_selectedCount == 1)
			{
				_pressed = false;
				_released = false;
				_clicking = false;
				ColorSelected();
				ScaleSelected();
				if (OnSelected != null)
				{
					OnSelected.Invoke(this);
				}
				if (ColliderButton.OnSelectedGlobal != null)
				{
					ColliderButton.OnSelectedGlobal(this);
				}
				IsSelected = true;
			}
		}

		public void Pressed()
		{
			if (_selectedCount > 0 && !_pressed)
			{
				_pressed = true;
				_released = false;
				_clicking = true;
				ColorPressed();
				ScalePressed();
				if (OnPressed != null)
				{
					OnPressed.Invoke(this);
				}
				if (ColliderButton.OnPressedGlobal != null)
				{
					ColliderButton.OnPressedGlobal(this);
				}
			}
		}

		public void Released()
		{
			if (_released)
			{
				return;
			}
			_pressed = false;
			_released = true;
			if (_selectedCount != 0)
			{
				ColorSelected();
				ScaleSelected();
			}
			if (_clicking)
			{
				if (OnClick != null)
				{
					OnClick.Invoke(this);
				}
				if (ColliderButton.OnClickGlobal != null)
				{
					ColliderButton.OnClickGlobal(this);
				}
			}
			_clicking = false;
			if (OnReleased != null)
			{
				OnReleased.Invoke(this);
			}
			if (ColliderButton.OnReleasedGlobal != null)
			{
				ColliderButton.OnReleasedGlobal(this);
			}
		}

		private void ResizeGUIBoxCollider(BoxCollider boxCollider)
		{
			if (resizeGUIBoxCollider)
			{
				boxCollider.size = new Vector3(_rectTransform.rect.width + guiBoxColliderPadding.x, _rectTransform.rect.height + guiBoxColliderPadding.y, _boxCollider.size.z);
				float x = (Mathf.Abs(_rectTransform.pivot.x - 1f) - 0.5f) * boxCollider.size.x;
				float y = (Mathf.Abs(_rectTransform.pivot.y - 1f) - 0.5f) * boxCollider.size.y;
				boxCollider.center = new Vector3(x, y, boxCollider.center.z);
			}
		}

		private void ColorReset()
		{
			if (_colorTweenImage != null)
			{
				_colorTweenImage.Stop();
			}
			if (_colorTweenMaterial != null)
			{
				_colorTweenMaterial.Stop();
			}
			if (applyColor)
			{
				if (colorRendererTarget != null && colorRendererTarget.material.HasProperty("_Color"))
				{
					colorRendererTarget.material.color = _normalColorRenderer;
				}
				if (colorImageTarget != null)
				{
					colorImageTarget.color = _normalColorImage;
				}
			}
		}

		private void ColorNormal()
		{
			if (applyColor)
			{
				if (colorRendererTarget != null && colorRendererTarget.material.HasProperty("_Color"))
				{
					_colorTweenMaterial = Tween.Color(colorRendererTarget, _normalColorRenderer, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
				if (colorImageTarget != null)
				{
					Tween.Color(colorImageTarget, _normalColorImage, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
			}
		}

		private void ColorSelected()
		{
			if (applyColor)
			{
				if (colorRendererTarget != null && colorRendererTarget.material.HasProperty("_Color"))
				{
					_colorTweenMaterial = Tween.Color(colorRendererTarget, selectedColor, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
				if (colorImageTarget != null)
				{
					Tween.Color(colorImageTarget, selectedColor, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
			}
		}

		private void ColorPressed()
		{
			if (applyColor)
			{
				if (colorRendererTarget != null && colorRendererTarget.material.HasProperty("_Color"))
				{
					_colorTweenMaterial = Tween.Color(colorRendererTarget, pressedColor, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
				if (colorImageTarget != null)
				{
					Tween.Color(colorImageTarget, pressedColor, colorDuration, 0f, null, Tween.LoopType.None, null, null, obeyTimescale: false);
				}
			}
		}

		private void ScaleReset()
		{
			if (_scaleTween != null)
			{
				_scaleTween.Stop();
			}
			scaleTarget.localScale = normalScale;
		}

		private void ScaleNormal()
		{
			if (applyScale)
			{
				AnimationCurve easeCurve = null;
				switch (scaleEaseType)
				{
				case EaseType.EaseOut:
					easeCurve = Tween.EaseOutStrong;
					break;
				case EaseType.EaseOutBack:
					easeCurve = Tween.EaseOutBack;
					break;
				}
				_scaleTween = Tween.LocalScale(scaleTarget, normalScale, scaleDuration, 0f, easeCurve, Tween.LoopType.None, null, null, obeyTimescale: false);
			}
		}

		private void ScaleSelected()
		{
			if (applyScale)
			{
				AnimationCurve easeCurve = null;
				switch (scaleEaseType)
				{
				case EaseType.EaseOut:
					easeCurve = Tween.EaseOutStrong;
					break;
				case EaseType.EaseOutBack:
					easeCurve = Tween.EaseOutBack;
					break;
				}
				_scaleTween = Tween.LocalScale(scaleTarget, selectedScale, scaleDuration, 0f, easeCurve, Tween.LoopType.None, null, null, obeyTimescale: false);
			}
		}

		private void ScalePressed()
		{
			if (applyScale)
			{
				AnimationCurve easeCurve = null;
				switch (scaleEaseType)
				{
				case EaseType.EaseOut:
					easeCurve = Tween.EaseOutStrong;
					break;
				case EaseType.EaseOutBack:
					easeCurve = Tween.EaseOutBack;
					break;
				}
				_scaleTween = Tween.LocalScale(scaleTarget, pressedScale, scaleDuration, 0f, easeCurve, Tween.LoopType.None, null, null, obeyTimescale: false);
			}
		}
	}
}
