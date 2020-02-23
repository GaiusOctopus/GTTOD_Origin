using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GILES
{
	[Serializable]
	public class pb_GizmoManager : pb_MonoBehaviourSingleton<pb_GizmoManager>
	{
		private static readonly Type[] BuiltinGizmos = new Type[2]
		{
			typeof(pb_Gizmo_Light),
			typeof(pb_Gizmo_Camera)
		};

		private static Dictionary<Type, Type> gizmoLookup = null;

		private static void RebuildGizmoLookup()
		{
			gizmoLookup = new Dictionary<Type, Type>();
			Type[] builtinGizmos = BuiltinGizmos;
			foreach (Type type in builtinGizmos)
			{
				pb_GizmoAttribute pb_GizmoAttribute = (pb_GizmoAttribute)((IEnumerable<Attribute>)type.GetCustomAttributes(inherit: true)).FirstOrDefault((Attribute x) => x is pb_GizmoAttribute);
				if (pb_GizmoAttribute != null)
				{
					gizmoLookup.Add(pb_GizmoAttribute.type, type);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			pb_Scene.AddOnObjectInstantiatedListener(OnObjectInstantiated);
			pb_Scene.AddOnLevelLoadedListener(OnLevelLoaded);
			pb_Selection.AddOnSelectionChangeListener(OnSelectionChange);
			pb_Selection.AddOnRemovedFromSelectionListener(OnRemovedFromSelection);
		}

		private void OnObjectInstantiated(GameObject go)
		{
			AssociateGizmos(go);
		}

		private void OnSelectionChange(IEnumerable<GameObject> selection)
		{
			SetIsSelected(selection, isSelected: true);
		}

		private void OnRemovedFromSelection(IEnumerable<GameObject> selection)
		{
			SetIsSelected(selection, isSelected: false);
		}

		private void SetIsSelected(IEnumerable<GameObject> selection, bool isSelected)
		{
			if (selection != null)
			{
				foreach (GameObject item in selection)
				{
					pb_Gizmo[] componentsInChildren = item.GetComponentsInChildren<pb_Gizmo>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].isSelected = isSelected;
					}
				}
			}
		}

		private void OnLevelLoaded()
		{
			foreach (GameObject item in pb_Scene.Children())
			{
				pb_Gizmo pb_Gizmo = AssociateGizmos(item);
				if (pb_Gizmo != null && pb_Selection.gameObjects != null && pb_Selection.gameObjects.Contains(item))
				{
					pb_Gizmo.isSelected = true;
				}
			}
		}

		private pb_Gizmo AssociateGizmos(GameObject go)
		{
			pb_Gizmo[] components = go.GetComponents<pb_Gizmo>();
			for (int i = 0; i < components.Length; i++)
			{
				pb_ObjectUtility.Destroy(components[i]);
			}
			Component[] componentsInChildren = go.GetComponentsInChildren<Component>();
			foreach (Component component in componentsInChildren)
			{
				if (!(component == null))
				{
					Type type = FindGizmoForType(component.GetType());
					if (type != null)
					{
						return (pb_Gizmo)go.AddComponent(type);
					}
				}
			}
			return null;
		}

		public static Type FindGizmoForType(Type type)
		{
			if (gizmoLookup == null)
			{
				RebuildGizmoLookup();
			}
			Type value = null;
			gizmoLookup.TryGetValue(type, out value);
			return value;
		}
	}
}
