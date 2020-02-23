using System.Collections.Generic;
using UnityEngine;

namespace Pixelplacement
{
	public class Chooser : MonoBehaviour
	{
		public enum Method
		{
			Raycast,
			RaycastAll
		}

		public GameObjectEvent OnSelected;

		public GameObjectEvent OnDeselected;

		public GameObjectEvent OnPressed;

		public GameObjectEvent OnReleased;

		public bool _cursorPropertiesFolded;

		public bool _unityEventsFolded;

		public Transform source;

		public float raycastDistance = 3f;

		public LayerMask layermask = -1;

		public KeyCode[] pressedInput;

		public Transform cursor;

		public float surfaceOffset;

		public float idleDistance = 3f;

		public float stabilityDelta = 0.0127f;

		public float snapDelta = 1f;

		public float stableSpeed = 2f;

		public float unstableSpeed = 20f;

		public bool flipForward;

		public bool matchSurfaceNormal = true;

		public bool autoHide;

		public bool cursorHidden;

		public bool flipCastDirection;

		public LineRenderer lineRenderer;

		[SerializeField]
		private Method _method;

		[SerializeField]
		private bool _debugView;

		private Transform _previousCursor;

		private List<Transform> _current = new List<Transform>();

		private List<Transform> _previous = new List<Transform>();

		private Transform _currentRaycast;

		private Transform _previousRaycast;

		private Vector3 _targetPosition;

		private bool _hidden;

		public Transform[] Current => _current.ToArray();

		public bool IsHitting
		{
			get;
			private set;
		}

		private void Reset()
		{
			source = base.transform;
			pressedInput = new KeyCode[1]
			{
				KeyCode.Mouse0
			};
		}

		private void OnEnable()
		{
			if (source == null)
			{
				source = base.transform;
			}
			if (cursor != null)
			{
				cursor.position = source.position;
				cursor.gameObject.SetActive(value: true);
			}
			if (lineRenderer != null)
			{
				lineRenderer.positionCount = 0;
				lineRenderer.enabled = true;
			}
		}

		private void OnDisable()
		{
			if (cursor != null)
			{
				cursor.gameObject.SetActive(value: false);
			}
			if (lineRenderer != null)
			{
				lineRenderer.enabled = false;
			}
		}

		private void OnDrawGizmosSelected()
		{
			if (!Application.isPlaying)
			{
				Vector3 forward = source.forward;
				if (flipCastDirection)
				{
					forward *= -1f;
				}
				Gizmos.color = Color.green;
				Gizmos.DrawRay(source.position, forward * raycastDistance);
				if (cursor != null)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawLine(source.position, cursor.position);
				}
			}
		}

		public void Pressed()
		{
			switch (_method)
			{
			case Method.Raycast:
				if (_currentRaycast != null)
				{
					_currentRaycast.SendMessage("Pressed", SendMessageOptions.DontRequireReceiver);
					if (OnPressed != null)
					{
						OnPressed.Invoke(_currentRaycast.gameObject);
					}
				}
				break;
			case Method.RaycastAll:
				if (_current.Count > 0)
				{
					foreach (Transform item in _current)
					{
						item.SendMessage("Pressed", SendMessageOptions.DontRequireReceiver);
						if (OnPressed != null)
						{
							OnPressed.Invoke(item.gameObject);
						}
					}
				}
				break;
			}
		}

		public void Released()
		{
			switch (_method)
			{
			case Method.Raycast:
				if (_currentRaycast != null)
				{
					_currentRaycast.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
					if (OnReleased != null)
					{
						OnReleased.Invoke(_currentRaycast.gameObject);
					}
				}
				break;
			case Method.RaycastAll:
				if (_current.Count > 0)
				{
					foreach (Transform item in _current)
					{
						item.SendMessage("Released", SendMessageOptions.DontRequireReceiver);
						if (OnReleased != null)
						{
							OnReleased.Invoke(item.gameObject);
						}
					}
				}
				break;
			}
		}

