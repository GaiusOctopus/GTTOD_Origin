using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_ComponentEditor : MonoBehaviour
	{
		protected Component target;

		public static readonly HashSet<string> ignoreProperties = new HashSet<string>
		{
			"tag",
			"name",
			"hideFlags",
			"useGUILayout"
		};

		public void SetComponent(Component target)
		{
			foreach (Transform item in base.transform)
			{
				pb_ObjectUtility.Destroy(item.gameObject);
			}
			this.target = target;
			InitializeGUI();
		}

		public virtual void UpdateGUI()
		{
			pb_TypeInspector[] componentsInChildren = base.gameObject.GetComponentsInChildren<pb_TypeInspector>(includeInactive: false);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].UpdateGUI();
			}
		}

		protected virtual void InitializeGUI()
		{
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject);
			foreach (PropertyInfo serializableProperty in pb_Reflection.GetSerializableProperties(target.GetType(), BindingFlags.Instance | BindingFlags.Public))
			{
				if (!ignoreProperties.Contains(serializableProperty.Name) && Attribute.GetCustomAttribute(serializableProperty, typeof(pb_InspectorIgnoreAttribute)) == null)
				{
					pb_InspectorResolver.AddTypeInspector(target, base.transform, serializableProperty).onTypeInspectorSetValue = OnTypeInspectorSetValue;
				}
			}
			FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (Attribute.GetCustomAttribute(fieldInfo, typeof(pb_InspectorIgnoreAttribute)) == null)
				{
					pb_InspectorResolver.AddTypeInspector(target, base.transform, null, fieldInfo).onTypeInspectorSetValue = OnTypeInspectorSetValue;
				}
			}
		}

		private void OnTypeInspectorSetValue()
		{
			pb_Gizmo[] components = target.gameObject.GetComponents<pb_Gizmo>();
			foreach (pb_Gizmo pb_Gizmo in components)
			{
				if (pb_Gizmo.CanEditType(target.GetType()))
				{
					pb_Gizmo.OnComponentModified();
				}
			}
		}
	}
}
