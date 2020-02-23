using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MeshRenderer : pb_SerializableObject<MeshRenderer>
	{
		public pb_MeshRenderer(MeshRenderer obj)
			: base(obj)
		{
		}

		public pb_MeshRenderer(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override Dictionary<string, object> PopulateSerializableDictionary()
		{
			return pb_Reflection.ReflectProperties(target, new HashSet<string>
			{
				"material",
				"sharedMaterial",
				"materials"
			});
		}
	}
}
