using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GILES
{
	[Serializable]
	[CreateAssetMenu(menuName = "Scene Editor", fileName = "RT Scene Editor", order = 800)]
	public abstract class pb_SceneEditor : ScriptableObject
	{
		private readonly KeyCode SHORTCUT_TRANSLATE = KeyCode.W;

		private readonly KeyCode SHORTCUT_ROTATE = KeyCode.E;

		private readonly KeyCode SHORTCUT_SCALE = KeyCode.R;

		private bool ignoreMouse;

		[SerializeField]
		private Vector2 _mouseOrigin;

		[SerializeField]
		private Vector2 _mousePosition;

		internal pb_SelectionHandle handle => pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance;

		internal void Enable()
		{
			handle.OnHandleBegin += OnHandleBegin;
			handle.OnHandleMove += OnHandleMove;
			handle.OnHandleFinish += OnHandleFinish;
			pb_Selection.AddOnSelectionChangeListener(OnSelectionChange);
			pb_InputManager.AddMouseInUseDelegate((Vector2 mpos) => EventSystem.current != null && EventSystem.current.IsPointerOverGameObject());
			OnEnable();
		}

		internal void Disable()
		{
			handle.OnHandleBegin -= OnHandleBegin;
			handle.OnHandleMove -= OnHandleMove;
			handle.SetIsHidden(isHidden: true);
			OnDisabled();
		}

		internal void UpdateBase()
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (!ignoreMouse)
				{
					OnMouseUp();
				}
				ignoreMouse = false;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				if (IsMouseInUse())
				{
					ignoreMouse = true;
				}
				else
				{
					ignoreMouse = false;
					OnMouseDown();
				}
			}
			else if (Input.GetMouseButton(0) && !ignoreMouse)
			{
				OnMouseMove();
			}
			Update();
		}

		public void OnKeyDownBase()
		{
			if (Input.GetKey(SHORTCUT_TRANSLATE))
			{
				handle.SetTool(Tool.Position);
			}
			else if (Input.GetKey(SHORTCUT_ROTATE))
			{
				handle.SetTool(Tool.Rotate);
			}
			else if (Input.GetKey(SHORTCUT_SCALE))
			{
				handle.SetTool(Tool.Scale);
			}
			else if (Input.GetKey(KeyCode.F))
			{
				OnFrameSelection();
			}
			OnKeyDown();
		}

		public virtual void Update()
		{
		}

		public virtual void OnGUI()
		{
		}

		public virtual void OnMouseDown()
		{
		}

		public virtual void OnMouseMove()
		{
		}

		public virtual void OnMouseUp()
		{
		}

		public virtual void OnHandleBegin(pb_Transform transform)
		{
		}

		public virtual void OnHandleMove(pb_Transform transform)
		{
		}

		public virtual void OnHandleFinish()
		{
		}

		public virtual void OnKeyDown()
		{
		}

		public virtual void OnFrameSelection()
		{
		}

		public virtual void OnEnable()
		{
		}

		public virtual void OnDisabled()
		{
		}

		public virtual void OnSelectionChange(IEnumerable<GameObject> added)
		{
		}

		public virtual bool EnableCameraControls()
		{
			return Input.GetKey(KeyCode.LeftAlt);
		}

		public virtual bool IsMouseInUse()
		{
			return pb_InputManager.IsMouseInUse();
		}
	}
}
