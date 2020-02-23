using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_CameraComponent : pb_SerializableObject<Camera>
	{
		public pb_CameraComponent(Camera obj)
			: base(obj)
		{
		}

		public pb_CameraComponent(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override Dictionary<string, object> PopulateSerializableDictionary()
		{
			Dictionary<string, object> dictionary = pb_Reflection.ReflectProperties(target);
			dictionary.Remove("worldToCameraMatrix");
			dictionary.Remove("projectionMatrix");
			return dictionary;
		}
	}
}
