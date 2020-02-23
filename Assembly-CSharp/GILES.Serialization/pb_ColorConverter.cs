using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_ColorConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject jObject = new JObject();
			jObject.Add("$type", value.GetType().AssemblyQualifiedName);
			if (value is Color)
			{
				jObject.Add("r", ((Color)value).r);
				jObject.Add("g", ((Color)value).g);
				jObject.Add("b", ((Color)value).b);
				jObject.Add("a", ((Color)value).a);
			}
			else
			{
				jObject.Add("r", ((Color32)value).r);
				jObject.Add("g", ((Color32)value).g);
				jObject.Add("b", ((Color32)value).b);
				jObject.Add("a", ((Color32)value).a);
			}
			jObject.WriteTo(writer, serializer.Converters.ToArray());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			if (typeof(Color32).IsAssignableFrom(objectType))
			{
				return new Color32((byte)jObject.GetValue("r"), (byte)jObject.GetValue("g"), (byte)jObject.GetValue("b"), (byte)jObject.GetValue("a"));
			}
			return new Color((float)jObject.GetValue("r"), (float)jObject.GetValue("g"), (float)jObject.GetValue("b"), (float)jObject.GetValue("a"));
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Color).IsAssignableFrom(objectType);
		}
	}
}
