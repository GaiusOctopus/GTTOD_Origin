using System;
using System.Collections.Generic;
using UnityEngine;

namespace GILES
{
	public static class pb_HandleMesh
	{
		private static readonly Color red = new Color(0.85f, 0.256f, 0.16f, 0f);

		private static readonly Color green = new Color(0.2f, 0.9f, 0.2f, 0f);

		private static readonly Color blue = new Color(0.26f, 0.56f, 0.85f, 0f);

		public static Mesh CreatePositionLineMesh(ref Mesh mesh, Transform transform, Vector3 scale, Camera cam, float handleSize)
		{
			Vector3 vector = pb_HandleUtility.DirectionMask(transform, cam.transform.forward);
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Vector2> list3 = new List<Vector2>();
			List<Color> list4 = new List<Color>();
			List<int> list5 = new List<int>();
			list.AddRange(new Vector3[24]
			{
				Vector3.zero,
				Vector3.right * scale.x,
				new Vector3(vector.x * 0f, vector.y * handleSize, 0f),
				new Vector3(vector.x * handleSize, vector.y * handleSize, 0f),
				new Vector3(vector.x * handleSize, vector.y * handleSize, 0f),
				new Vector3(vector.x * handleSize, vector.y * 0f, 0f),
				new Vector3(vector.x * handleSize, vector.y * 0f, 0f),
				Vector3.zero,
				Vector3.zero,
				Vector3.up * scale.y,
				new Vector3(vector.x * handleSize, 0f, vector.z * 0f),
				new Vector3(vector.x * handleSize, 0f, vector.z * handleSize),
				new Vector3(vector.x * handleSize, 0f, vector.z * handleSize),
				new Vector3(vector.x * 0f, 0f, vector.z * handleSize),
				Vector3.zero,
				Vector3.forward * scale.z,
				Vector3.zero,
				new Vector3(0f, vector.y * 0f, vector.z * handleSize),
				new Vector3(0f, vector.y * 0f, vector.z * handleSize),
				new Vector3(0f, vector.y * handleSize, vector.z * handleSize),
				new Vector3(0f, vector.y * handleSize, vector.z * handleSize),
				new Vector3(0f, vector.y * handleSize, vector.z * 0f),
				new Vector3(0f, vector.y * handleSize, vector.z * 0f),
				Vector3.zero
			});
			list4.AddRange(new Color[24]
			{
				red,
				red,
				blue,
				blue,
				blue,
				blue,
				blue,
				blue,
				green,
				green,
				green,
				green,
				green,
				green,
				blue,
				blue,
				red,
				red,
				red,
				red,
				red,
				red,
				red,
				red
			});
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(Vector3.up);
				list3.Add(Vector2.zero);
				list5.Add(i);
			}
			mesh.Clear();
			mesh.name = "pb_Handle_TRANSLATE";
			mesh.vertices = list.ToArray();
			mesh.uv = list3.ToArray();
			mesh.subMeshCount = 1;
			mesh.SetIndices(list5.ToArray(), MeshTopology.Lines, 0);
			mesh.colors = list4.ToArray();
			mesh.normals = list2.ToArray();
			return mesh;
		}

		public static Vector3[][] GetRotationVertices(int segments, float radius)
		{
			Vector3[] array = new Vector3[segments];
			Vector3[] array2 = new Vector3[segments];
			Vector3[] array3 = new Vector3[segments];
			for (int i = 0; i < segments; i++)
			{
				float f = (float)i / (float)(segments - 1) * 360f * ((float)Math.PI / 180f);
				array[i].x = Mathf.Cos(f) * radius;
				array[i].z = Mathf.Sin(f) * radius;
				array2[i].x = Mathf.Cos(f) * radius;
				array2[i].y = Mathf.Sin(f) * radius;
				array3[i].z = Mathf.Cos(f) * radius;
				array3[i].y = Mathf.Sin(f) * radius;
			}
			return new Vector3[3][]
			{
				array,
				array2,
				array3
			};
		}

		public static Mesh CreateRotateMesh(ref Mesh mesh, int segments, float radius)
		{
			Vector3[][] rotationVertices = GetRotationVertices(segments, radius);
			Vector3[] array = new Vector3[segments * 3];
			Vector3[] array2 = new Vector3[segments * 3];
			Color[] array3 = new Color[segments * 3];
			int[] array4 = new int[segments];
			int[] array5 = new int[segments];
			int[] array6 = new int[segments];
			int num = 0;
			int num2 = segments;
			int num3 = segments + segments;
			for (int i = 0; i < segments; i++)
			{
				num = i;
				num2 = i + segments;
				num3 = i + segments + segments;
				array[num] = rotationVertices[0][i];
				array3[num] = green;
				array2[num].x = array[num].x;
				array2[num].z = array[num].z;
				array[num2] = rotationVertices[1][i];
				array3[num2] = blue;
				array2[num2].x = array[num2].x;
				array2[num2].y = array[num2].y;
				array[num3] = rotationVertices[2][i];
				array3[num3] = red;
				array2[num3].z = array[num3].z;
				array2[num3].y = array[num3].y;
				array4[i] = num;
				array5[i] = num2;
				array6[i] = num3;
			}
			mesh.Clear();
			mesh.name = "pb_Handle_ROTATE";
			mesh.vertices = array;
			mesh.uv = new Vector2[segments * 3];
			mesh.subMeshCount = 3;
			mesh.SetIndices(array4, MeshTopology.LineStrip, 0);
			mesh.SetIndices(array5, MeshTopology.LineStrip, 1);
			mesh.SetIndices(array6, MeshTopology.LineStrip, 2);
			mesh.colors = array3;
			mesh.normals = array2;
			return mesh;
		}

