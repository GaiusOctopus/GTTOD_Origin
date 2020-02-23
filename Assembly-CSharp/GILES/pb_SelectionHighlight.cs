using GILES.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	[pb_EditorComponent]
	[pb_JsonIgnore]
	public class pb_SelectionHighlight : MonoBehaviour
	{
		private enum HighlightType
		{
			Wireframe,
			Glow,
			Bounds
		}

		private HighlightType highlightType;

		public Material material;

		private Mesh mesh;

		private void Awake()
		{
			MeshFilter component = GetComponent<MeshFilter>();
			if (component == null || component.sharedMesh == null)
			{
				highlightType = HighlightType.Bounds;
			}
			switch (highlightType)
			{
			case HighlightType.Wireframe:
				mesh = GenerateWireframe(component.sharedMesh);
				material = pb_BuiltinResource.GetMaterial("Material/Wireframe");
				break;
			case HighlightType.Bounds:
			{
				Bounds bounds = (component != null && component.sharedMesh != null) ? component.sharedMesh.bounds : new Bounds(Vector3.zero, Vector3.one);
				mesh = GenerateBounds(bounds);
				material = pb_BuiltinResource.GetMaterial("Material/UnlitVertexColor");
				break;
			}
			case HighlightType.Glow:
				mesh = component.sharedMesh;
				material = pb_BuiltinResource.GetMaterial("Material/Highlight");
				break;
			}
			mesh.RecalculateBounds();
			material.SetVector("_Center", mesh.bounds.center);
			Graphics.DrawMesh(mesh, base.transform.localToWorldMatrix, material, 0);
		}

		private void OnDestroy()
		{
			if (highlightType == HighlightType.Wireframe || highlightType == HighlightType.Bounds)
			{
				pb_ObjectUtility.Destroy(mesh);
			}
		}

		private void Update()
		{
			Graphics.DrawMesh(mesh, base.transform.localToWorldMatrix, material, 0);
		}

		private Mesh GenerateWireframe(Mesh mesh)
		{
			Mesh mesh2 = new Mesh();
			mesh2.vertices = mesh.vertices;
			mesh2.normals = mesh.normals;
			int[] array = new int[mesh.triangles.Length * 2];
			int[] triangles = mesh.triangles;
			int num = 0;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				array[num++] = triangles[i];
				array[num++] = triangles[i + 1];
				array[num++] = triangles[i + 1];
				array[num++] = triangles[i + 2];
				array[num++] = triangles[i + 2];
				array[num++] = triangles[i];
			}
			mesh2.subMeshCount = 1;
			mesh2.SetIndices(array, MeshTopology.Lines, 0);
			return mesh2;
		}

		private Mesh GenerateBounds(Bounds bounds)
		{
			Vector3 center = bounds.center;
			Vector3 vector = bounds.extents + bounds.extents.normalized * 0.1f;
			List<Vector3> list = new List<Vector3>();
			list.AddRange(DrawBoundsEdge(center, 0f - vector.x, 0f - vector.y, 0f - vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, 0f - vector.x, 0f - vector.y, vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, vector.x, 0f - vector.y, 0f - vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, vector.x, 0f - vector.y, vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, 0f - vector.x, vector.y, 0f - vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, 0f - vector.x, vector.y, vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, vector.x, vector.y, 0f - vector.z, 0.2f));
			list.AddRange(DrawBoundsEdge(center, vector.x, vector.y, vector.z, 0.2f));
			Vector2[] array = new Vector2[48];
			int[] array2 = new int[48];
			Color[] array3 = new Color[48];
			for (int i = 0; i < 48; i++)
			{
				array2[i] = i;
				array[i] = Vector2.zero;
				array3[i] = Color.white;
				array3[i].a = 0.5f;
			}
			Mesh obj = new Mesh();
			obj.vertices = list.ToArray();
			obj.subMeshCount = 1;
			obj.SetIndices(array2, MeshTopology.Lines, 0);
			obj.uv = array;
			obj.normals = list.ToArray();
			obj.colors = array3;
			return obj;
		}

		private Vector3[] DrawBoundsEdge(Vector3 center, float x, float y, float z, float size)
		{
			Vector3 vector = center;
			Vector3[] array = new Vector3[6];
			vector.x += x;
			vector.y += y;
			vector.z += z;
			array[0] = vector;
			array[1] = vector + (0f - x / Mathf.Abs(x)) * Vector3.right * Mathf.Min(size, Mathf.Abs(x));
			array[2] = vector;
			array[3] = vector + (0f - y / Mathf.Abs(y)) * Vector3.up * Mathf.Min(size, Mathf.Abs(y));
			array[4] = vector;
			array[5] = vector + (0f - z / Mathf.Abs(z)) * Vector3.forward * Mathf.Min(size, Mathf.Abs(z));
			return array;
		}
	}
}
