using Newtonsoft.Json;
using UnityEngine;

namespace GILES.Serialization
{
	public static class pb_Serialization
	{
		public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			ContractResolver = new pb_ContractResolver(),
			TypeNameHandling = TypeNameHandling.Objects
		};

		public static readonly JsonSerializer Serializer = JsonSerializer.Create(ConverterSettings);

		public static pb_ISerializable CreateSerializableObject<T>(T obj)
		{
			if (obj is Camera)
			{
				return new pb_CameraComponent(obj as Camera);
			}
			if (obj is MeshFilter)
			{
				return new pb_MeshFilter(obj as MeshFilter);
			}
			if (obj is MeshCollider)
			{
				return new pb_MeshCollider(obj as MeshCollider);
			}
			if (obj is MeshRenderer)
			{
				return new pb_MeshRenderer(obj as MeshRenderer);
			}
			return new pb_SerializableObject<T>(obj);
		}
	}
}
