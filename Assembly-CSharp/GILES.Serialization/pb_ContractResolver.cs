using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_ContractResolver : DefaultContractResolver
	{
		private static Dictionary<Type, JsonConverter> converters = new Dictionary<Type, JsonConverter>();

		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
			jsonProperty.ShouldSerialize = delegate(object instance)
			{
				try
				{
					if (pb_Reflection.HasIgnoredAttribute(member))
					{
						return false;
					}
					if (member is PropertyInfo)
					{
						PropertyInfo propertyInfo = (PropertyInfo)member;
						if (propertyInfo.CanRead && propertyInfo.CanWrite && !propertyInfo.IsSpecialName)
						{
							propertyInfo.GetValue(instance, null);
							return true;
						}
					}
					else if (member is FieldInfo)
					{
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogWarning("Can't create field \"" + member.Name + "\" " + member.DeclaringType + " -> " + member.ReflectedType + "\n\n" + ex.ToString());
				}
				return false;
			};
			return jsonProperty;
		}

		protected override JsonConverter ResolveContractConverter(Type type)
		{
			if (typeof(Color).IsAssignableFrom(type) || typeof(Color32).IsAssignableFrom(type))
			{
				return GetConverter<pb_ColorConverter>();
			}
			if (typeof(Matrix4x4).IsAssignableFrom(type))
			{
				return GetConverter<pb_MatrixConverter>();
			}
			if (typeof(Vector2).IsAssignableFrom(type) || typeof(Vector3).IsAssignableFrom(type) || typeof(Vector4).IsAssignableFrom(type) || typeof(Quaternion).IsAssignableFrom(type))
			{
				return GetConverter<pb_VectorConverter>();
			}
			if (typeof(Mesh).IsAssignableFrom(type))
			{
				return GetConverter<pb_MeshConverter>();
			}
			if (typeof(Material).IsAssignableFrom(type))
			{
				return GetConverter<pb_MaterialConverter>();
			}
			return base.ResolveContractConverter(type);
		}

		private static T GetConverter<T>() where T : JsonConverter, new()
		{
			if (converters.TryGetValue(typeof(T), out JsonConverter value))
			{
				return (T)value;
			}
			value = new T();
			converters.Add(typeof(T), value);
			return (T)value;
		}
	}
}
