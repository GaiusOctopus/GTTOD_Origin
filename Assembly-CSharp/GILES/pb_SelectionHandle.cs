using System.Collections;
using UnityEngine;

namespace GILES
{
	public class pb_SelectionHandle : pb_MonoBehaviourSingleton<pb_SelectionHandle>
	{
		public delegate void OnHandleMoveEvent(pb_Transform transform);

		public delegate void OnHandleBeginEvent(pb_Transform transform);

		public delegate void OnHandleFinishEvent();

		private class DragOrientation
		{
			public Vector3 origin;

			public Vector3 axis;

			public Vector3 mouse;

			public Vector3 cross;

			public Vector3 offset;

			public Plane plane;

			public DragOrientation()
			{
				origin = Vector3.zero;
				axis = Vector3.zero;
				mouse = Vector3.zero;
				cross = Vector3.zero;
				offset = Vector3.zero;
				plane = new Plane(Vector3.up, Vector3.zero);
			}
		}

		public static float positionSnapValue = 1f;

		public static float scaleSnapValue = 0.1f;

		public static float rotationSnapValue = 15f;

		private Transform _trs;

		private Camera _cam;

		private const int MAX_DISTANCE_TO_HANDLE = 15;

		private static Mesh _HandleLineMesh = null;

		private static Mesh _HandleTriangleMesh = null;

		public Mesh ConeMesh;

		public Mesh CubeMesh;

		private Tool tool = Tool.Position;

		private Mesh _coneRight;

		private Mesh _coneUp;

		private Mesh _coneForward;

		private const float CAP_SIZE = 0.07f;

		public float HandleSize = 90f;

		public Callback onHandleTypeChanged;

		private Vector2 mouseOrigin = Vector2.zero;

		private int draggingAxes;

		private Vector3 scale = Vector3.one;

		private pb_Transform handleOrigin = pb_Transform.identity;

		private DragOrientation drag = new DragOrientation();

		private float axisAngle;

		private Matrix4x4 handleMatrix;

		private const float HANDLE_BOX_SIZE = 0.25f;

		private Transform trs
		{
			get
			{
				if (_trs == null)
				{
					_trs = base.gameObject.GetComponent<Transform>();
				}
				return _trs;
			}
		}

		private Camera cam
		{
			get
			{
				if (_cam == null)
				{
					_cam = Camera.main;
				}
				return _cam;
			}
		}

		private Material HandleOpaqueMaterial => pb_BuiltinResource.GetMaterial("Handles/Material/HandleOpaqueMaterial");

		private Material RotateLineMaterial => pb_BuiltinResource.GetMaterial("Handles/Material/HandleRotateMaterial");

		private Material HandleTransparentMaterial => pb_BuiltinResource.GetMaterial("Handles/Material/HandleTransparentMaterial");

		private Mesh HandleLineMesh
		{
			get
			{
				if (_HandleLineMesh == null)
				{
					_HandleLineMesh = new Mesh();
					CreateHandleLineMesh(ref _HandleLineMesh, Vector3.one);
				}
				return _HandleLineMesh;
			}
		}

		private Mesh HandleTriangleMesh
		{
			get
			{
				if (_HandleTriangleMesh == null)
				{
					_HandleTriangleMesh = new Mesh();
					CreateHandleTriangleMesh(ref _HandleTriangleMesh, Vector3.one);
				}
				return _HandleTriangleMesh;
			}
		}

		public bool draggingHandle
		{
			get;
			private set;
		}

		public bool isHidden
		{
			get;
			private set;
		}

		public event OnHandleMoveEvent OnHandleMove;

		public event OnHandleBeginEvent OnHandleBegin;

		public event OnHandleFinishEvent OnHandleFinish;

		public bool InUse()
		{
			return draggingHandle;
		}

		protected override void Awake()
		{
			_trs = null;
			_cam = null;
			base.Awake();
		}

		private void Start()
		{
			pb_SceneCamera.AddOnCameraMoveDelegate(OnCameraMove);
			SetIsHidden(isHidden: true);
		}

		public void SetTRS(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			trs.position = position;
			trs.localRotation = rotation;
			trs.localScale = scale;
			RebuildGizmoMatrix();
		}

		private void OnCameraMove(pb_SceneCamera cam)
		{
			RebuildGizmoMesh(Vector3.one);
			RebuildGizmoMatrix();
		}

