using System;
using System.Runtime.Serialization;

namespace GILES.Serialization
{
	public class pb_ObjectContainer<T> : ISerializable, pb_ObjectWrapper
	{
		public T value;

		public pb_ObjectContainer(T value)
		{
			this.value = value;
		}

		public static implicit operator T(pb_ObjectContainer<T> container)
		{
			return container.value;
		}

		public new Type GetType()
		{
			return typeof(T);
		}

		public object GetValue()
		{
			return value;
		}

		public pb_ObjectContainer(SerializationInfo info, StreamingContext context)
		{
			value = (T)info.GetValue("value", typeof(T));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", value, typeof(T));
		}

		public override string ToString()
		{
			return "Container: " + value.ToString();
		}
	}
}
