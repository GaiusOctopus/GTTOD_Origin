using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_ComponentDiff : ISerializable
	{
		public Dictionary<Component, Dictionary<string, object>> modifiedValues;

		private List<Type> keys;

		private List<Dictionary<string, object>> values;

		public pb_ComponentDiff()
		{
			modifiedValues = new Dictionary<Component, Dictionary<string, object>>();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (modifiedValues == null)
			{
				modifiedValues = new Dictionary<Component, Dictionary<string, object>>();
			}
			List<Type> value = modifiedValues.Keys.Select((Component x) => x.GetType()).ToList();
			info.AddValue("components", value, typeof(List<Type>));
			info.AddValue("values", modifiedValues.Values.ToList(), typeof(List<Dictionary<string, object>>));
		}

		public pb_ComponentDiff(SerializationInfo info, StreamingContext context)
		{
			modifiedValues = new Dictionary<Component, Dictionary<string, object>>();
			keys = (List<Type>)info.GetValue("components", typeof(List<Type>));
			values = (List<Dictionary<string, object>>)info.GetValue("values", typeof(List<Dictionary<string, object>>));
		}

		public static void AddDiff(Component component, string name, object value)
		{
			GameObject gameObject = component.gameObject;
			pb_MetaDataComponent pb_MetaDataComponent = gameObject.GetComponent<pb_MetaDataComponent>();
			if (pb_MetaDataComponent == null)
			{
				pb_MetaDataComponent = gameObject.AddComponent<pb_MetaDataComponent>();
			}
			pb_ComponentDiff componentDiff = pb_MetaDataComponent.metadata.componentDiff;
			if (componentDiff.modifiedValues.TryGetValue(component, out Dictionary<string, object> value2))
			{
				if (value2.ContainsKey(name))
				{
					value2[name] = value;
				}
				else
				{
					value2.Add(name, value);
				}
			}
			else
			{
				componentDiff.modifiedValues.Add(component, new Dictionary<string, object>
				{
					{
						name,
						value
					}
				});
			}
		}

		public void ApplyPatch(GameObject target)
		{
			if (keys.Count < 1 || values.Count != keys.Count)
			{
				return;
			}
			modifiedValues = new Dictionary<Component, Dictionary<string, object>>();
			int count = keys.Count;
			Dictionary<Type, int> dictionary = new Dictionary<Type, int>();
			for (int i = 0; i < count; i++)
			{
				Component[] components = target.GetComponents(keys[i]);
				if (dictionary.ContainsKey(keys[i]))
				{
					dictionary[keys[i]]++;
				}
				else
				{
					dictionary.Add(keys[i], 0);
				}
				int num = Math.Min(dictionary[keys[i]], components.Length - 1);
				modifiedValues.Add(components[num], values[i]);
				foreach (KeyValuePair<string, object> item in values[i])
				{
					pb_Reflection.SetValue(components[num], item.Key, item.Value);
				}
			}
		}
	}
}
