using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MaterialConverter : pb_UnityTypeConverter<Material>
	{
		public override void WriteObjectJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject jObject = new JObject();
			Material material = (Material)value;
			jObject.Add("name", material.name);
			jObject.Add("shader", material.shader.name.ToString());
			jObject.Add("shaderObj", JObject.FromObject(material.shader));
			jObject.WriteTo(writer, serializer.Converters.ToArray());
		}

		public override object ReadJsonObject(JObject obj, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				string name = obj.GetValue("name").ToObject<string>();
				return new Material(Shader.Find(obj.GetValue("shader").ToObject<string>()))
				{
					name = name
				};
			}
			catch
			{
				return null;
			}
		}
	}
}
