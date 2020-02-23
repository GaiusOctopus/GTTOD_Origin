using UnityEngine;
using UnityEngine.EventSystems;

namespace GILES
{
	[RequireComponent(typeof(Camera))]
	public class pb_SceneCamera : MonoBehaviour
	{
		public delegate void OnCameraMoveEvent(pb_SceneCamera cam);

		public delegate void OnCameraFinishMoveEvent(pb_SceneCamera cam);

		private static pb_SceneCamera instance;

		public Transform SpawnPosition;

		public LayerMask SpawnLayers;

		public float ClickTime = 0.1f;

		public Texture2D PanCursor;

		public Texture2D OrbitCursor;

		public Texture2D DollyCursor;

		public Texture2D LookCursor;

		private Texture2D currentCursor;

		private const int CURSOR_ICON_SIZE = 64;

		private const string INPUT_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";

		private const string INPUT_MOUSE_X = "Mouse X";

		private const string INPUT_MOUSE_Y = "Mouse Y";

		private const int LEFT_MOUSE = 0;

		private const int RIGHT_MOUSE = 1;

		private const int MIDDLE_MOUSE = 2;

		private const float MIN_CAM_DISTANCE = 1f;

		private const float MAX_CAM_DISTANCE = 1000f;

		public float moveSpeed = 15f;

		public float lookSpeed = 5f;

		public float orbitSpeed = 7f;

		public float scrollModifier = 100f;

		public float zoomSpeed = 0.1f;

		private bool eatMouse;

		private Camera cam;

		private Vector3 pivot = Vector3.zero;

		private float distanceToCamera = 10f;

		private Vector3 prev_mousePosition = Vector3.zero;

		private Rect mouseCursorRect = new Rect(0f, 0f, 64f, 64f);

		private Rect screenCenterRect = new Rect(0f, 0f, 64f, 64f);

		private bool currentActionValid = true;

		private bool zooming;

		private float zoomProgress;

		private Vector3 previousPosition = Vector3.zero;

		private Vector3 targetPosition = Vector3.zero;

		private pb_SceneEditor editor => pb_InputManager.GetCurrentEditor();

		public ViewTool cameraState
		{
			get;
			private set;
		}

		public event OnCameraMoveEvent OnCameraMove;

		public event OnCameraFinishMoveEvent OnCameraFinishMove;

		public static void AddOnCameraMoveDelegate(OnCameraMoveEvent del)
		{
			if (instance == null)
			{
				Debug.LogWarning("No pb_SceneCamera found, but someone is trying to subscribe to events!");
			}
			else if (instance.OnCameraMove == null)
			{
				instance.OnCameraMove = del;
			}
			else
			{
				instance.OnCameraMove += del;
			}
		}

		public Vector3 GetPivot()
		{
			return pivot;
		}

		private Vector3 CamTarget()
		{
			return base.transform.position + base.transform.forward * distanceToCamera;
		}

		private void Awake()
		{
			instance = this;
		}

		private void Start()
		{
			pb_InputManager.AddMouseInUseDelegate(IsUsingMouse);
			pb_InputManager.AddKeyInUseDelegate(IsUsingKey);
			cam = GetComponent<Camera>();
			distanceToCamera = Vector3.Distance(pivot, Camera.main.transform.position);
		}

		private void OnGUI()
		{
			float num = Screen.height;
			mouseCursorRect.x = Input.mousePosition.x - 16f;
			mouseCursorRect.y = num - Input.mousePosition.y - 16f;
			screenCenterRect.x = Screen.width / 2 - 32;
			screenCenterRect.y = num / 2f - 32f;
			Cursor.visible = (cameraState == ViewTool.None);
			if (cameraState != 0)
			{
				switch (cameraState)
				{
				case ViewTool.Orbit:
					GUI.Label(mouseCursorRect, OrbitCursor);
					break;
				case ViewTool.Pan:
					GUI.Label(mouseCursorRect, PanCursor);
					break;
				case ViewTool.Dolly:
					GUI.Label(mouseCursorRect, DollyCursor);
					break;
				case ViewTool.Look:
					GUI.Label(mouseCursorRect, LookCursor);
					break;
				}
			}
		}

		public bool IsUsingMouse(Vector2 mousePosition)
		{
			if (cameraState == ViewTool.None && !eatMouse)
			{
				return Input.GetKey(KeyCode.LeftAlt);
			}
			return true;
		}

		public bool IsUsingKey()
		{
			return Input.GetKey(KeyCode.LeftAlt);
		}

		private bool CheckMouseOverGUI()
		{
			if (!(EventSystem.current == null))
			{
				return !EventSystem.current.IsPointerOverGameObject();
			}
			return true;
		}

