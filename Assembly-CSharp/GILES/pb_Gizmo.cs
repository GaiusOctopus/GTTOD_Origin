using GILES.Serialization;
using System;
using System.Linq;
using UnityEngine;

namespace GILES
{
	[pb_JsonIgnore]
	[pb_EditorComponent]
	public abstract class pb_Gizmo : MonoBehaviour
	{
		public Material icon;

		protected Transform cam;

		protected Transform trs;

		private Matrix4x4 _cameraFacingMatrix = Matrix4x4.identity;

		public bool isSelected;

		private static Mesh _mesh;

		protected Matrix4x4 cameraFacingMatrix => _cameraFacingMatrix;

		private static Mesh mesh
		{
			get
			{
				if (_mesh == null)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
					_mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
					pb_ObjectUtility.Destroy(gameObject);
				}
				return _mesh;
			}
		}

		private void Awake()
		{
			cam = Camera.main.transform;
			trs = base.transform;
		}

		public virtual void OnComponentModified()
		{
		}

		public bool CanEditType(Type t)
		{
			return (GetType().GetCustomAttributes(inherit: true).FirstOrDefault((object x) => x is pb_GizmoAttribute) as pb_GizmoAttribute)?.CanEditType(t) ?? false;
		}

		public virtual void Update()
		{
			_cameraFacingMatrix.SetTRS(trs.position, Quaternion.LookRotation(cam.forward, Vector3.up), Vector3.one);
			Graphics.DrawMesh(mesh, _cameraFacingMatrix, icon, 0);
		}
	}
}
