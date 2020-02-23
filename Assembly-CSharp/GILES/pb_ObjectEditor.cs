using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GILES
{
	public class pb_ObjectEditor : pb_SceneEditor
	{
		private bool global = true;

		private Transform _mTransform;

		private pb_Transform handleTransformOrigin;

		private pb_Transform[] selectionTransformOrigin;

		private Vector2 mMouseOrigin = Vector2.zero;

		private bool mMouseDragging;

		private bool mMouseIsDown;

		private bool mDragCanceled;

		private const float MOUSE_DRAG_DELTA = 0.2f;

		private Rect dragRect = new Rect(0f, 0f, 0f, 0f);

		private GUIStyle dragRectStyle = new GUIStyle();

		public Color dragRectColor = new Color(0f, 0.75f, 1f, 0.6f);

		private Transform mTransform
		{
			get
			{
				if (_mTransform == null)
				{
					_mTransform = new GameObject().transform;
					_mTransform.name = "Handle Transform Parent";
				}
				return _mTransform;
			}
		}

		public override void OnEnable()
		{
			dragRectStyle.normal.background = pb_BuiltinResource.GetResource<Texture2D>("Image/White");
		}

		public override void OnHandleBegin(pb_Transform transform)
		{
			handleTransformOrigin = transform;
			CacheSelectionTransforms();
			Undo.RegisterStates(((IEnumerable<GameObject>)pb_Selection.gameObjects).Select((Func<GameObject, IUndo>)((GameObject x) => new UndoTransform(x.transform))).ToList(), base.handle.GetTool().ToString() + " Tool");
		}

		public override void OnHandleMove(pb_Transform transform)
		{
			pb_Transform rhs = transform - handleTransformOrigin;
			if (global)
			{
				mTransform.SetTRS(handleTransformOrigin);
				Transform[] array = new Transform[pb_Selection.gameObjects.Count];
				for (int i = 0; i < pb_Selection.gameObjects.Count; i++)
				{
					GameObject gameObject = pb_Selection.gameObjects[i];
					gameObject.transform.SetTRS(selectionTransformOrigin[i]);
					array[i] = gameObject.transform.parent;
					gameObject.transform.parent = mTransform;
				}
				mTransform.SetTRS(transform);
				for (int j = 0; j < pb_Selection.gameObjects.Count; j++)
				{
					pb_Selection.gameObjects[j].transform.parent = array[j];
				}
			}
			else
			{
				for (int k = 0; k < pb_Selection.gameObjects.Count; k++)
				{
					pb_Selection.gameObjects[k].transform.SetTRS(selectionTransformOrigin[k] + rhs);
				}
			}
		}

		private void CacheSelectionTransforms()
		{
			int count = pb_Selection.gameObjects.Count;
			selectionTransformOrigin = new pb_Transform[count];
			for (int i = 0; i < count; i++)
			{
				selectionTransformOrigin[i] = new pb_Transform(pb_Selection.gameObjects[i].transform);
			}
		}

		public override void OnGUI()
		{
			if (mMouseDragging)
			{
				GUI.color = dragRectColor;
				GUI.Box(dragRect, "", dragRectStyle);
				GUI.color = Color.white;
			}
		}

		public override void OnMouseMove()
		{
			if (base.handle.InUse())
			{
				return;
			}
			if (mMouseDragging)
			{
				if (pb_InputManager.IsKeyInUse() || pb_InputManager.IsMouseInUse())
				{
					mDragCanceled = true;
					mMouseDragging = false;
					return;
				}
				dragRect.x = Mathf.Min(mMouseOrigin.x, Input.mousePosition.x);
				dragRect.y = (float)Screen.height - Mathf.Max(mMouseOrigin.y, Input.mousePosition.y);
				dragRect.width = Mathf.Abs(mMouseOrigin.x - Input.mousePosition.x);
				dragRect.height = Mathf.Abs(mMouseOrigin.y - Input.mousePosition.y);
				UpdateDragPicker();
			}
			else if (mMouseIsDown && !mDragCanceled && Vector2.Distance(mMouseOrigin, Input.mousePosition) > 0.2f)
			{
				if (pb_Selection.activeGameObject != null)
				{
					Undo.RegisterState(new UndoSelection(), "Change Selection");
				}
				mMouseDragging = true;
				dragRect.x = mMouseOrigin.x;
				dragRect.y = (float)Screen.height - mMouseOrigin.y;
				dragRect.width = 0f;
				dragRect.height = 0f;
			}
		}

		public override void OnMouseDown()
		{
			if (!base.handle.InUse())
			{
				mMouseOrigin = Input.mousePosition;
				mDragCanceled = false;
				mMouseIsDown = true;
			}
		}

		public override void OnMouseUp()
		{
			if (base.handle.InUse())
			{
				return;
			}
			if (mMouseDragging || mDragCanceled)
			{
				mDragCanceled = false;
				mMouseDragging = false;
			}
			else
			{
				if (IsMouseInUse() || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
				{
					return;
				}
				if ((bool)hitInfo.collider.GetComponent<LevelEditorObject>())
				{
					Undo.RegisterState(new UndoSelection(), "Change Selection");
					if (!pb_InputExtension.Shift() && !pb_InputExtension.Control())
					{
						pb_Selection.SetSelection(hitInfo.collider.gameObject);
					}
					else
					{
						pb_Selection.AddToSelection(hitInfo.collider.gameObject);
					}
					return;
				}
				if (pb_Selection.activeGameObject != null)
				{
					Undo.RegisterState(new UndoSelection(), "Change Selection");
				}
				if (!pb_InputExtension.Shift() && !pb_InputExtension.Control())
				{
					pb_Selection.Clear();
				}
				if (pb_Selection.gameObjects.Count < 1)
				{
					base.handle.SetIsHidden(isHidden: true);
				}
			}
		}

		public override void OnKeyDown()
		{
			if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.D))
			{
				List<GameObject> list = new List<GameObject>();
				List<IUndo> list2 = new List<IUndo>
				{
					new UndoSelection()
				};
				foreach (GameObject gameObject2 in pb_Selection.gameObjects)
				{
					GameObject gameObject = pb_Scene.Instantiate(gameObject2);
					list.Add(gameObject);
					list2.Add(new UndoInstantiate(gameObject));
				}
				Undo.RegisterStates(list2, "Duplicate Object");
				pb_Selection.SetSelection(list);
			}
		}

		public override void OnSelectionChange(IEnumerable<GameObject> added)
		{
			if (pb_Selection.activeGameObject != null)
			{
				Transform transform = pb_Selection.activeGameObject.transform;
				base.handle.SetTRS(transform.position, transform.localRotation, Vector3.one);
				base.handle.SetIsHidden(isHidden: false);
			}
			else
			{
				base.handle.SetIsHidden(isHidden: true);
			}
		}

		public override void OnFrameSelection()
		{
			if (pb_Selection.activeGameObject != null)
			{
				pb_SceneCamera.Focus(pb_Selection.activeGameObject);
			}
			else
			{
				pb_SceneCamera.Focus(Vector3.zero);
			}
		}

		public override bool EnableCameraControls()
		{
			if (!base.EnableCameraControls())
			{
				return Input.GetKey(KeyCode.Q);
			}
			return true;
		}

		private void UpdateDragPicker()
		{
			Rect rect = new Rect(dragRect.x, (float)Screen.height - (dragRect.y + dragRect.height), dragRect.width, dragRect.height);
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject item in pb_Scene.Children())
			{
				Vector2 point = Camera.main.WorldToScreenPoint(item.transform.position);
				if (rect.Contains(point))
				{
					list.Add(item);
				}
			}
			pb_Selection.SetSelection(list);
		}
	}
}