		private void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hitInfo, float.PositiveInfinity, SpawnLayers) && Input.GetKeyUp(KeyCode.Mouse0) && ClickTime > 0f && !EventSystem.current.IsPointerOverGameObject())
			{
				SpawnPosition.transform.position = hitInfo.point;
			}
			if (Input.GetKey(KeyCode.Mouse0))
			{
				ClickTime -= Time.deltaTime;
			}
			else
			{
				ClickTime = 0.1f;
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				SpawnPosition.transform.position = new Vector3(0f, -1f, 0f);
			}
		}

		private void LateUpdate()
		{
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
			{
				if (cameraState != 0 && this.OnCameraFinishMove != null)
				{
					this.OnCameraFinishMove(this);
				}
				currentActionValid = true;
				eatMouse = false;
			}
			else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				currentActionValid = CheckMouseOverGUI();
			}
			else if (base.transform.hasChanged && this.OnCameraMove != null)
			{
				this.OnCameraMove(this);
				base.transform.hasChanged = false;
			}
			cameraState = ViewTool.None;
			if (zooming)
			{
				base.transform.position = Vector3.Lerp(previousPosition, targetPosition, (zoomProgress += Time.deltaTime) / zoomSpeed);
				if (Vector3.Distance(base.transform.position, targetPosition) < 0.1f)
				{
					zooming = false;
				}
			}
			if ((Input.GetAxis("Mouse ScrollWheel") != 0f || (Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftAlt))) && CheckMouseOverGUI())
			{
				float num = Input.GetAxis("Mouse ScrollWheel");
				if (Mathf.Approximately(num, 0f))
				{
					cameraState = ViewTool.Dolly;
					num = pb_HandleUtility.CalcSignedMouseDelta(Input.mousePosition, prev_mousePosition);
				}
				distanceToCamera -= num * (distanceToCamera / 1000f) * scrollModifier;
				distanceToCamera = Mathf.Clamp(distanceToCamera, 1f, 1000f);
				base.transform.position = base.transform.localRotation * (Vector3.forward * (0f - distanceToCamera)) + pivot;
			}
			bool flag = editor == null || editor.EnableCameraControls();
			if (!currentActionValid || (flag && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2) && !Input.GetKey(KeyCode.LeftAlt)))
			{
				if (new Rect(0f, 0f, Screen.width, Screen.height).Contains(Input.mousePosition))
				{
					prev_mousePosition = Input.mousePosition;
				}
				return;
			}
			if (Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftAlt))
			{
				cameraState = ViewTool.Look;
				eatMouse = true;
				float axis = Input.GetAxis("Mouse X");
				float axis2 = Input.GetAxis("Mouse Y");
				Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
				eulerAngles.x -= axis2 * lookSpeed;
				eulerAngles.y += axis * lookSpeed;
				eulerAngles.z = 0f;
				base.transform.localRotation = Quaternion.Euler(eulerAngles);
				float d = moveSpeed * Time.deltaTime;
				base.transform.position += base.transform.forward * d * Input.GetAxis("Vertical");
				base.transform.position += base.transform.right * d * Input.GetAxis("Horizontal");
				try
				{
					base.transform.position += base.transform.up * d * Input.GetAxis("CameraUp");
				}
				catch
				{
					Debug.LogWarning("CameraUp input is not configured.  Open \"Edit/Project Settings/Input\" and add an input named \"CameraUp\", mapping q and e to Negative and Positive buttons.");
				}
				pivot = base.transform.position + base.transform.forward * distanceToCamera;
			}
			else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
			{
				cameraState = ViewTool.Orbit;
				eatMouse = true;
				float axis3 = Input.GetAxis("Mouse X");
				float num2 = 0f - Input.GetAxis("Mouse Y");
				Vector3 eulerAngles2 = base.transform.localRotation.eulerAngles;
				if ((Mathf.Approximately(eulerAngles2.x, 90f) && num2 > 0f) || (Mathf.Approximately(eulerAngles2.x, 270f) && num2 < 0f))
				{
					num2 = 0f;
				}
				eulerAngles2.x += num2 * orbitSpeed;
				eulerAngles2.y += axis3 * orbitSpeed;
				eulerAngles2.z = 0f;
				base.transform.localRotation = Quaternion.Euler(eulerAngles2);
				base.transform.position = CalculateCameraPosition(pivot);
			}
			else if (Input.GetMouseButton(2) || (Input.GetMouseButton(0) && flag))
			{
				cameraState = ViewTool.Pan;
				Vector2 vector = Input.mousePosition - prev_mousePosition;
				vector.x = ScreenToWorldDistance(vector.x, distanceToCamera);
				vector.y = ScreenToWorldDistance(vector.y, distanceToCamera);
				base.transform.position -= base.transform.right * vector.x;
				base.transform.position -= base.transform.up * vector.y;
				pivot = base.transform.position + base.transform.forward * distanceToCamera;
			}
			prev_mousePosition = Input.mousePosition;
		}

		private Vector3 CalculateCameraPosition(Vector3 target)
		{
			return base.transform.localRotation * (Vector3.forward * (0f - distanceToCamera)) + target;
		}

		public static void Focus(Vector3 target)
		{
			instance.ZoomInternal(target, 10f);
		}

		public static void Focus(Vector3 target, float distance)
		{
			instance.ZoomInternal(target, distance);
		}

		public static void Focus(GameObject target)
		{
			instance.ZoomInternal(target);
		}

		private void ZoomInternal(Vector3 target, float distance)
		{
			pivot = target;
			distanceToCamera = distance;
			previousPosition = base.transform.position;
			targetPosition = CalculateCameraPosition(pivot);
			zoomProgress = 0f;
			zooming = true;
		}

		private void ZoomInternal(GameObject target)
		{
			Vector3 position = target.transform.position;
			Renderer component = target.GetComponent<Renderer>();
			Bounds bounds = (component != null) ? component.bounds : new Bounds(position, Vector3.one * 10f);
			distanceToCamera = pb_ObjectUtility.CalcMinDistanceToBounds(cam, bounds);
			distanceToCamera += distanceToCamera;
			position = bounds.center;
			ZoomInternal(position, distanceToCamera);
		}

		private float ScreenToWorldDistance(float screenDistance, float distanceFromCamera)
		{
			Vector3 a = cam.ScreenToWorldPoint(Vector3.forward * distanceFromCamera);
			Vector3 b = cam.ScreenToWorldPoint(new Vector3(screenDistance, 0f, distanceFromCamera));
			return CopySign(Vector3.Distance(a, b), screenDistance);
		}

		private float CopySign(float x, float y)
		{
			if ((x < 0f && y < 0f) || (x > 0f && y > 0f) || x == 0f || y == 0f)
			{
				return x;
			}
			return 0f - x;
		}
	}
}
