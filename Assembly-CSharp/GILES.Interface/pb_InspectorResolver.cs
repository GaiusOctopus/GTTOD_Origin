using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GILES.Interface
{
	public static class pb_InspectorResolver
	{
		public const string TYPE_INSPECTOR_PATH = "Required/GUI/TypeInspector";

		private static Dictionary<Type, GameObject> inspectorPool = new Dictionary<Type, GameObject>();

		private static Dictionary<IEnumerable<pb_TypeInspectorAttribute>, GameObject> inspectorLookup = null;

		private static void InitializeLookup()
		{
			inspectorPool = new Dictionary<Type, GameObject>();
			inspectorLookup = new Dictionary<IEnumerable<pb_TypeInspectorAttribute>, GameObject>();
			UnityEngine.Object[] array = Resources.LoadAll("Required/GUI/TypeInspector", typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject gameObject = (GameObject)array[i];
				pb_TypeInspector component = gameObject.GetComponent<pb_TypeInspector>();
				if (!(component == null))
				{
					IEnumerable<Attribute> source = (IEnumerable<Attribute>)component.GetType().GetCustomAttributes(inherit: true);
					inspectorLookup.Add(source.Where((Attribute x) => x != null && x is pb_TypeInspectorAttribute).Cast<pb_TypeInspectorAttribute>(), gameObject);
				}
			}
		}

		public static pb_TypeInspector GetInspector(Type type)
		{
			if (inspectorLookup == null)
			{
				InitializeLookup();
			}
			if (inspectorPool.TryGetValue(type, out GameObject value))
			{
				return UnityEngine.Object.Instantiate(value).GetComponent<pb_TypeInspector>();
			}
			List<GameObject> list = new List<GameObject>();
			foreach (KeyValuePair<IEnumerable<pb_TypeInspectorAttribute>, GameObject> item in inspectorLookup)
			{
				foreach (pb_TypeInspectorAttribute item2 in item.Key)
				{
					if (!item2.CanEditType(type))
					{
						continue;
					}
					if (!(item2.type == type))
					{
						list.Add(item.Value);
						continue;
					}
					list.Insert(0, item.Value);
					goto IL_00c1;
				}
			}
			goto IL_00c1;
			IL_00c1:
			if (list.Count > 0)
			{
				inspectorPool.Add(type, list[0]);
				value = UnityEngine.Object.Instantiate(list[0]);
				pb_TypeInspector component = value.GetComponent<pb_TypeInspector>();
				component.SetDeclaringType(type);
				return component;
			}
			pb_ObjectInspector pb_ObjectInspector = new GameObject
			{
				name = "Generic Object Inspector: " + type
			}.AddComponent<pb_ObjectInspector>();
			pb_ObjectInspector.SetDeclaringType(type);
			return pb_ObjectInspector;
		}

		public static pb_TypeInspector AddTypeInspector(object target, Transform parentTransform, PropertyInfo property = null, FieldInfo field = null)
		{
			pb_TypeInspector pb_TypeInspector = null;
			pb_TypeInspector = GetInspector((property != null) ? property.PropertyType : field.FieldType);
			if (pb_TypeInspector != null)
			{
				if (property != null)
				{
					pb_TypeInspector.Initialize(target, property);
				}
				else
				{
					pb_TypeInspector.Initialize(target, field);
				}
				pb_TypeInspector.transform.SetParent(parentTransform);
			}
			else
			{
				Debug.LogError("No inspector found!  Is `pb_ObjectInspector.cs` missing?");
			}
			return pb_TypeInspector;
		}
	}
}
