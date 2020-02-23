using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GILES.Serialization
{
	[Serializable]
	public class pb_SerializableObject<T> : pb_ISerializable, ISerializable
	{
		protected T target;

		protected Dictionary<string, object> reflectedProperties;

		public Type type
		{
			get;
			set;
		}

		public pb_SerializableObject(T obj)
		{
			target = obj;
		}

		public static explicit operator T(pb_SerializableObject<T> obj)
		{
			if (obj.target == null)
			{
				T val = default(T);
				obj.ApplyProperties(val);
				return val;
			}
			return obj.target;
		}

		public pb_SerializableObject(SerializationInfo info, StreamingContext context)
		{
			string typeName = (string)info.GetValue("typeName", typeof(string));
			type = Type.GetType(typeName);
			reflectedProperties = (Dictionary<string, object>)info.GetValue("reflectedProperties", typeof(Dictionary<string, object>));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			Type type = target.GetType();
			info.AddValue("typeName", type.AssemblyQualifiedName, typeof(string));
			reflectedProperties = PopulateSerializableDictionary();
			info.AddValue("reflectedProperties", reflectedProperties, typeof(Dictionary<string, object>));
		}

		public virtual void ApplyProperties(object obj)
		{
			pb_ISerializableComponent pb_ISerializableComponent = obj as pb_ISerializableComponent;
			if (pb_ISerializableComponent != null)
			{
				pb_ISerializableComponent.ApplyDictionaryValues(reflectedProperties);
			}
			else
			{
				pb_Reflection.ApplyProperties(obj, reflectedProperties);
			}
		}

		public virtual Dictionary<string, object> PopulateSerializableDictionary()
		{
			pb_ISerializableComponent pb_ISerializableComponent = target as pb_ISerializableComponent;
			if (pb_ISerializableComponent != null)
			{
				return pb_ISerializableComponent.PopulateSerializableDictionary();
			}
			return pb_Reflection.ReflectProperties(target);
		}
	}
}
