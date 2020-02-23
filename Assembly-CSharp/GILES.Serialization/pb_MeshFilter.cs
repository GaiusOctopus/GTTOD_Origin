using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MeshFilter : pb_SerializableObject<MeshFilter>
	{
		public pb_MeshFilter(MeshFilter obj)
			: base(obj)
		{
		}

		public pb_MeshFilter(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override Dictionary<string, object> PopulateSerializableDictionary()
		{
			return new Dictionary<string, object>
			{
				{
					"sharedMesh",
					target.sharedMesh
				},
				{
					"tag",
					target.tag
				},
				{
					"name",
					target.name
				},
				{
					"hideFlags",
					target.hideFlags
				}
			};
		}
	}
}