		private void Update()
		{
			if (isHidden)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftAlt))
			{
				OnMouseDown();
			}
			if (draggingHandle && Input.GetKey(KeyCode.LeftAlt))
			{
				OnFinishHandleMovement();
			}
			if (Input.GetMouseButton(0) && draggingHandle)
			{
				Vector3 OutPointA = Vector3.zero;
				bool flag = false;
				if (draggingAxes < 2 && tool != Tool.Rotate)
				{
					flag = pb_HandleUtility.PointOnLine(new Ray(trs.position, drag.axis), cam.ScreenPointToRay(Input.mousePosition), out OutPointA, out Vector3 _);
				}
				else
				{
					Ray ray = cam.ScreenPointToRay(Input.mousePosition);
					float enter = 0f;
					if (drag.plane.Raycast(ray, out enter))
					{
						OutPointA = ray.GetPoint(enter);
						flag = true;
					}
				}
				if (flag)
				{
					drag.origin = trs.position;
					switch (tool)
					{
					case Tool.Position:
						trs.position = pb_Snap.Snap(OutPointA - drag.offset, positionSnapValue);
						break;
					case Tool.Rotate:
					{
						Vector2 mouseDelta = (Vector2)Input.mousePosition - mouseOrigin;
						mouseOrigin = Input.mousePosition;
						float num = pb_HandleUtility.CalcMouseDeltaSignWithAxes(cam, drag.origin, drag.axis, drag.cross, mouseDelta);
						axisAngle += mouseDelta.magnitude * num;
						trs.localRotation = Quaternion.AngleAxis(pb_Snap.Snap(axisAngle, rotationSnapValue), drag.axis) * handleOrigin.rotation;
						break;
					}
					case Tool.Scale:
					{
						Vector3 value = (draggingAxes <= 1) ? (Quaternion.Inverse(handleOrigin.rotation) * (OutPointA - drag.offset - trs.position)) : SetUniformMagnitude(OutPointA - drag.offset - trs.position);
						value += Vector3.one;
						scale = pb_Snap.Snap(value, scaleSnapValue);
						RebuildGizmoMesh(scale);
						break;
					}
					}
					if (this.OnHandleMove != null)
					{
						this.OnHandleMove(GetTransform());
					}
					RebuildGizmoMatrix();
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				OnFinishHandleMovement();
			}
		}

		private Vector3 SetUniformMagnitude(Vector3 a)
		{
			a.z = (a.y = (a.x = ((Mathf.Abs(a.x) > Mathf.Abs(a.y) && Mathf.Abs(a.x) > Mathf.Abs(a.z)) ? a.x : ((Mathf.Abs(a.y) > Mathf.Abs(a.z)) ? a.y : a.z))));
			return a;
		}

		private void OnMouseDown()
		{
			scale = Vector3.one;
			drag.offset = Vector3.zero;
			axisAngle = 0f;
			draggingHandle = CheckHandleActivated(Input.mousePosition, out Axis plane);
			mouseOrigin = Input.mousePosition;
			handleOrigin.SetTRS(trs);
			drag.axis = Vector3.zero;
			draggingAxes = 0;
			if (!draggingHandle)
			{
				return;
			}
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if ((plane & Axis.X) == Axis.X)
			{
				draggingAxes++;
				drag.axis = trs.right;
				drag.plane.SetNormalAndPosition(trs.right.normalized, trs.position);
			}
			if ((plane & Axis.Y) == Axis.Y)
			{
				draggingAxes++;
				if (draggingAxes > 1)
				{
					drag.plane.SetNormalAndPosition(Vector3.Cross(drag.axis, trs.up).normalized, trs.position);
				}
				else
				{
					drag.plane.SetNormalAndPosition(trs.up.normalized, trs.position);
				}
				drag.axis += trs.up;
			}
			if ((plane & Axis.Z) == Axis.Z)
			{
				draggingAxes++;
				if (draggingAxes > 1)
				{
					drag.plane.SetNormalAndPosition(Vector3.Cross(drag.axis, trs.forward).normalized, trs.position);
				}
				else
				{
					drag.plane.SetNormalAndPosition(trs.forward.normalized, trs.position);
				}
				drag.axis += trs.forward;
			}
			if (draggingAxes < 2)
			{
				if (pb_HandleUtility.PointOnLine(new Ray(trs.position, drag.axis), ray, out Vector3 OutPointA, out Vector3 _))
				{
					drag.offset = OutPointA - trs.position;
				}
				float enter = 0f;
				if (drag.plane.Raycast(ray, out enter))
				{
					drag.mouse = (ray.GetPoint(enter) - trs.position).normalized;
					drag.cross = Vector3.Cross(drag.axis, drag.mouse);
				}
			}
			else
			{
				float enter2 = 0f;
				if (drag.plane.Raycast(ray, out enter2))
				{
					drag.offset = ray.GetPoint(enter2) - trs.position;
					drag.mouse = (ray.GetPoint(enter2) - trs.position).normalized;
					drag.cross = Vector3.Cross(drag.axis, drag.mouse);
				}
			}
			if (this.OnHandleBegin != null)
			{
				this.OnHandleBegin(GetTransform());
			}
		}

