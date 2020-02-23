using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_VectorConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject jObject = new JObject();
			jObject.Add("$type", value.GetType().AssemblyQualifiedName);
			if (value is Vector2)
			{
				jObject.Add("x", ((Vector2)value).x);
				jObject.Add("y", ((Vector2)value).y);
			}
			else if (value is Vector3)
			{
				jObject.Add("x", ((Vector3)value).x);
				jObject.Add("y", ((Vector3)value).y);
				jObject.Add("z", ((Vector3)value).z);
			}
			else if (value is Vector4)
			{
				jObject.Add("x", ((Vector4)value).x);
				jObject.Add("y", ((Vector4)value).y);
				jObject.Add("z", ((Vector4)value).z);
				jObject.Add("w", ((Vector4)value).w);
			}
			else if (value is Quaternion)
			{
				jObject.Add("x", ((Quaternion)value).x);
				jObject.Add("y", ((Quaternion)value).y);
				jObject.Add("z", ((Quaternion)value).z);
				jObject.Add("w", ((Quaternion)value).w);
			}
			jObject.WriteTo(writer, serializer.Converters.ToArray());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			if (objectType == typeof(Vector2))
			{
				return new Vector2((float)jObject.GetValue("x"), (float)jObject.GetValue("y"));
			}
			if (objectType == typeof(Vector3))
			{
				return new Vector3((float)jObject.GetValue("x"), (float)jObject.GetValue("y"), (float)jObject.GetValue("z"));
			}
			if (objectType == typeof(Vector4))
			{
				return new Vector4((float)jObject.GetValue("x"), (float)jObject.GetValue("y"), (float)jObject.GetValue("z"), (float)jObject.GetValue("w"));
			}
			if (objectType == typeof(Quaternion))
			{
				return new Quaternion((float)jObject.GetValue("x"), (float)jObject.GetValue("y"), (float)jObject.GetValue("z"), (float)jObject.GetValue("w"));
			}
			return new Vector3(0f, 0f, 0f);
		}

		public override bool CanConvert(Type objectType)
		{
			if (!typeof(Vector2).IsAssignableFrom(objectType))
			{
				return typeof(Quaternion).IsAssignableFrom(objectType);
			}
			return true;
		}
	}
}