		public static Mesh CreateDiscMesh(ref Mesh mesh, int segments, float radius)
		{
			Vector3[] array = new Vector3[segments];
			Vector3[] array2 = new Vector3[segments];
			Vector2[] array3 = new Vector2[segments];
			int[] array4 = new int[segments];
			for (int i = 0; i < segments; i++)
			{
				float f = (float)i / (float)(segments - 1) * 360f * ((float)Math.PI / 180f);
				array[i] = new Vector3(Mathf.Cos(f) * radius, Mathf.Sin(f) * radius, 0f);
				array2[i] = array[i];
				array3[i] = Vector2.zero;
				array4[i] = ((i < segments - 1) ? i : 0);
			}
			mesh.Clear();
			mesh.name = "Disc";
			mesh.vertices = array;
			mesh.uv = array3;
			mesh.normals = array2;
			mesh.subMeshCount = 1;
			mesh.SetIndices(array4, MeshTopology.LineStrip, 0);
			return mesh;
		}

		public static Mesh CreateTriangleMesh(ref Mesh mesh, Transform transform, Vector3 scale, Camera cam, Mesh cap, float handleSize, float capSize)
		{
			Vector3 vector = pb_HandleUtility.DirectionMask(transform, cam.transform.forward);
			List<Vector3> list = new List<Vector3>
			{
				new Vector3(0f, vector.y * 0f, vector.z * 0f),
				new Vector3(0f, vector.y * 0f, vector.z * handleSize),
				new Vector3(0f, vector.y * handleSize, vector.z * handleSize),
				new Vector3(0f, vector.y * handleSize, vector.z * 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(vector.x * handleSize, 0f, 0f),
				new Vector3(vector.x * handleSize, vector.y * handleSize, 0f),
				new Vector3(0f, vector.y * handleSize, 0f),
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, 0f, handleSize * vector.z),
				new Vector3(handleSize * vector.x, 0f, handleSize * vector.z),
				new Vector3(handleSize * vector.x, 0f, 0f)
			};
			List<Vector3> list2 = new List<Vector3>
			{
				Vector3.right,
				Vector3.right,
				Vector3.right,
				Vector3.right,
				Vector3.forward,
				Vector3.forward,
				Vector3.forward,
				Vector3.forward,
				Vector3.up,
				Vector3.up,
				Vector3.up,
				Vector3.up
			};
			Color item = red;
			Color item2 = blue;
			Color item3 = green;
			item.a = 0.25f;
			item3.a = 0.25f;
			item2.a = 0.25f;
			List<Color> list3 = new List<Color>
			{
				item,
				item,
				item,
				item,
				item2,
				item2,
				item2,
				item2,
				item3,
				item3,
				item3,
				item3
			};
			List<Vector2> list4 = new List<Vector2>();
			List<int> list5 = new List<int>();
			for (int i = 0; i < list.Count; i += 4)
			{
				list4.Add(Vector2.zero);
				list4.Add(Vector2.zero);
				list4.Add(Vector2.zero);
				list4.Add(Vector2.zero);
				list5.Add(i);
				list5.Add(i + 1);
				list5.Add(i + 3);
				list5.Add(i + 1);
				list5.Add(i + 2);
				list5.Add(i + 3);
			}
			Matrix4x4 matrix = Matrix4x4.TRS(Vector3.right * scale.x, Quaternion.Euler(90f, 90f, 0f), Vector3.one * capSize);
			Matrix4x4 matrix2 = Matrix4x4.TRS(Vector3.up * scale.y, Quaternion.identity, Vector3.one * capSize);
			Matrix4x4 matrix3 = Matrix4x4.TRS(Vector3.forward * scale.z, Quaternion.Euler(90f, 0f, 0f), Vector3.one * capSize);
			List<int> list6 = new List<int>();
			TransformMesh(cap, matrix, out Vector3[] v, out Vector3[] n, out Vector2[] u, out Color[] c, out int[] t, list.Count, red);
			list.AddRange(v);
			list3.AddRange(c);
			list2.AddRange(n);
			list4.AddRange(u);
			list6.AddRange(t);
			TransformMesh(cap, matrix2, out v, out n, out u, out c, out t, list.Count, green);
			list.AddRange(v);
			list3.AddRange(c);
			list2.AddRange(n);
			list4.AddRange(u);
			list6.AddRange(t);
			TransformMesh(cap, matrix3, out v, out n, out u, out c, out t, list.Count, blue);
			list.AddRange(v);
			list3.AddRange(c);
			list2.AddRange(n);
			list4.AddRange(u);
			list6.AddRange(t);
			mesh.Clear();
			mesh.subMeshCount = 2;
			mesh.vertices = list.ToArray();
			mesh.normals = list2.ToArray();
			mesh.uv = list4.ToArray();
			mesh.SetTriangles(list5.ToArray(), 0);
			mesh.SetTriangles(list6.ToArray(), 1);
			mesh.colors = list3.ToArray();
			return mesh;
		}

		public static void TransformMesh(Mesh mesh, Matrix4x4 matrix, out Vector3[] v, out Vector3[] n, out Vector2[] u, out Color[] c, out int[] t, int indexOffset, Color color)
		{
			v = mesh.vertices;
			n = mesh.normals;
			u = mesh.uv;
			c = mesh.colors;
			t = mesh.triangles;
			color.a = 1f;
			for (int i = 0; i < v.Length; i++)
			{
				v[i] = matrix * v[i] + matrix.GetColumn(3);
				n[i] = matrix * n[i];
				c[i] = color;
			}
			for (int j = 0; j < t.Length; j++)
			{
				t[j] += indexOffset;
			}
		}
	}
}