		private void OnFinishHandleMovement()
		{
			RebuildGizmoMesh(Vector3.one);
			RebuildGizmoMatrix();
			if (this.OnHandleFinish != null)
			{
				this.OnHandleFinish();
			}
			StartCoroutine(SetDraggingFalse());
		}

		private IEnumerator SetDraggingFalse()
		{
			yield return new WaitForEndOfFrame();
			draggingHandle = false;
		}

		private Vector2 screenToGUIPoint(Vector2 v)
		{
			v.y = (float)Screen.height - v.y;
			return v;
		}

		public pb_Transform GetTransform()
		{
			return new pb_Transform(trs.position, trs.localRotation, scale);
		}

		private bool CheckHandleActivated(Vector2 mousePosition, out Axis plane)
		{
			plane = Axis.None;
			if (tool == Tool.Position || tool == Tool.Scale)
			{
				float handleSize = pb_HandleUtility.GetHandleSize(trs.position);
				Vector2 vector = cam.WorldToScreenPoint(trs.position);
				Vector2 vector2 = cam.WorldToScreenPoint(trs.position + (trs.up + trs.up * 0.07f * 4f) * (handleSize * HandleSize));
				Vector2 vector3 = cam.WorldToScreenPoint(trs.position + (trs.right + trs.right * 0.07f * 4f) * (handleSize * HandleSize));
				Vector2 vector4 = cam.WorldToScreenPoint(trs.position + (trs.forward + trs.forward * 0.07f * 4f) * (handleSize * HandleSize));
				Vector3 vector5 = pb_HandleUtility.DirectionMask(trs, cam.transform.forward);
				Vector2 vector6 = vector + (vector3 - vector) * vector5.x * 0.25f;
				Vector2 vector7 = vector + (vector2 - vector) * vector5.y * 0.25f;
				Vector2 vector8 = vector + (vector4 - vector) * vector5.z * 0.25f;
				if (pb_HandleUtility.PointInPolygon(new Vector2[8]
				{
					vector,
					vector7,
					vector7,
					vector7 + vector8 - vector,
					vector7 + vector8 - vector,
					vector8,
					vector8,
					vector
				}, mousePosition))
				{
					plane = (Axis.Y | Axis.Z);
				}
				else if (pb_HandleUtility.PointInPolygon(new Vector2[8]
				{
					vector,
					vector6,
					vector6,
					vector6 + vector8 - vector,
					vector6 + vector8 - vector,
					vector8,
					vector8,
					vector
				}, mousePosition))
				{
					plane = (Axis.X | Axis.Z);
				}
				else if (pb_HandleUtility.PointInPolygon(new Vector2[8]
				{
					vector,
					vector7,
					vector7,
					vector7 + vector6 - vector,
					vector7 + vector6 - vector,
					vector6,
					vector6,
					vector
				}, mousePosition))
				{
					plane = (Axis.X | Axis.Y);
				}
				else if (pb_HandleUtility.DistancePointLineSegment(mousePosition, vector, vector2) < 15f)
				{
					plane = Axis.Y;
				}
				else if (pb_HandleUtility.DistancePointLineSegment(mousePosition, vector, vector3) < 15f)
				{
					plane = Axis.X;
				}
				else
				{
					if (!(pb_HandleUtility.DistancePointLineSegment(mousePosition, vector, vector4) < 15f))
					{
						return false;
					}
					plane = Axis.Z;
				}
				return true;
			}
			Vector3[][] rotationVertices = pb_HandleMesh.GetRotationVertices(16, 1f);
			float num = float.PositiveInfinity;
			Vector2 zero = Vector2.zero;
			plane = Axis.X;
			for (int i = 0; i < 3; i++)
			{
				Vector2 vector9 = cam.WorldToScreenPoint(rotationVertices[i][0]);
				for (int j = 0; j < rotationVertices[i].Length - 1; j++)
				{
					zero = vector9;
					vector9 = cam.WorldToScreenPoint(handleMatrix.MultiplyPoint3x4(rotationVertices[i][j + 1]));
					float num2 = pb_HandleUtility.DistancePointLineSegment(mousePosition, zero, vector9);
					if (!(num2 < num) || !(num2 < 15f))
					{
						continue;
					}
					Vector3 normalized = (handleMatrix.MultiplyPoint3x4((rotationVertices[i][j] + rotationVertices[i][j + 1]) * 0.5f) - cam.transform.position).normalized;
					if (!(Vector3.Dot(base.transform.TransformDirection(rotationVertices[i][j]).normalized, normalized) > 0.5f))
					{
						num = num2;
						switch (i)
						{
						case 0:
							plane = Axis.Y;
							break;
						case 1:
							plane = Axis.Z;
							break;
						case 2:
							plane = Axis.X;
							break;
						}
					}
				}
			}
			if (num < 15.1f)
			{
				return true;
			}
			return false;
		}

