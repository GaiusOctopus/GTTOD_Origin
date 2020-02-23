using GILES.Serialization;
using UnityEngine;

namespace GILES
{
	public static class pb_ComponentExtension
	{
		public static Component AddComponent(this GameObject go, pb_ISerializable component)
		{
			if (!typeof(Component).IsAssignableFrom(component.type))
			{
				Debug.LogError(component.type + " does not inherit UnityEngine.Component!");
				return null;
			}
			Component component2 = go.AddComponent(component.type);
			component.ApplyProperties(component2);
			return component2;
		}

		public static T DemandComponent<T>(this GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if ((Object)val == (Object)null)
			{
				val = go.AddComponent<T>();
			}
			return val;
		}
	}
}
