using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GILES.Serialization
{
	public abstract class pb_UnityTypeConverter<T> : JsonConverter
	{
		public sealed override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("$type");
			writer.WriteValue(typeof(pb_ObjectContainer<T>).AssemblyQualifiedName);
			writer.WritePropertyName("value");
			WriteObjectJson(writer, value, serializer);
			writer.WriteEndObject();
		}

		public abstract void WriteObjectJson(JsonWriter writer, object value, JsonSerializer serializer);

		public sealed override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			JProperty jProperty = jObject.Property("$type");
			if (jProperty != null && Type.GetType(jProperty.Value.ToString()) == typeof(pb_ObjectContainer<T>))
			{
				return ((pb_ObjectContainer<T>)jObject.ToObject(typeof(pb_ObjectContainer<T>), serializer)).value;
			}
			return ReadJsonObject(jObject, objectType, existingValue, serializer);
		}

		public abstract object ReadJsonObject(JObject obj, Type objectType, object existingValue, JsonSerializer serializer);

		public sealed override bool CanConvert(Type objectType)
		{
			if (!typeof(T).IsAssignableFrom(objectType))
			{
				return typeof(pb_ObjectContainer<T>).IsAssignableFrom(objectType);
			}
			return true;
		}
	}
}
