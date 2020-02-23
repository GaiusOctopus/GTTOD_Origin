using GILES.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GILES
{
	public static class pb_Reflection
	{
		public static readonly HashSet<Type> IgnoreAttributes = new HashSet<Type>
		{
			typeof(ObsoleteAttribute),
			typeof(pb_JsonIgnoreAttribute)
		};

		public static IEnumerable<PropertyInfo> GetSerializableProperties(Type type, BindingFlags flags)
		{
			return from x in type.GetProperties(flags)
				where x.CanWrite && ((flags & BindingFlags.Public) == 0 || x.GetSetMethod() != null) && !x.IsSpecialName && !HasIgnoredAttribute(x)
				select x;
		}

		public static IEnumerable<FieldInfo> GetSerializableFields(Type type, BindingFlags flags)
		{
			return from x in type.GetFields(flags)
				where ((flags & BindingFlags.Public) == 0 || !x.IsPrivate) && !HasIgnoredAttribute(x)
				select x;
		}

		public static Dictionary<string, object> ReflectProperties<T>(T obj)
		{
			return ReflectProperties(obj, BindingFlags.Instance | BindingFlags.Public, null);
		}

		public static Dictionary<string, object> ReflectProperties<T>(T obj, HashSet<string> ignoreFields)
		{
			return ReflectProperties(obj, BindingFlags.Instance | BindingFlags.Public, ignoreFields);
		}

		public static Dictionary<string, object> ReflectProperties<T>(T obj, BindingFlags flags, HashSet<string> ignoreFields)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Type type = obj.GetType();
			foreach (PropertyInfo serializableProperty in GetSerializableProperties(type, flags))
			{
				try
				{
					if (serializableProperty.CanWrite && !serializableProperty.IsSpecialName && !HasIgnoredAttribute(serializableProperty) && (ignoreFields == null || !ignoreFields.Contains(serializableProperty.Name)))
					{
						ParameterInfo[] indexParameters = serializableProperty.GetIndexParameters();
						if (indexParameters == null || indexParameters.Length == 0)
						{
							string name = serializableProperty.Name;
							object obj2 = obj;
							object[] index = indexParameters;
							dictionary.Add(name, serializableProperty.GetValue(obj2, index));
						}
					}
				}
				catch
				{
				}
			}
			FieldInfo[] fields = type.GetFields(flags);
			foreach (FieldInfo fieldInfo in fields)
			{
				try
				{
					if (!HasIgnoredAttribute(fieldInfo) && (ignoreFields == null || !ignoreFields.Contains(fieldInfo.Name)))
					{
						dictionary.Add(fieldInfo.Name, fieldInfo.GetValue(obj));
					}
				}
				catch
				{
					Debug.LogError("Failed extracting property: " + fieldInfo.Name);
				}
			}
			return dictionary;
		}

		public static T GetValue<T>(object obj, string name)
		{
			return GetValue<T>(obj, name, BindingFlags.Instance | BindingFlags.Public);
		}

		public static T GetValue<T>(object obj, string name, BindingFlags flags)
		{
			if (obj == null)
			{
				return default(T);
			}
			PropertyInfo property = obj.GetType().GetProperty(name, flags);
			if (property != null)
			{
				return (T)property.GetValue(obj, null);
			}
			FieldInfo field = obj.GetType().GetField(name, flags);
			if (field != null)
			{
				return (T)field.GetValue(obj);
			}
			return default(T);
		}

		public static bool SetValue(object obj, string name, object value)
		{
			return SetValue(obj, name, value, BindingFlags.Instance | BindingFlags.Public);
		}

		public static bool SetValue(object obj, string name, object value, BindingFlags flags)
		{
			if (obj == null)
			{
				return false;
			}
			PropertyInfo property = obj.GetType().GetProperty(name, flags);
			if (property != null)
			{
				return SetPropertyValue(obj, property, value);
			}
			FieldInfo field = obj.GetType().GetField(name, flags);
			if (field != null)
			{
				return SetFieldValue(obj, field, value);
			}
			return false;
		}

		public static bool SetPropertyValue(object target, PropertyInfo propertyInfo, object value)
		{
			if (propertyInfo == null || target == null)
			{
				return false;
			}
			try
			{
				object obj = (value is JToken) ? ((JToken)value).ToObject(propertyInfo.PropertyType, pb_Serialization.Serializer) : value;
				if (propertyInfo.PropertyType.IsEnum)
				{
					int num = (int)Convert.ChangeType(obj, typeof(int));
					propertyInfo.SetValue(target, num, null);
				}
				else if (obj is pb_ObjectWrapper)
				{
					object value2 = Convert.ChangeType(((pb_ObjectWrapper)obj).GetValue(), propertyInfo.PropertyType);
					propertyInfo.SetValue(target, value2, null);
				}
				else
				{
					object value3 = Convert.ChangeType(obj, propertyInfo.PropertyType);
					propertyInfo.SetValue(target, value3, null);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool SetFieldValue(object target, FieldInfo field, object value)
		{
			if (target == null || field == null)
			{
				return false;
			}
			try
			{
				object obj = (value is JToken) ? ((JToken)value).ToObject(field.FieldType, pb_Serialization.Serializer) : value;
				if (field.FieldType.IsEnum)
				{
					int num = (int)Convert.ChangeType(obj, typeof(int));
					field.SetValue(target, num);
				}
				else if (obj is pb_ObjectWrapper)
				{
					object value2 = Convert.ChangeType(((pb_ObjectWrapper)obj).GetValue(), field.FieldType);
					field.SetValue(target, value2);
				}
				else
				{
					object value3 = Convert.ChangeType(obj, field.FieldType);
					field.SetValue(target, value3);
				}
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.ToString());
				return false;
			}
		}

		public static void ApplyProperties<T>(T obj, Dictionary<string, object> properties)
		{
			foreach (KeyValuePair<string, object> property in properties)
			{
				SetValue(obj, property.Key, property.Value);
			}
		}

		public static bool HasIgnoredAttribute(Type type)
		{
			return type.GetCustomAttributes(inherit: true).Any((object x) => IgnoreAttributes.Contains(x.GetType()));
		}

		public static bool HasIgnoredAttribute<T>(T info) where T : MemberInfo
		{
			return info.GetCustomAttributes(inherit: true).Any((object x) => IgnoreAttributes.Contains(x.GetType()));
		}

		public static IEnumerable<Attribute> GetAttributes<T>(T obj)
		{
			return (IEnumerable<Attribute>)typeof(T).GetCustomAttributes(inherit: true);
		}
	}
}
