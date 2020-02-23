using GILES.Interface;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GILES
{
	public class pb_InputManager : pb_MonoBehaviourSingleton<pb_InputManager>
	{
		public pb_SceneEditor currentEditor;

		private EventSystem eventSystem;

		[HideInInspector]
		[SerializeField]
		public pb_SceneEditor _editor;

		public MouseInUse mouseUsedDelegate;

		public KeyInUse keyUsedDelegate;

		public static void AddMouseInUseDelegate(MouseInUse del)
		{
			if (pb_MonoBehaviourSingleton<pb_InputManager>.instance.mouseUsedDelegate == null)
			{
				pb_MonoBehaviourSingleton<pb_InputManager>.instance.mouseUsedDelegate = del;
				return;
			}
			pb_InputManager instance = pb_MonoBehaviourSingleton<pb_InputManager>.instance;
			instance.mouseUsedDelegate = (MouseInUse)Delegate.Combine(instance.mouseUsedDelegate, del);
		}

		public static void RemoveMouseInUseDelegate(MouseInUse del)
		{
			pb_InputManager instance = pb_MonoBehaviourSingleton<pb_InputManager>.instance;
			instance.mouseUsedDelegate = (MouseInUse)Delegate.Remove(instance.mouseUsedDelegate, del);
		}

		public static void AddKeyInUseDelegate(KeyInUse del)
		{
			if (pb_MonoBehaviourSingleton<pb_InputManager>.instance.keyUsedDelegate == null)
			{
				pb_MonoBehaviourSingleton<pb_InputManager>.instance.keyUsedDelegate = del;
				return;
			}
			pb_InputManager instance = pb_MonoBehaviourSingleton<pb_InputManager>.instance;
			instance.keyUsedDelegate = (KeyInUse)Delegate.Combine(instance.keyUsedDelegate, del);
		}

		public static void RemoveKeyInUseDelegate(KeyInUse del)
		{
			pb_InputManager instance = pb_MonoBehaviourSingleton<pb_InputManager>.instance;
			instance.keyUsedDelegate = (KeyInUse)Delegate.Remove(instance.keyUsedDelegate, del);
		}

		public void SetEditor(pb_SceneEditor editor)
		{
			if (_editor != null)
			{
				_editor.Disable();
			}
			_editor = editor;
			if (_editor != null)
			{
				_editor.Enable();
			}
		}

		public static pb_SceneEditor GetCurrentEditor()
		{
			if (pb_MonoBehaviourSingleton<pb_InputManager>.instance._editor == null)
			{
				Debug.Log("NULL! e r est");
			}
			return pb_MonoBehaviourSingleton<pb_InputManager>.instance._editor;
		}

		protected override void Awake()
		{
			base.Awake();
			Cursor.lockState = CursorLockMode.None;
		}

		protected virtual void Start()
		{
			eventSystem = EventSystem.current;
			pb_Scene.AddOnLevelLoadedListener(OnLevelReset);
			pb_Scene.AddOnLevelClearedListener(OnLevelReset);
			SetEditor(currentEditor);
		}

		private void Update()
		{
			if (!(_editor == null))
			{
				if (Input.anyKeyDown && !IsKeyInUse() && !ProcessKeyInput())
				{
					_editor.OnKeyDownBase();
				}
				_editor.UpdateBase();
			}
		}

		private void OnGUI()
		{
			_editor.OnGUI();
		}

		private void OnLevelReset()
		{
			if (pb_ScriptableObjectSingleton<Undo>.nullableInstance != null)
			{
				UnityEngine.Object.Destroy(pb_ScriptableObjectSingleton<Undo>.nullableInstance);
			}
		}

		public static bool IsMouseInUse()
		{
			if (pb_MonoBehaviourSingleton<pb_InputManager>.instance.mouseUsedDelegate != null)
			{
				return pb_MonoBehaviourSingleton<pb_InputManager>.instance.mouseUsedDelegate.GetInvocationList().Any((Delegate x) => ((MouseInUse)x)(Input.mousePosition));
			}
			return false;
		}

		public static bool IsKeyInUse()
		{
			if (pb_MonoBehaviourSingleton<pb_InputManager>.instance.keyUsedDelegate != null)
			{
				return pb_MonoBehaviourSingleton<pb_InputManager>.instance.keyUsedDelegate.GetInvocationList().Any((Delegate x) => ((KeyInUse)x)());
			}
			return false;
		}

		private bool ProcessKeyInput()
		{
			if (eventSystem != null && eventSystem.currentSelectedGameObject != null)
			{
				if (Input.GetKey(KeyCode.Tab))
				{
					Selectable nextSelectable = pb_GUIUtility.GetNextSelectable(eventSystem.currentSelectedGameObject.GetComponent<Selectable>());
					if (nextSelectable != null)
					{
						InputField component = nextSelectable.GetComponent<InputField>();
						if (component != null)
						{
							component.OnPointerClick(new PointerEventData(eventSystem));
						}
					}
				}
				return true;
			}
			if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl))
			{
				if (Input.GetKey(KeyCode.Z))
				{
					Undo.PerformUndo();
					return true;
				}
				if (Input.GetKey(KeyCode.Y))
				{
					Undo.PerformRedo();
					return true;
				}
			}
			if (Input.GetKey(KeyCode.Delete))
			{
				Undo.RegisterStates(new IUndo[2]
				{
					new UndoDelete(pb_Selection.gameObjects),
					new UndoSelection()
				}, "Delete Selection");
				pb_Selection.Clear();
			}
			return false;
		}
	}
}
