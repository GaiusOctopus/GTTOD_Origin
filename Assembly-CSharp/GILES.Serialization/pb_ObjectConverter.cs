using Newtonsoft.Json;
using System;

namespace GILES.Serialization
{
	public class pb_ObjectConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException("Cannot write objects!");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException("Cannot read objects!");
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType is pb_ObjectWrapper;
		}
	}
}