		private void Update()
		{
			if (cursor != _previousCursor)
			{
				_previousCursor = cursor;
				if (cursor == null)
				{
					return;
				}
				Collider[] componentsInChildren = cursor.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsInChildren)
				{
					Debug.Log("Cursor can not contain colliders. Disabling colliders on: " + collider.name);
					collider.enabled = false;
				}
			}
			if (pressedInput != null)
			{
				KeyCode[] array = pressedInput;
				foreach (KeyCode key in array)
				{
					if (Input.GetKeyDown(key))
					{
						Pressed();
					}
					if (Input.GetKeyUp(key))
					{
						Released();
					}
				}
			}
			_current.Clear();
			Vector3 forward = source.forward;
			if (flipCastDirection)
			{
				forward *= -1f;
			}
			Physics.Raycast(source.position, forward, out RaycastHit hitInfo, raycastDistance, layermask);
			_currentRaycast = hitInfo.transform;
			IsHitting = (hitInfo.transform != null);
			if (_method == Method.Raycast && IsHitting)
			{
				_current.Clear();
				_current.Add(hitInfo.transform);
			}
			if (_debugView)
			{
				if (hitInfo.transform != null)
				{
					Debug.DrawLine(source.position, hitInfo.point, Color.green);
				}
				else
				{
					Debug.DrawRay(source.position, forward * raycastDistance, Color.red);
				}
			}
			if (cursor != null)
			{
				if (cursorHidden)
				{
					cursor.gameObject.SetActive(value: false);
				}
				else if (autoHide)
				{
					cursor.gameObject.SetActive(IsHitting);
					if (lineRenderer != null)
					{
						lineRenderer.enabled = IsHitting;
					}
				}
				else
				{
					cursor.gameObject.SetActive(value: true);
					if (lineRenderer != null)
					{
						lineRenderer.enabled = true;
					}
				}
			}
			if (cursor != null)
			{
				if (hitInfo.transform != null)
				{
					_targetPosition = hitInfo.point + hitInfo.normal * surfaceOffset;
					float num = unstableSpeed;
					float num2 = Vector3.Distance(_targetPosition, cursor.position);
					if (num2 <= stabilityDelta)
					{
						num = stableSpeed;
					}
					if (num2 >= snapDelta)
					{
						cursor.position = _targetPosition;
					}
					else
					{
						cursor.position = Vector3.Lerp(cursor.position, _targetPosition, Time.unscaledDeltaTime * num);
					}
					if (matchSurfaceNormal)
					{
						cursor.rotation = Quaternion.LookRotation(hitInfo.normal, source.up);
					}
					else
					{
						cursor.LookAt(source, Vector3.up);
					}
					if (flipForward)
					{
						cursor.Rotate(Vector3.up * 180f);
					}
				}
				else
				{
					Vector3 vector = source.position + forward * idleDistance;
					float num3 = Vector3.Distance(vector, cursor.position);
					float num4 = unstableSpeed;
					if (num3 <= stabilityDelta)
					{
						num4 = stableSpeed;
					}
					if (num3 >= snapDelta)
					{
						cursor.position = vector;
					}
					else
					{
						cursor.position = Vector3.Lerp(cursor.position, vector, Time.unscaledDeltaTime * num4);
					}
					cursor.LookAt(source.position);
					if (flipForward)
					{
						cursor.Rotate(Vector3.up * 180f);
					}
				}
			}
			if (_method == Method.Raycast)
			{
				if (_previousRaycast == null && hitInfo.transform != null)
				{
					hitInfo.transform.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
					if (OnSelected != null)
					{
						OnSelected.Invoke(hitInfo.transform.gameObject);
					}
				}
				if (hitInfo.transform != null && _previousRaycast != null && _previousRaycast != hitInfo.transform)
				{
					_previousRaycast.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
					if (OnDeselected != null)
					{
						OnDeselected.Invoke(_previousRaycast.gameObject);
					}
					hitInfo.transform.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
					if (OnSelected != null)
					{
						OnSelected.Invoke(hitInfo.transform.gameObject);
					}
				}
				if (_previousRaycast != null && hitInfo.transform == null)
				{
					_previousRaycast.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
					if (OnDeselected != null)
					{
						OnDeselected.Invoke(_previousRaycast.gameObject);
					}
				}
				_previousRaycast = hitInfo.transform;
			}
			if (_method == Method.RaycastAll)
			{
				RaycastHit[] array2 = Physics.RaycastAll(source.position, forward, raycastDistance, layermask);
				foreach (RaycastHit raycastHit in array2)
				{
					_current.Add(raycastHit.transform);
				}
				if (_current.Count > 0)
				{
					foreach (Transform item in _current)
					{
						if (_previous.Count == 0 || !_previous.Contains(item))
						{
							item.SendMessage("Selected", SendMessageOptions.DontRequireReceiver);
							if (OnSelected != null)
							{
								OnSelected.Invoke(item.gameObject);
							}
						}
					}
				}
				if (_previous.Count > 0)
				{
					foreach (Transform previou in _previous)
					{
						if (_current.Count == 0 || !_current.Contains(previou))
						{
							previou.SendMessage("Deselected", SendMessageOptions.DontRequireReceiver);
							if (OnDeselected != null)
							{
								OnDeselected.Invoke(previou.gameObject);
							}
						}
					}
				}
				_previous.Clear();
				_previous.AddRange(_current);
			}
			if (cursor != null && cursor.gameObject.activeSelf && lineRenderer != null)
			{
				if (lineRenderer.positionCount != 2)
				{
					lineRenderer.positionCount = 2;
				}
				lineRenderer.SetPosition(0, source.position);
				lineRenderer.SetPosition(1, cursor.position);
			}
		}
	}
}
