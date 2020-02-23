using System;
using UnityEngine;

namespace GILES
{
	[pb_Gizmo(typeof(Light))]
	public class pb_Gizmo_Light : pb_Gizmo
	{
		public Material lightRayMaterial;

		private Light lightComponent;

		private Mesh _lightMesh;

		private readonly Color yellow = new Color(1f, 1f, 0f, 0.5f);

		private Matrix4x4 gizmoMatrix = Matrix4x4.identity;

		private Mesh pointLightDisc;

		private Mesh lightMesh
		{
			get
			{
				if (_lightMesh == null && lightComponent != null)
				{
					switch (lightComponent.type)
					{
					case LightType.Directional:
						_lightMesh = DirectionalLightMesh();
						lightRayMaterial = pb_BuiltinResource.GetMaterial("Material/UnlitVertexColorWavy");
						break;
					case LightType.Spot:
						_lightMesh = SpotLightMesh();
						lightRayMaterial = pb_BuiltinResource.GetMaterial("Material/UnlitVertexColor");
						break;
					case LightType.Point:
						_lightMesh = PointLightMesh();
						lightRayMaterial = pb_BuiltinResource.GetMaterial("Material/UnlitVertexColor");
						break;
					case LightType.Area:
						_lightMesh = AreaLightMesh();
						lightRayMaterial = pb_BuiltinResource.GetMaterial("Material/UnlitVertexColorWavy");
						break;
					default:
						_lightMesh = null;
						break;
					}
				}
				return _lightMesh;
			}
		}

		private void RebuildGizmos()
		{
			if (_lightMesh != null)
			{
				pb_ObjectUtility.Destroy(lightMesh);
				_lightMesh = null;
			}
			if (pointLightDisc == null)
			{
				pointLightDisc = new Mesh();
				pb_HandleMesh.CreateDiscMesh(ref pointLightDisc, 64, 1.1f);
				pointLightDisc.colors = pb_CollectionUtil.Fill(yellow, pointLightDisc.vertexCount);
			}
		}

		private void Start()
		{
			icon = pb_BuiltinResource.GetMaterial("Gizmos/Light");
			lightComponent = GetComponentInChildren<Light>();
			RebuildGizmos();
		}

		public override void Update()
		{
			base.Update();
			if (isSelected && !(lightMesh == null) && !(lightComponent == null))
			{
				LightType type = lightComponent.type;
				if (type == LightType.Point)
				{
					gizmoMatrix.SetTRS(trs.position, cam.localRotation, Vector3.one * lightComponent.range);
					Graphics.DrawMesh(pointLightDisc, gizmoMatrix, lightRayMaterial, 0, null, 0, null, castShadows: false, receiveShadows: false);
					gizmoMatrix.SetTRS(trs.position, Quaternion.identity, Vector3.one * lightComponent.range);
				}
				else
				{
					gizmoMatrix.SetTRS(trs.position, trs.localRotation, Vector3.one);
				}
				for (int i = 0; i < lightMesh.subMeshCount; i++)
				{
					Graphics.DrawMesh(lightMesh, gizmoMatrix, lightRayMaterial, 0, null, i, null, castShadows: false, receiveShadows: false);
				}
			}
		}

		public override void OnComponentModified()
		{
			RebuildGizmos();
		}

		private Mesh PointLightMesh()
		{
			Mesh mesh = new Mesh();
			pb_HandleMesh.CreateRotateMesh(ref mesh, 64, 1f);
			Color value = yellow;
			value.a = 0.2f;
			mesh.colors = pb_CollectionUtil.Fill(value, mesh.vertexCount);
			return mesh;
		}

