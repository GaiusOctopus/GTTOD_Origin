using GILES.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_Inspector : MonoBehaviour
	{
		public GameObject inspectorScrollPanel;

		private GameObject currentSelection;

		public bool showUnityComponents;

		private List<pb_ComponentEditor> componentEditors = new List<pb_ComponentEditor>();

		private static HashSet<Type> userIgnoredTypes = new HashSet<Type>();

		public static void AddIgnoredType(Type type)
		{
			userIgnoredTypes.Add(type);
		}

		public static void RemoveIgnoredType(Type type)
		{
			userIgnoredTypes.Remove(type);
		}

		private void Start()
		{
			pb_Selection.AddOnSelectionChangeListener(OnSelectionChange);
			Undo.AddUndoPerformedListener(UndoRedoPerformed);
			Undo.AddRedoPerformedListener(UndoRedoPerformed);
		}

		private void UndoRedoPerformed()
		{
			foreach (pb_ComponentEditor componentEditor in componentEditors)
			{
				componentEditor.UpdateGUI();
			}
		}

		private void OnSelectionChange(IEnumerable<GameObject> selection)
		{
			if (currentSelection != pb_Selection.activeGameObject)
			{
				RebuildInspector(pb_Selection.activeGameObject);
				currentSelection = pb_Selection.activeGameObject;
			}
		}

		public void RebuildInspector(GameObject go)
		{
			ClearInspector();
			if (go == null)
			{
				return;
			}
			Component[] components = go.GetComponents<Component>();
			foreach (Component component in components)
			{
				if (component == null || userIgnoredTypes.Contains(component.GetType()) || pb_Reflection.HasIgnoredAttribute(component.GetType()) || Attribute.GetCustomAttribute(component.GetType(), typeof(pb_InspectorIgnoreAttribute)) != null || (!showUnityComponents && pb_Config.IgnoredComponentsInInspector.Contains(component.GetType())))
				{
					continue;
				}
				string label = component.GetType().ToString();
				if (component.GetType().GetCustomAttributes(typeof(pb_InspectorNameAttribute), inherit: true).Length != 0)
				{
					pb_InspectorNameAttribute pb_InspectorNameAttribute = (pb_InspectorNameAttribute)component.GetType().GetCustomAttributes(typeof(pb_InspectorNameAttribute), inherit: true)[0];
					if (pb_InspectorNameAttribute.name != null && pb_InspectorNameAttribute.name.Length > 0)
					{
						label = pb_InspectorNameAttribute.name;
					}
				}
				GameObject gameObject = pb_GUIUtility.CreateLabeledVerticalPanel(label);
				gameObject.transform.SetParent(inspectorScrollPanel.transform);
				pb_ComponentEditor pb_ComponentEditor = null;
				pb_ComponentEditor = ((!typeof(pb_ICustomEditor).IsAssignableFrom(component.GetType())) ? pb_ComponentEditorResolver.GetEditor(component) : ((pb_ICustomEditor)component).InstantiateInspector(component));
				pb_ComponentEditor.transform.SetParent(gameObject.transform);
				componentEditors.Add(pb_ComponentEditor);
			}
		}

		private void ClearInspector()
		{
			foreach (Transform item in inspectorScrollPanel.transform)
			{
				pb_ObjectUtility.Destroy(item.gameObject);
			}
			componentEditors.Clear();
		}

		public void ToggleInspector(bool show)
		{
		}
	}
}
