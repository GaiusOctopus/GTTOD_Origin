using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MeshCollider : pb_SerializableObject<MeshCollider>
	{
		public pb_MeshCollider(MeshCollider obj)
			: base(obj)
		{
		}

		public pb_MeshCollider(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public override Dictionary<string, object> PopulateSerializableDictionary()
		{
			Dictionary<string, object> dictionary = pb_Reflection.ReflectProperties(target);
			if (dictionary.TryGetValue("sharedMesh", out object value))
			{
				MeshFilter component = target.gameObject.GetComponent<MeshFilter>();
				if (component != null && component.sharedMesh.GetInstanceID() == value.GetHashCode())
				{
					dictionary.Remove("sharedMesh");
				}
			}
			return dictionary;
		}
	}
}
