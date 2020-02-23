using System;
using System.Collections.Generic;
using UnityEngine;

namespace GILES.Interface
{
	public static class pb_ComponentEditorResolver
	{
		private static readonly Dictionary<Type, Type> builtInComponentEditors = new Dictionary<Type, Type>
		{
			{
				typeof(Transform),
				typeof(pb_TransformEditor)
			},
			{
				typeof(MeshRenderer),
				typeof(pb_MeshRendererEditor)
			},
			{
				typeof(Camera),
				typeof(pb_CameraEditor)
			}
		};

		public static pb_ComponentEditor GetEditor(Component component)
		{
			GameObject gameObject = new GameObject();
			Type value = null;
			if (!builtInComponentEditors.TryGetValue(component.GetType(), out value))
			{
				foreach (KeyValuePair<Type, Type> builtInComponentEditor in builtInComponentEditors)
				{
					if (builtInComponentEditor.Key.IsAssignableFrom(component.GetType()))
					{
						value = builtInComponentEditor.Value;
						break;
					}
				}
			}
			gameObject.name = component.name;
			pb_ComponentEditor obj = (pb_ComponentEditor)gameObject.AddComponent(value ?? typeof(pb_ComponentEditor));
			obj.SetComponent(component);
			return obj;
		}
	}
}
