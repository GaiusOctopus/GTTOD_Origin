using System;
using UnityEngine;

namespace GILES
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class pb_EditorComponentAttribute : Attribute
	{
		public static void StripEditorComponents(GameObject target)
		{
			Component[] components = target.GetComponents<Component>();
			foreach (Component component in components)
			{
				if (component != null && Attribute.GetCustomAttribute(component.GetType(), typeof(pb_EditorComponentAttribute)) != null)
				{
					pb_ObjectUtility.Destroy(component);
				}
			}
		}
	}
}
