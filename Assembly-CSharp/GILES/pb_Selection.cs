using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public class pb_Selection : pb_MonoBehaviourSingleton<pb_Selection>
	{
		private List<GameObject> _gameObjects = new List<GameObject>();

		public static List<GameObject> gameObjects => pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects;

		public static GameObject activeGameObject
		{
			get
			{
				if (pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects.Count <= 0)
				{
					return null;
				}
				return pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects[0];
			}
		}

		public event Callback<IEnumerable<GameObject>> OnSelectionChange;

		public event Callback<IEnumerable<GameObject>> OnRemovedFromSelection;

		protected override void Awake()
		{
			base.Awake();
			Undo.AddUndoPerformedListener(UndoRedoPerformed);
			Undo.AddRedoPerformedListener(UndoRedoPerformed);
		}

		public static void AddOnSelectionChangeListener(Callback<IEnumerable<GameObject>> del)
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange += del;
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange = del;
			}
		}

		public static void AddOnRemovedFromSelectionListener(Callback<IEnumerable<GameObject>> del)
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection += del;
			}
			else
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection = del;
			}
		}

		public static void Clear()
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects.Count > 0 && pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection(pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects);
			}
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance._Clear() > 0 && pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(null);
			}
		}

		public static void SetSelection(IEnumerable<GameObject> selection)
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection(pb_MonoBehaviourSingleton<pb_Selection>.instance._gameObjects);
			}
			pb_MonoBehaviourSingleton<pb_Selection>.instance._Clear();
			foreach (GameObject item in selection)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance._AddToSelection(item);
			}
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(selection);
			}
		}

		public static void SetSelection(GameObject selection)
		{
			SetSelection(new List<GameObject>
			{
				selection
			});
		}

		public static void AddToSelection(GameObject go)
		{
			pb_MonoBehaviourSingleton<pb_Selection>.instance._AddToSelection(go);
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(new List<GameObject>
				{
					go
				});
			}
		}

		public static void RemoveFromSelection(GameObject go)
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance._RemoveFromSelection(go))
			{
				if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection != null)
				{
					pb_MonoBehaviourSingleton<pb_Selection>.instance.OnRemovedFromSelection(new List<GameObject>
					{
						go
					});
				}
				if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
				{
					pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(null);
				}
			}
		}

		private static void UndoRedoPerformed()
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(null);
			}
		}

		public static void OnExternalUpdate()
		{
			if (pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange != null)
			{
				pb_MonoBehaviourSingleton<pb_Selection>.instance.OnSelectionChange(null);
			}
		}

		private void _InitializeSelected(GameObject go)
		{
			go.AddComponent<pb_SelectionHighlight>();
		}

		private void _DeinitializeSelected(GameObject go)
		{
			pb_SelectionHighlight component = go.GetComponent<pb_SelectionHighlight>();
			if (component != null)
			{
				pb_ObjectUtility.Destroy(component);
			}
		}

		private bool _AddToSelection(GameObject go)
		{
			if (go != null && !_gameObjects.Contains(go))
			{
				_InitializeSelected(go);
				_gameObjects.Add(go);
				return true;
			}
			return false;
		}

		private bool _RemoveFromSelection(GameObject go)
		{
			if (go != null && _gameObjects.Contains(go))
			{
				pb_ObjectUtility.Destroy(go.GetComponent<pb_SelectionHighlight>());
				_gameObjects.Remove(go);
				return true;
			}
			return false;
		}

		private int _Clear()
		{
			int count = _gameObjects.Count;
			for (int i = 0; i < count; i++)
			{
				_DeinitializeSelected(_gameObjects[i]);
			}
			_gameObjects.Clear();
			return count;
		}
	}
}
