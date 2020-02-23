using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MatrixConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject obj = new JObject
			{
				{
					"$type",
					value.GetType().AssemblyQualifiedName
				}
			};
			Matrix4x4 matrix4x = (Matrix4x4)value;
			obj.Add("00", matrix4x[0, 0]);
			obj.Add("01", matrix4x[0, 1]);
			obj.Add("02", matrix4x[0, 2]);
			obj.Add("03", matrix4x[0, 3]);
			obj.Add("10", matrix4x[1, 0]);
			obj.Add("11", matrix4x[1, 1]);
			obj.Add("12", matrix4x[1, 2]);
			obj.Add("13", matrix4x[1, 3]);
			obj.Add("20", matrix4x[2, 0]);
			obj.Add("21", matrix4x[2, 1]);
			obj.Add("22", matrix4x[2, 2]);
			obj.Add("23", matrix4x[2, 3]);
			obj.Add("30", matrix4x[3, 0]);
			obj.Add("31", matrix4x[3, 1]);
			obj.Add("32", matrix4x[3, 2]);
			obj.Add("33", matrix4x[3, 3]);
			obj.WriteTo(writer, serializer.Converters.ToArray());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			Matrix4x4 identity = Matrix4x4.identity;
			identity[0, 0] = (float)jObject.GetValue("00");
			identity[0, 1] = (float)jObject.GetValue("01");
			identity[0, 2] = (float)jObject.GetValue("02");
			identity[0, 3] = (float)jObject.GetValue("03");
			identity[1, 0] = (float)jObject.GetValue("10");
			identity[1, 1] = (float)jObject.GetValue("11");
			identity[1, 2] = (float)jObject.GetValue("12");
			identity[1, 3] = (float)jObject.GetValue("13");
			identity[2, 0] = (float)jObject.GetValue("20");
			identity[2, 1] = (float)jObject.GetValue("21");
			identity[2, 2] = (float)jObject.GetValue("22");
			identity[2, 3] = (float)jObject.GetValue("23");
			identity[3, 0] = (float)jObject.GetValue("30");
			identity[3, 1] = (float)jObject.GetValue("31");
			identity[3, 2] = (float)jObject.GetValue("32");
			identity[3, 3] = (float)jObject.GetValue("33");
			return identity;
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(Matrix4x4).IsAssignableFrom(objectType);
		}
	}
}
