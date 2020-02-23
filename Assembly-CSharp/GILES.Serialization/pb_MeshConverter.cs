using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using UnityEngine;

namespace GILES.Serialization
{
	public class pb_MeshConverter : pb_UnityTypeConverter<Mesh>
	{
		public override void WriteObjectJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JObject jObject = new JObject();
			Mesh mesh = (Mesh)value;
			jObject.Add("$type", value.GetType().AssemblyQualifiedName);
			jObject.Add("vertices", JArray.FromObject(mesh.vertices, serializer));
			jObject.Add("bindposes", JArray.FromObject(mesh.bindposes, serializer));
			jObject.Add("boneWeights", JArray.FromObject(mesh.boneWeights, serializer));
			jObject.Add("colors", JArray.FromObject(mesh.colors, serializer));
			jObject.Add("colors32", JArray.FromObject(mesh.colors32, serializer));
			jObject.Add("normals", JArray.FromObject(mesh.normals, serializer));
			jObject.Add("tangents", JArray.FromObject(mesh.tangents, serializer));
			jObject.Add("uv", JArray.FromObject(mesh.uv, serializer));
			jObject.Add("uv2", JArray.FromObject(mesh.uv2, serializer));
			jObject.Add("uv3", JArray.FromObject(mesh.uv3, serializer));
			jObject.Add("uv4", JArray.FromObject(mesh.uv4, serializer));
			jObject.Add("subMeshCount", mesh.subMeshCount);
			for (int i = 0; i < mesh.subMeshCount; i++)
			{
				jObject.Add("meshTopology" + i, (int)mesh.GetTopology(i));
				jObject.Add("submesh" + i, JArray.FromObject(mesh.GetTriangles(i)));
			}
			jObject.Add("hideFlags", (int)mesh.hideFlags);
			jObject.Add("name", mesh.name);
			jObject.WriteTo(writer, serializer.Converters.ToArray());
		}

		public override object ReadJsonObject(JObject o, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Mesh mesh = new Mesh();
			Matrix4x4[] bindposes = o.GetValue("bindposes").ToObject<Matrix4x4[]>();
			BoneWeight[] boneWeights = o.GetValue("boneWeights").ToObject<BoneWeight[]>();
			Color[] colors = o.GetValue("colors").ToObject<Color[]>();
			Color32[] colors2 = o.GetValue("colors32").ToObject<Color32[]>();
			Vector3[] normals = o.GetValue("normals").ToObject<Vector3[]>();
			Vector4[] tangents = o.GetValue("tangents").ToObject<Vector4[]>();
			Vector2[] uv = o.GetValue("uv").ToObject<Vector2[]>();
			Vector2[] uv2 = o.GetValue("uv2").ToObject<Vector2[]>();
			Vector2[] uv3 = o.GetValue("uv3").ToObject<Vector2[]>();
			Vector2[] uv4 = o.GetValue("uv4").ToObject<Vector2[]>();
			Vector3[] array2 = mesh.vertices = o.GetValue("vertices").ToObject<Vector3[]>();
			mesh.bindposes = bindposes;
			mesh.boneWeights = boneWeights;
			mesh.colors = colors;
			mesh.colors32 = colors2;
			mesh.normals = normals;
			mesh.tangents = tangents;
			mesh.uv = uv;
			mesh.uv2 = uv2;
			mesh.uv3 = uv3;
			mesh.uv4 = uv4;
			int num2 = mesh.subMeshCount = (int)o.GetValue("subMeshCount");
			for (int i = 0; i < num2; i++)
			{
				int[] indices = o.GetValue("submesh" + i).ToObject<int[]>();
				MeshTopology topology = (MeshTopology)(int)o.GetValue("meshTopology" + i);
				mesh.SetIndices(indices, topology, i);
			}
			mesh.hideFlags = (HideFlags)(int)o.GetValue("hideFlags");
			mesh.name = o.GetValue("name").ToObject<string>();
			return mesh;
		}
	}
}