		private void OnRenderObject()
		{
			if (!isHidden && !(Camera.current != cam))
			{
				switch (tool)
				{
				case Tool.Position:
				case Tool.Scale:
					HandleOpaqueMaterial.SetPass(0);
					Graphics.DrawMeshNow(HandleLineMesh, handleMatrix);
					Graphics.DrawMeshNow(HandleTriangleMesh, handleMatrix, 1);
					HandleTransparentMaterial.SetPass(0);
					Graphics.DrawMeshNow(HandleTriangleMesh, handleMatrix, 0);
					break;
				case Tool.Rotate:
					RotateLineMaterial.SetPass(0);
					Graphics.DrawMeshNow(HandleLineMesh, handleMatrix);
					break;
				}
			}
		}

		private void RebuildGizmoMatrix()
		{
			float handleSize = pb_HandleUtility.GetHandleSize(trs.position);
			Matrix4x4 rhs = Matrix4x4.Scale(Vector3.one * handleSize * HandleSize);
			handleMatrix = base.transform.localToWorldMatrix * rhs;
		}

		private void RebuildGizmoMesh(Vector3 scale)
		{
			if (_HandleLineMesh == null)
			{
				_HandleLineMesh = new Mesh();
			}
			if (_HandleTriangleMesh == null)
			{
				_HandleTriangleMesh = new Mesh();
			}
			CreateHandleLineMesh(ref _HandleLineMesh, scale);
			CreateHandleTriangleMesh(ref _HandleTriangleMesh, scale);
		}

		public void SetTool(Tool tool)
		{
			if (this.tool != tool)
			{
				this.tool = tool;
				RebuildGizmoMesh(Vector3.one);
				if (onHandleTypeChanged != null)
				{
					onHandleTypeChanged();
				}
			}
		}

		public Tool GetTool()
		{
			return tool;
		}

		public void SetIsHidden(bool isHidden)
		{
			draggingHandle = false;
			this.isHidden = isHidden;
			if (onHandleTypeChanged != null)
			{
				onHandleTypeChanged();
			}
		}

		public bool GetIsHidden()
		{
			return isHidden;
		}

		private void CreateHandleLineMesh(ref Mesh mesh, Vector3 scale)
		{
			switch (tool)
			{
			case Tool.Position:
			case Tool.Scale:
				pb_HandleMesh.CreatePositionLineMesh(ref mesh, trs, scale, cam, 0.25f);
				break;
			case Tool.Rotate:
				pb_HandleMesh.CreateRotateMesh(ref mesh, 48, 1f);
				break;
			}
		}

		private void CreateHandleTriangleMesh(ref Mesh mesh, Vector3 scale)
		{
			if (tool == Tool.Position)
			{
				pb_HandleMesh.CreateTriangleMesh(ref mesh, trs, scale, cam, ConeMesh, 0.25f, 0.07f);
			}
			else if (tool == Tool.Scale)
			{
				pb_HandleMesh.CreateTriangleMesh(ref mesh, trs, scale, cam, CubeMesh, 0.25f, 0.07f);
			}
		}
	}
}
