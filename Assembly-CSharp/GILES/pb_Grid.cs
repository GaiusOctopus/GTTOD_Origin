using UnityEngine;

namespace GILES
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class pb_Grid : MonoBehaviour
	{
		[Tooltip("This script generates a grid mesh on load.")]
		public int lines = 10;

		public float scale = 1f;

		public Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);

		public Material gridMaterial;

		private void Start()
		{
			GetComponent<MeshFilter>().sharedMesh = GridMesh(lines, scale);
			GetComponent<MeshRenderer>().sharedMaterial = gridMaterial;
			GetComponent<MeshRenderer>().sharedMaterial.color = gridColor;
			base.transform.position = Vector3.zero;
		}

		private Mesh GridMesh(int lineCount, float scale)
		{
			float num = (float)lineCount / 2f * scale;
			lineCount++;
			Vector3[] array = new Vector3[lineCount * 4];
			Vector3[] array2 = new Vector3[lineCount * 4];
			Vector2[] array3 = new Vector2[lineCount * 4];
			int[] array4 = new int[lineCount * 4];
			int num2 = 0;
			for (int i = 0; i < lineCount; i++)
			{
				array4[num2] = num2;
				array3[num2] = ((i % 10 == 0) ? Vector2.one : Vector2.zero);
				array[num2++] = new Vector3((float)i * scale - num, 0f, 0f - num);
				array4[num2] = num2;
				array3[num2] = ((i % 10 == 0) ? Vector2.one : Vector2.zero);
				array[num2++] = new Vector3((float)i * scale - num, 0f, num);
				array4[num2] = num2;
				array3[num2] = ((i % 10 == 0) ? Vector2.one : Vector2.zero);
				array[num2++] = new Vector3(0f - num, 0f, (float)i * scale - num);
				array4[num2] = num2;
				array3[num2] = ((i % 10 == 0) ? Vector2.one : Vector2.zero);
				array[num2++] = new Vector3(num, 0f, (float)i * scale - num);
			}
			for (int j = 0; j < array.Length; j++)
			{
				array2[j] = Vector3.up;
			}
			Mesh mesh = new Mesh();
			mesh.name = "GridMesh";
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.subMeshCount = 1;
			mesh.SetIndices(array4, MeshTopology.Lines, 0);
			mesh.uv = array3;
			return mesh;
		}
	}
}