		private Mesh DirectionalLightMesh()
		{
			Mesh mesh = new Mesh();
			Vector3[] array = new Vector3[96];
			Vector2[] array2 = new Vector2[96];
			int[] array3 = new int[192];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 32; i++)
			{
				float num3 = (float)i / 32f;
				array[num] = new Vector3(-0.6f, -0.5f, num3 * 4f);
				array[num + 1] = new Vector3(0.6f, -0.5f, num3 * 4f);
				array[num + 2] = new Vector3(0f, 0.2f, num3 * 4f);
				array2[num] = new Vector2(num3, 1f - num3 + 0f);
				array2[num + 1] = new Vector2(num3, 1f - num3 + 0.23f);
				array2[num + 2] = new Vector2(num3, 1f - num3 + 0.34f);
				if (i < 31)
				{
					array3[num2++] = num;
					array3[num2++] = num + 3;
					array3[num2++] = num + 1;
					array3[num2++] = num + 4;
					array3[num2++] = num + 2;
					array3[num2++] = num + 5;
				}
				num += 3;
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.colors = pb_CollectionUtil.Fill(Color.yellow, mesh.vertexCount);
			mesh.normals = pb_CollectionUtil.Fill(Vector3.up, mesh.vertexCount);
			mesh.subMeshCount = 1;
			mesh.SetIndices(array3, MeshTopology.Lines, 0);
			return mesh;
		}

		private Mesh SpotLightMesh()
		{
			Mesh mesh = new Mesh();
			float num = lightComponent.range * Mathf.Tan((float)Math.PI / 180f * (lightComponent.spotAngle / 2f));
			Vector3[] array = new Vector3[33];
			int[] array2 = new int[72];
			int num2 = 0;
			for (int i = 0; i < 32; i++)
			{
				float f = (float)i / 32f * 360f * ((float)Math.PI / 180f);
				array[i] = new Vector3(Mathf.Cos(f) * num, Mathf.Sin(f) * num, lightComponent.range);
				array2[num2++] = i;
				array2[num2++] = ((i < 31) ? (i + 1) : 0);
			}
			array[32] = Vector3.zero;
			array2[num2++] = 32;
			array2[num2++] = 0;
			array2[num2++] = 32;
			array2[num2++] = 8;
			array2[num2++] = 32;
			array2[num2++] = 16;
			array2[num2++] = 32;
			array2[num2++] = 24;
			mesh.vertices = array;
			mesh.normals = pb_CollectionUtil.Fill(Vector3.up, array.Length);
			mesh.colors = pb_CollectionUtil.Fill(yellow, array.Length);
			mesh.subMeshCount = 1;
			mesh.SetIndices(array2, MeshTopology.Lines, 0);
			return mesh;
		}

		private Mesh AreaLightMesh()
		{
			Mesh mesh = new Mesh();
			Vector3 one = Vector3.one;
			mesh.vertices = new Vector3[12]
			{
				Vector3.Scale(new Vector3(-0.5f, -0.5f, 0f), one),
				Vector3.Scale(new Vector3(0.5f, -0.5f, 0f), one),
				Vector3.Scale(new Vector3(0.5f, 0.5f, 0f), one),
				Vector3.Scale(new Vector3(-0.5f, 0.5f, 0f), one),
				Vector3.Scale(new Vector3(-0.4f, -0.4f, 0f), one),
				Vector3.Scale(new Vector3(0.4f, -0.4f, 0f), one),
				Vector3.Scale(new Vector3(0.4f, 0.4f, 0f), one),
				Vector3.Scale(new Vector3(-0.4f, 0.4f, 0f), one),
				Vector3.Scale(new Vector3(-0.4f, -0.4f, 0f), one) + Vector3.forward,
				Vector3.Scale(new Vector3(0.4f, -0.4f, 0f), one) + Vector3.forward,
				Vector3.Scale(new Vector3(0.4f, 0.4f, 0f), one) + Vector3.forward,
				Vector3.Scale(new Vector3(-0.4f, 0.4f, 0f), one) + Vector3.forward
			};
			mesh.subMeshCount = 1;
			mesh.SetIndices(new int[16]
			{
				0,
				1,
				1,
				2,
				2,
				3,
				3,
				0,
				4,
				8,
				5,
				9,
				6,
				10,
				7,
				11
			}, MeshTopology.Lines, 0);
			mesh.colors = pb_CollectionUtil.Fill(yellow, mesh.vertexCount);
			return mesh;
		}
	}
}
